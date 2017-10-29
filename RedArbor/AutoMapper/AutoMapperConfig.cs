using AutoMapper;
using RedArbor.DTO;
using RedArbor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RedArbor.AutoMapper
{
    public class AutoMapperConfig
    {
        public static void AutoMapper()
        {
            AutoMapperDTO();
        }

        public static void AutoMapperInverse()
        {
            AutoMapperInverseDTO();
        }

        private static void AutoMapperDTO()
        {
            // Empleados

            //var config = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<Employee, EmployeeBaseDTO>();

            //});
            //config.CreateMapper();

            Mapper.CreateMap<Employee, EmployeeBaseDTO>();
        }

        private static void AutoMapperInverseDTO()
        {
            // Empleados

            //var config = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<EmployeeBaseDTO, Employee>();

            //});
            //config.CreateMapper();

            Mapper.CreateMap<EmployeeBaseDTO, Employee>();
        }
    }
}