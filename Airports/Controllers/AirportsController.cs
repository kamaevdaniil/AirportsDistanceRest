using Airports.Model;
using Microsoft.AspNetCore.Mvc;
using System.Device.Location;

namespace Airports.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirportsController : Controller
    {

        /*private double GetDistance( Location first,Location second)
        {
            var FirstPlace= new GeoCoordinate();
            FirstPlace.Latitude = 55.7522;
            FirstPlace.Longitude = 37.6156;

            var SecondPlace = new GeoCoordinate();
            SecondPlace.Latitude = 59.89444;
            SecondPlace.Longitude = 30.26417;


            var distance  = FirstPlace.GetDistanceTo(SecondPlace);
            

            return distance;
        }*/




        private static readonly HttpClient client = new HttpClient();
        private static async Task<string> ProcessPlaceAirports(string iata)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            var stringTask = client.GetStringAsync($"https://places-dev.cteleport.com/airports/{iata.ToUpper()}");
            var msg = await stringTask;
            return msg;
        }


        [HttpGet("{iata}")]

        public async Task<ActionResult<Airport>> Get(string iata)
        {
            var json = await ProcessPlaceAirports(iata);
            Airport? restoredAirports = Airport.FromJson(json);
           
            
            return Ok(restoredAirports);
        }

        [HttpGet("{firts},{second}")]

        public async Task<ActionResult<string>> GetDistance(string firts,string second)
        {
            var json = await ProcessPlaceAirports(firts);
            var json2 = await ProcessPlaceAirports(second);

            Airport? PointOne = Airport.FromJson(json);
            Airport? PointTwo = Airport.FromJson(json2);

            //var dis = GetDistance(PointOne.Location,PointTwo.Location);
            var FirstPlace = new GeoCoordinate();
            FirstPlace.Latitude = PointOne.Location.Lat;
            FirstPlace.Longitude = PointOne.Location.Lon;

            var SecondPlace = new GeoCoordinate();
            SecondPlace.Latitude = PointTwo.Location.Lat;
            SecondPlace.Longitude = PointTwo.Location.Lon;


            var distance = FirstPlace.GetDistanceTo(SecondPlace);


            int rezult = Convert.ToInt32(distance) / 1000;
            return Ok("От\n" +
                $"Type:{PointOne.Type} \n" +
                $"Code IATA: {PointOne.Iata}\n" + 
                $"Name Country: {PointOne.Country}\n" +
                $"Name City: {PointOne.Name}" +
                "\n" +
                "До\n" +
                $"Type:{PointTwo.Type} \n" +
                $"Code IATA: {PointTwo.Iata} \n" +
                $"Name Country: {PointTwo.Country}\n" +
                $"Name City: {PointTwo.Name}\n" +
                $"Расстояние {rezult} км \n");
        }

    }
}
