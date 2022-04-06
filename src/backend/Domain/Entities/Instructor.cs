using Domain.Common;
using System;

namespace Domain.Entities
{
    public class Instructor : AuditableEntity
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string EmailNormalize { get; set; }

        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string Bio { get; set; }

        public string WalletAddress { get; set; }

        public string ProfilePictureFilename { get; set; }

        public bool EmailConfirmed { get; set; }
        public DateTime? EmailConfirmedAt { get; set; }

        public DateTime DateRegistered { get; set; }
    }
}
