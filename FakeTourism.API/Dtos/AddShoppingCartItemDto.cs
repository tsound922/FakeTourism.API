using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeTourism.API.Dtos
{
    public class AddShoppingCartItemDto
    {
        public Guid TouristRouteId { get; set; }
    }
}
