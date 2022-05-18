﻿namespace CC.IdentityService.Models.Requests
{
    public class RegisterRequest
    {
        public string? Username { get; set; }

        public string? Password { get; set; }

        public string? Company { get; set; }

        public string? Email { get; set; }

        public List<string> Scopes { get; set; } = new();
    }
}
