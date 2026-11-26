namespace TecnoEdu.Models.ViewModels
{
    public class InstituteDirectoryViewModel
    {
        public string Search { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public List<string> Cities { get; set; } = new List<string>();
        public List<InstituteCard> Institutes { get; set; } = new List<InstituteCard>();
    }
}
