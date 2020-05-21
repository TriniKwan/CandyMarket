using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CandyMarket.DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CandyMarket.Controllers
{
    [Route("api/candymarket")]
    [ApiController]
    public class CandyMarketController : ControllerBase
    {
        CandyMarketRepository _repository = new CandyMarketRepository();

        [HttpGet("userid/{userId}")]
        public IActionResult GetUserWithCandyInfo(int userId)
        {
            var result = _repository.GetUserWithCandyInfo(userId);
            if (result == null)
            {
                return NotFound("No user found");
            }
            return Ok(result);
        }

        [HttpGet("candies")]
        public IActionResult GetAllCandies()
        {
            var result = _repository.GetAllCandies();
            if (result == null)
            {
                return NotFound("No candy found");
            }
            return Ok(result);
        }
    }
}