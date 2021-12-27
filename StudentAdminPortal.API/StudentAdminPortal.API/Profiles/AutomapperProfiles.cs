using AutoMapper;
using DataModels = StudentAdminPortal.API.DataModels;
using StudentAdminPortal.API.DomainModels;
using System;

namespace StudentAdminPortal.API.Profiles
{
    public class AutomapperProfiles: Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<DataModels.Student, Student>()
            .ReverseMap();

            CreateMap<DataModels.Gender, Gender>()
            .ReverseMap();

            CreateMap<DataModels.Address, Address>()
            .ReverseMap();
        }
    }
}
