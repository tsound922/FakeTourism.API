using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeTourism.API.Models
{
    public enum TripType
    {
        HotelAndAttractions, //Hotel+travel point
        Group, //Group Travel
        PrivateGroup, //Private group travel
        BackPackTour, //Freeform travel
        SemiBackPackTour //Semi freeform travel
    }
}
