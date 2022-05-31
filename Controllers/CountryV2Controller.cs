using AutoMapper;
using HotelListing.api.DTO;
using HotelListing.api.IRepository;
using HotelListing.api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.api.Controllers
{
    //[ApiVersion("2.0")]
    [ApiVersion("2.0", Deprecated = true)]
    //[Route("api/country")]
    [Route("api/{v:apiversion}/country")]
    [ApiController]
    public class CountryV2Controller : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CountryController> _logger;
        private readonly IMapper _mapper;
        public CountryV2Controller(IUnitOfWork unitOfWork, ILogger<CountryController> logger,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;

        }
        // GET api/country
        [HttpGet]
        [ResponseCache(Duration = 60)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountries([FromQuery] RequestParams requestParams)
        {
            //try
            //{
                var countries = await _unitOfWork.Countries.GetAll(requestParams, null);
                var results = _mapper.Map<List<CountryDTO>>(countries);
                return Ok(results);
            //}
            //catch (Exception ex)
            //{
                //_logger.LogError(ex, $"Something went wrong in the {nameof(GetCountries)}");
                //return StatusCode(500, "Internal Server Error, Please try again later.");
            //}
        }
        // GET api/country/5
        [Authorize]
        [HttpGet("{id}", Name ="GetCountry")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountry(int id){
            //try
            //{
                var country = await _unitOfWork.Countries.Get(q => q.Id == id, new List<string> { "Hotels" });
                var results = _mapper.Map<CountryDTO>(country);
                return Ok(results);
            //}
            //catch(Exception ex)
            //{
                //_logger.LogError(ex, $"Something went wrong in the {nameof(GetCountry)}");
                //return StatusCode(500, "Internal Server Error, Please try again later.");
            //}
        }

        // POST api/hotel/
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCountry([FromBody] CreateCountryDTO countryDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(CreateCountry)}");
                return BadRequest(ModelState);
            }

            //try
            //{
                var country =  _mapper.Map<Country>(countryDTO);
                await _unitOfWork.Countries.Insert(country);
                await _unitOfWork.Save();
                
                return CreatedAtRoute("GetCountry", new { id = country.Id }, country);
            //}
            //catch (Exception ex)
            //{
                //_logger.LogError(ex, $"Something went wrong in the {nameof(CreateCountry)}");
                //return StatusCode(500, "Internal Server Error, Please try again later.");
            //}
        }

        // DELETE api/country/1
        [Authorize(Roles = "Administrator")]
        [HttpPost("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteCountry)}");
                return BadRequest();
            }

            //try
            //{
                var country =  await _unitOfWork.Countries.Get(q => q.Id == id);
                if (country == null)
                {
                     _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteCountry)}");
                    return BadRequest("Submitted data is invalid");
                }
                await _unitOfWork.Countries.Delete(id);
                await _unitOfWork.Save();
                
                return NoContent();
            //}
            //catch (Exception ex)
            //{
                //_logger.LogError(ex, $"Something went wrong in the {nameof(DeleteCountry)}");
                //return StatusCode(500, "Internal Server Error, Please try again later.");
            //}
        }

        // UPDATE/PUT api/country/1
        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCountry(int id, [FromBody] UpdateCountryDTO countryDTO)
        {   
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateCountry)}");
                return BadRequest(ModelState);
            }

            //try
            //{
                var country =  await _unitOfWork.Countries.Get(q => q.Id == id);
                if (country == null)
                {
                     _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateCountry)}");
                    return BadRequest("Submitted data is invalid");
                }

                _mapper.Map(countryDTO, country);
                _unitOfWork.Countries.Update(country);
                await _unitOfWork.Save();
                
                return NoContent();
            //}
            //catch (Exception ex)
            //{
                //_logger.LogError(ex, $"Something went wrong in the {nameof(UpdateCountry)}");
                //return StatusCode(500, "Internal Server Error, Please try again later.");
            //}
        }
    }
}