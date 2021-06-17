using System;

namespace ApiValidation.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string EncryptedPassword { get; set; }
    }
}
