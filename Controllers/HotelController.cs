using AutoMapper;
using HotelListing.api.DTO;
using HotelListing.api.IRepository;
using HotelListing.api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.api.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CountryController> _logger;
        private readonly IMapper _mapper;
        public HotelController(IUnitOfWork unitOfWork, ILogger<CountryController> logger,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            
        }
        // GET api/hotel
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotels(){
            try
            {
                var hotels = await _unitOfWork.Hotels.GetAll();
                var results = _mapper.Map<List<HotelDTO>>(hotels);
                return Ok(results);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetHotels)}");
                return StatusCode(500, "Internal Server Error, Please try again later.");
            }
        }
        
        // GET api/hotel
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotels([FromQuery] RequestParams requestParams)
        {
            try
            {
                var hotels = await _unitOfWork.Countries.GetAll(requestParams, null);
                var results = _mapper.Map<List<CountryDTO>>(hotels);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetHotels)}");
                return StatusCode(500, "Internal Server Error, Please try again later.");
            }
        }
        
        // GET api/hotel/5
        [Authorize]
        [HttpGet("{id}", Name ="GetHotel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotel(int id){
            try
            {
                var hotel = await _unitOfWork.Hotels.Get(q => q.Id == id, new List<string> { "Country" });
                var results = _mapper.Map<HotelDTO>(hotel);
                return Ok(results);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetHotel)}");
                return StatusCode(500, "Internal Server Error, Please try again later.");
            }
        }

        // POST api/hotel/
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateHotel([FromBody] CreateHotelDTO hotelDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(CreateHotel)}");
                return BadRequest(ModelState);
            }

            try
            {
                var hotel =  _mapper.Map<Hotel>(hotelDTO);
                await _unitOfWork.Hotels.Insert(hotel);
                await _unitOfWork.Save();
                
                return CreatedAtRoute("GetHotel", new { id = hotel.Id }, hotel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(CreateHotel)}");
                return StatusCode(500, "Internal Server Error, Please try again later.");
            }
        }

        // DELETE api/country/1
        [Authorize(Roles = "Administrator")]
        [HttpPost("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteHotel)}");
                return BadRequest();
            }

            try
            {
                var hotel =  await _unitOfWork.Hotels.Get(q => q.Id == id);
                if (hotel == null)
                {
                     _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteHotel)}");
                    return BadRequest("Submitted data is invalid");
                }
                await _unitOfWork.Hotels.Delete(id);
                await _unitOfWork.Save();
                
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteHotel)}");
                return StatusCode(500, "Internal Server Error, Please try again later.");
            }
        }
    }
}