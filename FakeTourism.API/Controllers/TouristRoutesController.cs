using FakeTourism.API.Dtos;
using FakeTourism.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using System.Text.RegularExpressions;
using FakeTourism.API.ResourceParameters;
using FakeTourism.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using FakeTourism.API.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace FakeTourism.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TouristRoutesController : ControllerBase
    {
        private ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;
        private readonly IUrlHelper _urlHelper;

        //Dependency injection
        public TouristRoutesController(
            ITouristRouteRepository touristRouteRepository, 
            IMapper mapper, 
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor
            ) 
        {
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        private string GenerateTouristRouteResourceURL(
            TouristRouteResourceParamaters paramaters,
            PaginationResourceParamaters pageParamaters,
            ResourceUriType type
            ) 
        {
            return type switch
            {
                ResourceUriType.PreviousPage => _urlHelper.Link("GetTouristRoutes",
                    new
                    {
                        keyword = paramaters.Keyword,
                        rating = paramaters.Rating,
                        pageNumber = pageParamaters.PageNumber - 1,
                        pageSize = pageParamaters.PageSize
                    }),
                ResourceUriType.NextPage => _urlHelper.Link("GetTouristRoutes",
                    new
                    {
                        keyword = paramaters.Keyword,
                        rating = paramaters.Rating,
                        pageNumber = pageParamaters.PageNumber + 1,
                        pageSize = pageParamaters.PageSize
                    }),
                _ => _urlHelper.Link("GetTouristRoutes",
                    new
                    {
                        keyword = paramaters.Keyword,
                        rating = paramaters.Rating,
                        pageNumber = pageParamaters.PageNumber,
                        pageSize = pageParamaters.PageSize
                    })
            };
        }
        [HttpGet(Name = "GetTouristRoutes")]
        [HttpHead]
        public async Task<IActionResult> GetTouristRoutes(
            [FromQuery]TouristRouteResourceParamaters paramaters,
            [FromQuery]PaginationResourceParamaters pageParamaters 
            /*[FromQuery] string keyword,
            string rating //less then, larger than, equal to*/
            ) {

            

            var touristRoutesFromRepo = await _touristRouteRepository.GetTouristRoutesAsync(
                paramaters.Keyword, 
                paramaters.RatingOperator, 
                paramaters.RatingValue,
                pageParamaters.PageSize,
                pageParamaters.PageNumber,
                paramaters.OrderBy
                );
            if (touristRoutesFromRepo == null || touristRoutesFromRepo.Count() <= 0) {
                return NotFound("No Tourist routes");
            }

            var touristRouteDto = _mapper.Map<IEnumerable<TouristRouteDto>>(touristRoutesFromRepo);

            var previousPageLink = touristRoutesFromRepo.HasPrevious
                ? GenerateTouristRouteResourceURL(paramaters, pageParamaters, ResourceUriType.PreviousPage)
                : null;

            var nextPageLink = touristRoutesFromRepo.HasNext
                ? GenerateTouristRouteResourceURL(paramaters, pageParamaters, ResourceUriType.NextPage)
                : null;

            // Add x-pagination at header
            var paginationMetaData = new
            {
                previousPageLink,
                nextPageLink,
                totalCount = touristRoutesFromRepo.TotalCount,
                pageSize = touristRoutesFromRepo.PageSize,
                current = touristRoutesFromRepo.CurrentPage,
                totalPages = touristRoutesFromRepo.TotalPages
            };

            Response.Headers.Add("x-pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetaData));

            return Ok(touristRouteDto);
        }

        [HttpGet("{touristRouteId}", Name = "GetTouristRouteById")]
        [HttpHead("{touristRouteId}")]
        public async Task<IActionResult> GetTouristRouteById(Guid touristRouteId) {

            //It (touristRouteFromRepo) is the original data model
            var touristRouteFromRepo = await _touristRouteRepository.GetTouristRouteByIdAsync(touristRouteId);
            
            if (touristRouteFromRepo == null ) {
                return NotFound($"Tourist route {touristRouteId} is not found");
            }
            /*var touristRouteDto = new TouristRouteDto() {
                Id = touristRouteFromRepo.Id,
                Title = touristRouteFromRepo.Title,
                Description = touristRouteFromRepo.Description,
                Price = touristRouteFromRepo.OriginalPrice * (Decimal)(touristRouteFromRepo.DiscountPresent ?? 1),
                CreateTime = touristRouteFromRepo.CreateTime,
                UpdateTime = touristRouteFromRepo.UpdateTime,
                Features = touristRouteFromRepo.Features,
                Fees = touristRouteFromRepo.Fees,
                Notes = touristRouteFromRepo.Notes,
                Rating = touristRouteFromRepo.Rating,
                TravelDays = touristRouteFromRepo.TravelDays.ToString(),
                TripType = touristRouteFromRepo.TripType.ToString(),
                DepartureCity = touristRouteFromRepo.DepartureCity.ToString()
            };*/

            var touristRouteDto = _mapper.Map<TouristRouteDto>(touristRouteFromRepo);

            return Ok(touristRouteDto);
           
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        /*[Authorize(Roles = "Admin")]*/
        public async Task<IActionResult> CreateTouristRoute([FromBody] TouristRouteForCreationDto touristRouteForCreationDto) 
        {
            var touristRouteFromRepo = await _touristRouteRepository.GetTouristRouteByTitleAsync(touristRouteForCreationDto.Title);
            if (touristRouteFromRepo != null) 
            {
                return Conflict($"Tourist route {touristRouteForCreationDto.Title} is duplicated");
            }

            var touristRouteModel = _mapper.Map<TouristRoute>(touristRouteForCreationDto);
            _touristRouteRepository.AddTouristRoute(touristRouteModel);
            await _touristRouteRepository.SaveAsync();

            var touristRouteToReturn = _mapper.Map<TouristRouteDto>(touristRouteModel);

            return CreatedAtRoute("GetTouristRouteById", 
                new { touristRouteId = touristRouteToReturn.Id }, 
                touristRouteToReturn
            );
        }

        [HttpPut("{touristRouteId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateTouristRoute(
            [FromRoute] Guid touristRouteId,
            [FromBody] TouristRouteForUpdateDto touristRouteForUpdateDto
            ) 
        {
            if (!(await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId))) 
            {
                return NotFound("Tourist route is not found");
            }

            var touristRouteFromRepo = await _touristRouteRepository.GetTouristRouteByIdAsync(touristRouteId);
            // 1. Reflect DTO
            // 2. update DTO
            // 3. mapping model

            _mapper.Map(touristRouteForUpdateDto, touristRouteFromRepo);
            await _touristRouteRepository.SaveAsync();

            return NoContent();
        }
        [HttpPatch("{touristRouteId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PartiallyUpdateTouristRoute(
            [FromRoute]Guid touristRouteId,
            [FromBody] JsonPatchDocument<TouristRouteForUpdateDto> patchDocument
            ) 
        {
            if (!(await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId))) 
            {
                return NotFound("Tourist route is not found");
            }

            var touristRouteFromRepo = await _touristRouteRepository.GetTouristRouteByIdAsync(touristRouteId);
            var touristRouteToPatch = _mapper.Map<TouristRouteForUpdateDto>(touristRouteFromRepo);
            patchDocument.ApplyTo(touristRouteToPatch, ModelState);

            if (!TryValidateModel(touristRouteToPatch)) 
            {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(touristRouteToPatch, touristRouteFromRepo);
            await _touristRouteRepository.SaveAsync();

            return NoContent();

        }

        [HttpDelete("{touristRouteId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTouristRoute([FromRoute]Guid touristRouteId) 
        {
            if (!(await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId)))
            {
                return NotFound("Tourist route is not found");
            }


            var touristRoute = await _touristRouteRepository.GetTouristRouteByIdAsync(touristRouteId);
            _touristRouteRepository.DeleteTouristRoute(touristRoute);
            await _touristRouteRepository.SaveAsync();

            return Ok("Tourist route has been removed");
        }

        [HttpDelete("({touristIDs})")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteByIDs(
            [ModelBinder(BinderType = typeof(ArrayModelBinder))][FromRoute] IEnumerable<Guid> touristIDs
            ) 
        {
            if (touristIDs == null) 
            {
                return BadRequest();
            }

            var touristRoutesFromRepo = await _touristRouteRepository.GetTouristRoutesByIDListAsync(touristIDs);
            _touristRouteRepository.DeleteTouristRoutes(touristRoutesFromRepo);
            await _touristRouteRepository.SaveAsync();

            return Ok("Requested tourist routes have been deleted");
        }

    }
}
