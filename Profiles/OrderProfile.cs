using AutoMapper;
using TMS.Api.Models;
using TMS.Api.Models.Dto;

namespace TMS.Api.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<Order, OrderPatchDto>().ReverseMap();
        }
    }
}
