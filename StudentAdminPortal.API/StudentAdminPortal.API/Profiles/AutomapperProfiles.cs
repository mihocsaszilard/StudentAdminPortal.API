using AutoMapper;
using DataModels = StudentAdminPortal.API.DataModels;
using StudentAdminPortal.API.DomainModels;
using System;
using StudentAdminPortal.API.Profiles.AfterMaps;

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

            CreateMap<UpdateStudentRequest, DataModels.Student>()
                .AfterMap<UpdateStudentRequestAfterMap>();

            CreateMap<CreateStudentRequest, DataModels.Student>()
                .AfterMap<CreateStudentRequestAfterMap>();
        }
    }
}
