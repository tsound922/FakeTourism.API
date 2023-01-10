using FakeTourism.API.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeTourism.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class RootController: ControllerBase
    {
        [HttpGet(Name = "GetRoot")]
        public IActionResult GetRoot() 
        {
            var links = new List<LinkDto>();

            //Self links
            links.Add(
                new LinkDto(
                Url.Link("GetRoot", null),
                "self",
                "GET"
            ));

            //1st level links Tourist Route "GET api/touristRoutes"
            links.Add(
                new LinkDto(
                Url.Link("GetTouristRoutes", null),
                "get_tourist_routes",
                "GET"
            ));

            //1st level links Tourist Route "POST api/tourist"
            links.Add(
                new LinkDto(
                Url.Link("CreateTouristRoute", null),
                "create_tourist_routes",
                "POST"
            ));

            //1st level links "GET api/shoppingCart"
            links.Add(
                new LinkDto(
                Url.Link("GetShoppingCart", null),
                "get_shopping_cart",
                "POST"
            ));

            //1st level links "GET api/orders"
            links.Add(
                new LinkDto(
                Url.Link("GetOrders", null),
                "get_orders",
                "POST"
            ));

            return Ok(links);
        }
    }
}
