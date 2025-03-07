namespace NetBank.Core.Application.ViewModels.Role
{
    public class RolViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool HasError { get; set; }

        public string? Error { get; set; }
    }
}
