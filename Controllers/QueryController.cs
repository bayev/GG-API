using Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[Authorize]
    public class QueryController : Controller
    {
        private Context _context;
        private readonly UserManager<User> _userManager;
        public QueryController(Context context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("search/{queryString}")] //querystring iställer för all 
        public async Task<ActionResult> SearchProductResult([FromRoute] string queryString)
        {
            try
            {
                var q1 = _context.Products
                .Where(x => x.Category.Contains(queryString) || x.Name.Contains(queryString)).ToList();

                return Ok(q1);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Sorry, something happend. {ex.ToString()}" });
            }
        }
        //private static List<(DateTime date, float avgTemp)>

        //SortOnTemperature(string sensor)
        //{
        //    using (var db = new Library.Models.EFContext())
        //    {
        //        var query = db.Temperature
        //        .Where(t => t.PositionForReading == sensor)
        //        .GroupBy(t => t.Date.DateOfTemperature)
        //        .Select(g => new { date = g.Key, avgTemp = g.Average(t => t.TemperatureReading) })
        //        .OrderByDescending(x => x.avgTemp).AsEnumerable();



        //        List<(DateTime, float)> resultSet = new();



        //        foreach (var item in query)
        //        {
        //            resultSet.Add((item.date, item.avgTemp));
        //        }
        //        return resultSet;
        //    }

        //}
    }

    }