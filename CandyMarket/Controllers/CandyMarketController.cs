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

        [HttpPost("userId/{userId}/candy/{candyId}")]
        public IActionResult AddCandyToUser(int candyId, int userId)
        {
            var result = _repository.AddCandies(candyId, userId);
            if (result == null)
            {
                return NotFound("No candy found");
            }
            return Ok(result);
        }

        [HttpDelete("userId/{userId}/eat/{candyId}")]
        public IActionResult EatCandy(int userId, int candyId)
        {
            var result = _repository.Eat(userId, candyId);
            if (result == null)
            {
                return NotFound("No candy found; can't eat");
            }
            return Ok(result);
        }
    }
}