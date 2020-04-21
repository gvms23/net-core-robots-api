using AutoMapper;
using Kodo.Robots.Api.ViewModels;
using Kodo.Robots.Domain.Entities;

namespace Kodo.Robots.Api.AutoMapper
{
    internal class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<RobotViewModel, Robot>();
        }
    }
}