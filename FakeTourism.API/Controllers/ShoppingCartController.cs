using AutoMapper;
using FakeTourism.API.Dtos;
using FakeTourism.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FakeTourism.API.Controllers
{
    [ApiController]
    [Route("api/shoppingCart")]
    public class ShoppingCartController: ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;

        public ShoppingCartController(
            IHttpContextAccessor httpContextAccessor,
            ITouristRouteRepository touristRouteRepository,
            IMapper mapper
            ) 
        {
            _httpContextAccessor = httpContextAccessor;
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;

        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetShoppingCart() 
        {
            //1 acquire current user from HTTP Context
            var userId = _httpContextAccessor
                .HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //2 use userId to acquire shopping cart
            var shoppingCart = await _touristRouteRepository.GetShoppingCartByUserId(userId);

            return Ok(_mapper.Map<ShoppingCartDto>(shoppingCart));
        }
    }
}
