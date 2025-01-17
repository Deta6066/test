using Microsoft.AspNetCore.Mvc;
using VisLibrary.Business.Interface;
using VisLibrary.Models;
using VisLibrary.Service.Interface;

namespace VIS_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IOrder _orderService;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(IOrder orderService, ILogger<WeatherForecastController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet("GetWeather", Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

       
    }
}
