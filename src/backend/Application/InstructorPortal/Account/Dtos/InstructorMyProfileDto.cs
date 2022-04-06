using Application.Common.Mappings;
using Application.InstructorPortal.Communities.Dtos;
using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Application.InstructorPortal.Account.Dtos
{
    public class InstructorMyProfileDto : IMapFrom<Instructor>
    {
        public string Email { get; set; }
        public DateTime DateRegistered { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string ProfilePictureUri { get; set; }
        public string Bio { get; set; }

        public List<InstructorCommunityDto> Communities { get; set; }
    }
}
