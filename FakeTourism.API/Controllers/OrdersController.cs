using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using FakeTourism.API.Services;
using AutoMapper;
using FakeTourism.API.Dtos;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FakeTourism.API.Controllers
{
    [ApiController]
    [Route("api/orders")]

    public class OrdersController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;

        public OrdersController(
            IHttpContextAccessor httpContextAccessor,
            ITouristRouteRepository touristRouteRepository,
            IMapper mapper,
            IHttpClientFactory httpClientFactory
        ) 
        {
            _httpContextAccessor = httpContextAccessor;
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
        }


        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetOrders() 
        {
            //1 acquire current user from HTTP Context
            var userId = _httpContextAccessor
                .HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //2 get histroy order from user id
            var orders = await _touristRouteRepository.GetOrdersByUserId(userId);
            return Ok(_mapper.Map<IEnumerable<OrderDto>>(orders));
        }

        [HttpGet("{orderId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetOrderById([FromRoute]Guid orderId) 
        {
            //1 acquire current user from HTTP Context
            var userId = _httpContextAccessor
                .HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var order = await _touristRouteRepository.GetOrderById(orderId);
            return Ok(_mapper.Map<OrderDto>(order));
        }

        [HttpPost("{orderId}/placeOrder")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PlaceOrder([FromRoute]Guid orderId) 
        {
            //1 acquire current user from HTTP Context
            var userId = _httpContextAccessor
                .HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //2 start pre-process with current order find by orderId
            var order = await _touristRouteRepository.GetOrderById(orderId);
            order.PaymentProcessing();
            await _touristRouteRepository.SaveAsync();

            //3 send request to 3rd party payment org (their URL) and wait for response
            var httpClient = _httpClientFactory.CreateClient();
            string url = @"http://123.56.149.216/api/FakePaymentProcess?icode={0}&orderNumber={1}&returnFault={2}";
            var response = await httpClient.PostAsync(
                string.Format(url, "877D2DC1F09FAD57", order.Id, false),
                null
                );

            //4 get payment result and information
            bool isApproved = false;
            string transactionMetaData = "";
            if (response.IsSuccessStatusCode) 
            {
                transactionMetaData = await response.Content.ReadAsStringAsync();
                var jsonObject = (JObject)JsonConvert.DeserializeObject(transactionMetaData);
                isApproved = jsonObject["approved"].Value<bool>();

            }

            //5 complete the order if payment is success
            if (isApproved)
            {
                order.PaymentApproved();
            }
            else 
            {
                order.PaymentRejected();
            }
            order.TransactionMetaData = transactionMetaData;

            await _touristRouteRepository.SaveAsync();

            return Ok(_mapper.Map<OrderDto>(order));
        }


    }
}
