using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LabRequest.DomainModel.Entities;
using AutoMapper;
using LabRequest.Web.ViewModel;

namespace LabRequest.Web.Infrastracture
{
    public class AutoMapperHelper
    {
        public static TDest Map<TSource, TDest>(TSource viewmodel)
        {
            Mapper.CreateMap<TSource, TDest>();
            var result = Mapper.Map<TSource, TDest>(viewmodel);
            return result;
        }
    }
}