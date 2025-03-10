namespace NetBank.Core.Application.ViewModels.User
{
    public class UserViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Identification { get; set; }
        public string Email { get; set; }

        public bool IsActive { get; set; }

        public int Rol {  get; set; }
    }
}
