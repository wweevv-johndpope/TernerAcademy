using Application.Common.Mappings;
using Domain.Entities;
using System;

namespace Application.StudentPortal.Account.Dtos
{
    public class StudentMyProfileDto : IMapFrom<Student>
    {
        public string Email { get; set; }
        public DateTime DateRegistered { get; set; }
        public string Name { get; set; }
        public string ProfilePictureUri { get; set; }
    }
}
