﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FakeTourism.API.Dtos;
using FakeTourism.API.Models;

namespace FakeTourism.API.Profiles
{
    public class TouristRoutePictureProfile: Profile
    {
        public TouristRoutePictureProfile()
        {
            CreateMap<TouristRoutePicture, TouristRoutePictureDto>();
            CreateMap<TouristRoutePictureForCreationDto, TouristRoutePicture>();
            CreateMap<TouristRoutePicture, TouristRoutePictureForCreationDto>();
        }
    }
}
