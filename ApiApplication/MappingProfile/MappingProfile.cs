using ApiApplication.Models;
using ApiApplication.ModelsDTO.OrderDTO;
using AutoMapper;
using Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserForRegistrationDto, User>();
            CreateMap<OrderToCreateDto, Order>();
            CreateMap<OrderToUpdateDto, Order>();
                

            CreateMap<Phone, PhonesToShowDto>()
                .ForMember(e => e.CompanyName, opt => opt.MapFrom(src => src.Company.Name));
            CreateMap<Order, OrdersToShowDto>()
              .ForMember(e => e.Email, opt => opt.MapFrom(src => src.User.Email))
              .ForMember(e => e.Phones, opt => opt.MapFrom(src => src.Phones));

        }
    }
}
