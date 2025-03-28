﻿namespace NetBank.Core.Application.Dtos.Account
{
    public class AuthenticationResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string LastName { get; set; }
        public string Identification { get; set; }
        public int Rol { get; set; }
        public bool IsVerified { get; set; }

        public bool IsActive { get; set; }
        public bool HasError { get; set; }
        public string? Error { get; set; }
        
    }
}
