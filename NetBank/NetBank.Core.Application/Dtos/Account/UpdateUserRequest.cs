namespace NetBank.Core.Application.Dtos.Account
{
    public class UpdateUserRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string LastName { get; set; }

        public string Identification { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }

        public string OldPass { get; set; }
        public string Password { get; set; }

        public decimal? AditionalAmount { get; set; }
    }
}
