using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marnie.Model;
using Newtonsoft.Json;

namespace Marnie
{
    public class LocationService
    {
        public  Station GetNearestStation(double latitude, double longitude)
        {
            var marnieClient = new RestClient("http://marnie-001-site1.atempurl.com/api/Station");
            var request = new RestRequest(Method.GET);
            request.AddParameter("latitude", latitude);
            request.AddParameter("longitude", longitude);

            IRestResponse response = marnieClient.Execute(request);

            DStation station = JsonConvert.DeserializeObject<DStation>(response.Content);

            var newStation = new Station(station.Id, station.Name, station.Latitude, station.Longitude);
            return newStation;
            
            //get responce status code, then return true if succsessfull (between 200 and 299) else return false
            //var num = (int) response.StatusCode;
            //if (num >= 200 && num <= 299)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }

        private class DStation
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }
    }
}
