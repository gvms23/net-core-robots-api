using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Kodo.Robots.Api.ViewModels;
using Kodo.Robots.Domain.Entities;

namespace Kodo.Robots.Api.AutoMapper
{
    internal class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Robot, RobotViewModel>()
                .ForMember(viewModel => viewModel.Links, opt =>
                    opt.MapFrom((entity, vm, member, context) =>
                    {
                        string _type = context.Items["Route"] as string;
                        string _route = context.Items["Route"] as string;
                        List<dynamic> _actions = context.Items["Actions"] as List<dynamic>;

                        var _links = new List<HateoasLinkViewModel>();

                        _actions?.ForEach(_action =>
                        {
                            _links.Add(new HateoasLinkViewModel($"{_route}/{entity.Name}",
                                $"{_action.Action}_{_type}", $"{_action.Method}"));
                        });

                        return _links;
                    }));

            CreateMap<Robot, RobotSimplifiedViewModel>()
                .ForMember(vm => vm.Name,
                    opt => opt.MapFrom(e => e.Name ?? "[Empty Name]"))
                .ForMember(vm => vm.LastUpdate,
                    opt => opt.MapFrom(e => e.ModifiedDate.HasValue
                        ? e.ModifiedDate.Value.ToString("yyyy-MM-dd")
                        : null));
        }
    }
}