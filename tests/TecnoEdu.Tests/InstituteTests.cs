using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TecnoEdu.Data;

namespace TecnoEdu.Tests;

public class InstituteTests
{
    [Fact]
    public async Task ProfileLifecycleEnforcesOwnershipAndValidation()
    {
        await using var factory = new InstituteFactory();
        using var owner = factory.NewClient();
        using var other = factory.NewClient();
        using var guest = factory.NewClient();
        Assert.Equal(HttpStatusCode.Redirect, (await guest.GetAsync("/Institutes")).StatusCode);
        await Register(owner, "owner@example.com");
        await Register(other, "other@example.com");

        var invalid = await Post(owner, "/Institutes/Create", "/Institutes/Create", new() { ["Name"] = " " });
        Assert.Equal(HttpStatusCode.OK, invalid.StatusCode);
        Assert.Contains("The Name field is required.", await invalid.Content.ReadAsStringAsync());
        var profile = Profile();
        profile["OwnerId"] = "forged-owner";
        var created = await Post(owner, "/Institutes/Create", "/Institutes/Create", profile);
        Assert.Equal(HttpStatusCode.Redirect, created.StatusCode);
        var details = created.Headers.Location!.ToString();
        var id = details.Split('/').Last();
        Assert.Contains("Technical Institute", await owner.GetStringAsync(details));
        Assert.DoesNotContain("Technical Institute", await other.GetStringAsync("/Institutes"));
        foreach (var action in new[] { "Details", "Edit", "Delete" })
        {
            var denied = await other.GetAsync($"/Institutes/{action}/{id}");
            Assert.Equal(HttpStatusCode.Redirect, denied.StatusCode);
            Assert.StartsWith("http://localhost/Account/AccessDenied", denied.Headers.Location!.ToString());
        }
        foreach (var action in new[] { "Edit", "Delete" })
        {
            var denied = await Post(other, "/Institutes/Create", $"/Institutes/{action}/{id}", Profile());
            Assert.Equal(HttpStatusCode.Redirect, denied.StatusCode);
            Assert.Contains("AccessDenied", denied.Headers.Location!.ToString());
        }
        Assert.Equal(HttpStatusCode.BadRequest,
            (await owner.PostAsync($"/Institutes/Edit/{id}", new FormUrlEncodedContent(Profile()))).StatusCode);
        Assert.Equal(HttpStatusCode.Conflict,
            (await Post(owner, $"/Institutes/Edit/{id}", "/Institutes/Create", Profile())).StatusCode);
        profile["Name"] = "Updated Institute";
        profile["Id"] = id;
        var invalidEdit = Profile();
        invalidEdit["Id"] = id;
        invalidEdit["Email"] = "invalid-email";
        var editResponse = await Post(owner, $"/Institutes/Edit/{id}", $"/Institutes/Edit/{id}", invalidEdit);
        Assert.Equal(HttpStatusCode.OK, editResponse.StatusCode);
        Assert.Contains("The Email field is not a valid e-mail address.", await editResponse.Content.ReadAsStringAsync());
        Assert.Contains("Technical Institute", await owner.GetStringAsync(details));
        var mismatchedId = Profile();
        mismatchedId["Id"] = "999999";
        Assert.Equal(HttpStatusCode.NotFound,
            (await Post(owner, $"/Institutes/Edit/{id}", $"/Institutes/Edit/{id}", mismatchedId)).StatusCode);
        Assert.Equal(HttpStatusCode.Redirect,
            (await Post(owner, $"/Institutes/Edit/{id}", $"/Institutes/Edit/{id}", profile)).StatusCode);
        Assert.Contains("Updated Institute", await owner.GetStringAsync(details));
        Assert.Equal(HttpStatusCode.NotFound, (await owner.GetAsync("/Institutes/Details/999999")).StatusCode);
        Assert.Equal(HttpStatusCode.Redirect,
            (await Post(owner, $"/Institutes/Delete/{id}", $"/Institutes/Delete/{id}", new())).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await owner.GetAsync(details)).StatusCode);
    }

    [Fact]
    public async Task AccountFlowRejectsStudentsAndAdministratorRegistration()
    {
        await using var factory = new InstituteFactory();
        using var student = factory.NewClient();
        await Register(student, "student@example.com", "Student");
        Assert.Contains("AccessDenied", (await student.GetAsync("/Institutes")).Headers.Location!.ToString());
        Assert.Equal(HttpStatusCode.Forbidden, (await student.GetAsync("/Account/AccessDenied")).StatusCode);
        Assert.Equal(HttpStatusCode.Redirect,
            (await Post(student, "/", "/Account/Logout", new())).StatusCode);
        var login = await Post(student, "/Account/Login", "/Account/Login?returnUrl=https://example.com", new()
        {
            ["Email"] = "student@example.com", ["Password"] = "Password123!"
        });
        Assert.Equal(HttpStatusCode.Redirect, login.StatusCode);
        Assert.Equal("/", login.Headers.Location!.ToString());
        using var attacker = factory.NewClient();
        var response = await Post(attacker, "/Account/Register", "/Account/Register", new()
        {
            ["Email"] = "admin@example.com", ["Password"] = "Password123!",
            ["ConfirmPassword"] = "Password123!", ["AccountType"] = "Administrator"
        });
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("AccountType", await response.Content.ReadAsStringAsync());
        Assert.Contains("/Account/Login", (await attacker.GetAsync("/Institutes")).Headers.Location!.ToString());
    }

    private static Dictionary<string, string> Profile() => new()
    {
        ["Name"] = "Technical Institute", ["Description"] = "Technical education and training.",
        ["City"] = "La Paz", ["Address"] = "123 Main Street", ["Email"] = "contact@example.com", ["Phone"] = "+591 71234567"
    };

    private static async Task Register(HttpClient client, string email, string role = "Institute")
    {
        var response = await Post(client, "/Account/Register", "/Account/Register", new()
        {
            ["Email"] = email, ["Password"] = "Password123!", ["ConfirmPassword"] = "Password123!", ["AccountType"] = role
        });
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
    }

    private static async Task<HttpResponseMessage> Post(HttpClient client, string page, string action,
        Dictionary<string, string> values)
    {
        var html = await client.GetStringAsync(page);
        var token = Regex.Match(html, "name=\"__RequestVerificationToken\"[^>]*value=\"([^\"]+)\"");
        Assert.True(token.Success);
        values["__RequestVerificationToken"] = WebUtility.HtmlDecode(token.Groups[1].Value);
        return await client.PostAsync(action, new FormUrlEncodedContent(values));
    }
}

public class InstituteFactory : WebApplicationFactory<Program>
{
    private readonly string databaseName = $"TecnoEduTests_{Guid.NewGuid():N}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        builder.UseSetting("ConnectionStrings:DefaultConnection",
            $"Server=localhost;Database={databaseName};Trusted_Connection=True;TrustServerCertificate=True");
    }

    public HttpClient NewClient() => CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

    public override async ValueTask DisposeAsync()
    {
        using (var scope = Services.CreateScope())
            await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.EnsureDeletedAsync();
        await base.DisposeAsync();
    }
}
