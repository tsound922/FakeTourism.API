using AutoMapper;
using FakeTourism.API.Dtos;
using FakeTourism.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeTourism.API.Profiles
{
    public class ShoppingCartProfile : Profile
    {
        public ShoppingCartProfile() 
        {
            CreateMap<ShoppingCart, ShoppingCartDto>();
            CreateMap<LineItem, LineItemDto>();
        }
    }
}
