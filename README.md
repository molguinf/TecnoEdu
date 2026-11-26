# TecnoEdu

ASP.NET Core MVC application targeting .NET 10 with Identity and SQL Server.

## Run locally

Install the .NET 10 SDK and start SQL Server. The development connection in `appsettings.Development.json` uses `localhost`, Windows authentication and the `TecnoEdu` database. Adjust it to your SQL Server instance if needed.

```powershell
dotnet restore
dotnet run --launch-profile http
```

Open http://localhost:5161. Development startup applies migrations and creates the `Administrator`, `Institute` and `Student` roles. Register an `Institute` account, then select **My institute** to create a profile. Each account can own one institute profile.

For production, set `ConnectionStrings__DefaultConnection`, apply migrations before starting the application, and use HTTPS. The application never creates administrator accounts through public registration.

```powershell
dotnet tool install --global dotnet-ef --version 10.0.13
dotnet ef database update
```

The migration command uses `ASPNETCORE_ENVIRONMENT`, defaulting to `Development` when unset. Set it to `Production` when applying production migrations.

## Sprint 1: Rodolfo Wascar Jimenez Rios

- [Institute authorization and ownership](https://trello.com/c/lWjVCmBn): role policy, owner-filtered listing and direct ownership checks in the controller for details, editing and deletion.
- [Institute profile CRUD](https://trello.com/c/1Iv17APo): create, list, details, edit and delete views with server and browser validation, antiforgery and SQL persistence.

Identity, account forms, the database context and the initial migration provide the dependencies needed by these two tasks. Other sprint features are pending integration with the team.

| Route | Access |
| --- | --- |
| `/Account/Register` | Public; Institute or Student registration |
| `/Account/Login` | Public |
| `/Account/Logout` | Authenticated; POST only |
| `/Institutes` | Institute role; own profile only |
| `/Institutes/Create` | Institute role; one profile per account |
| `/Institutes/Details/{id}` | Institute owner |
| `/Institutes/Edit/{id}` | Institute owner |
| `/Institutes/Delete/{id}` | Institute owner; deletion requires POST |

The CRUD uses traditional constructors, the `Institute` model directly, explicit `[Bind]` lists and separate Razor forms, following the structure of the supplied `appMvcEf` example. Edit binds the identifier and checks it against the route. The owner is never bound from forms; ownership comes from the authenticated account. Administrator and Student accounts cannot use institute management routes. Public institute discovery belongs to the separate listing task.

## Verify

```powershell
dotnet build
dotnet test tests/TecnoEdu.Tests
```

Integration tests require local SQL Server with Windows authentication and permission to create and drop databases. Each test uses a unique `TecnoEduTests_` database and removes it afterwards. They exercise real Identity cookies, migrations, Razor forms, validation, CRUD, ownership restrictions, role restrictions, antiforgery and local login redirects.
