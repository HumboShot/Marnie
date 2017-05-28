using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marnie.Model;
using Marnie.MultilingualResources;
using Newtonsoft.Json;
using RestSharp;

namespace Marnie.Services
{
    public class PersonService
    {
        public bool UpdatePerson(Person newPerson)
        {
            var marnieClient = new RestClient(AppResources.OwnApiEndpoint);
            var request = new RestRequest("Person", Method.PUT);
            var json = request.JsonSerializer.Serialize(newPerson);

            request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);


            IRestResponse response = marnieClient.Execute(request);
            //get responce status code, then return true if succsessfull (between 200 and 299) else return false
            var num = (int)response.StatusCode;
            if (num >= 200 && num <= 299)
            {
                return true;
            }
            return false;
        }

        public Person GetPersonById(int id)
        {
            var marnieClient = new RestClient(AppResources.OwnApiEndpoint);
            var request = new RestRequest("Person", Method.GET);
            request.AddParameter("Id", id);

            Person person = null;
            IRestResponse response = marnieClient.Execute(request);
            var num = (int)response.StatusCode;
            if (num >= 200 && num <= 299)
            {
                Debug.WriteLine(response.StatusCode);
                person = JsonConvert.DeserializeObject<Person>(response.Content);
            }
            return person;
        }
    }
}
