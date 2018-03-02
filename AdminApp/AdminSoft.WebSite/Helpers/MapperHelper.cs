using AutoMapper;
using AdminSoft.Domain.Employees;
using AdminSoft.WebSite.Models;
using AdminSoft.WebSite.Models.Dto;
using System;
using System.Web;
using System.Web.Mvc;

namespace AdminSoft.WebSite.Helpers
{
    public class MapperHelper
    {
        internal static IMapper mapper;
        static MapperHelper()
        {
            var config = new MapperConfiguration(x =>
            {
                // mapper models
                x.CreateMap<Employee, EmployeeViewModels>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
                x.CreateMap<EmployeeViewModels, Employee>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));


                // mapper apis
                x.CreateMap<Employee, Employeedto>()
                .ReverseMap();

            });
            mapper = config.CreateMapper();
        }

        public static T Map<T>(object source) {
            return mapper.Map<T>(source);
        }
        public static void Map(object source, object destination)
        {
            mapper.Map(source, destination);
        }
    }
}