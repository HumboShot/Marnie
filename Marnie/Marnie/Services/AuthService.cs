using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marnie.Layout;
using Marnie.Model;
using Newtonsoft.Json;
using RestSharp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net;
using Org.Apache.Http.Client.Params;

namespace Marnie
{
    public class AuthService
    {

        public bool Login(string username, string password)
        {
            var client = new RestClient("https://olek.eu.auth0.com");
            var request = new RestRequest("oauth/ro", Method.POST);

            request.AddParameter("client_id", "WU0DxEKuQXDpgSJ8lGmLNr1Nux2ejl1P");
            request.AddParameter("username", username);
            request.AddParameter("password", password);
            request.AddParameter("connection", "Username-Password-Authentication");
            request.AddParameter("grant_type", "password");
            request.AddParameter("scope", "openid");

            IRestResponse response = client.Execute(request);

            // Using the Newtonsoft.Json library we deserialaize the string into an object,
            // we have created a LoginToken class that will capture the keys we need
            LoginToken token = JsonConvert.DeserializeObject<LoginToken>(response.Content);

            if (token.id_token != null)
            {
                if (Application.Current.Properties.ContainsKey("id_token"))
                {
                    Application.Current.Properties["id_token"] = token.id_token;
                }
                else
                {
                    Application.Current.Properties.Add("id_token", token.id_token);
                }//end if

                if (Application.Current.Properties.ContainsKey("access_token"))
                {
                    Application.Current.Properties["access_token"] = token.access_token;
                }
                else
                {
                    Application.Current.Properties.Add("access_token", token.access_token);
                }//end if

                if (Application.Current.Properties.ContainsKey("isLoggetIn"))
                {
                    Application.Current.Properties["isLoggetIn"] = true;
                }
                else
                {
                    Application.Current.Properties.Add("isLoggetIn", true);
                }//end if

                SaveChanges(); //save properties

                return true;
            }
            else
            {
                if (Application.Current.Properties.ContainsKey("isLoggetIn"))
                {
                    Application.Current.Properties["isLoggetIn"] = false;
                }
                else
                {
                    Application.Current.Properties.Add("isLoggetIn", false);
                }
                SaveChanges();
                return false;
            }
        }

        //Save aplication properties persistent

        public bool Signup(string name, DateTime birthdate, string picturePath, string gender, string username, string password)
        {
            var status = false;
            var client = new RestClient("https://olek.eu.auth0.com");
            var request = new RestRequest("dbconnections/signup", Method.POST);

            request.AddParameter("client_id", "WU0DxEKuQXDpgSJ8lGmLNr1Nux2ejl1P");
            request.AddParameter("email", username);
            request.AddParameter("password", password);
            request.AddParameter("connection", "Username-Password-Authentication");

            IRestResponse response = client.Execute(request);
            // Once the request is executed we capture the response.
            // If we get a `user_id`, we know that the account has been created
            // and display an appropriate message. If we do not get a `user_id`
            // we know something went wrong, so we ask the user if they already have
            // an account and if not to try again.
            PersonJ p = JsonConvert.DeserializeObject<PersonJ>(response.Content);

            if (p.AuthId != null)//account created
            {
                //Create new Person with UserId
                var newPerson = new Person(p.AuthId);
                // newPerson.AuthId = p.AuthId;
                newPerson.Name = name;
                newPerson.Birthday = birthdate;
                newPerson.ProfilPicture = picturePath;
                newPerson.Gender = gender;

                //JsonConvert.SerializeObject(newPerson);

                SavePersonToDb(newPerson);


                //todo Create new user in our database with provided user_id
                //redirect to  a new screen with the rest of the user info and save button and 
                //update the user insted of display alert '

                status = true;//account created
                //Perform login
                if (!Login(username, password))
                {
                    status = false;//login failed
                }
            }
            return status;// account created and login successful
        }

        private void SavePersonToDb(Person newPerson)
        {
            var marnieClient = new RestClient("http://marnie-001-site1.atempurl.com/api");
            var request = new RestRequest("Person", Method.POST);
            var json = request.JsonSerializer.Serialize(newPerson);
         
            request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);
            //request.AddHeader("Content-type", "application/json");
            //request.AddParameter("Person", newPerson);
            
            IRestResponse response = marnieClient.Execute(request);
        }

       

        public async void SaveChanges()
        {
            await Application.Current.SavePropertiesAsync();
        }

        private class PersonJ
        {
            [JsonProperty("_id")]
            public string AuthId { get; set; }

            public string Name { get; set; }

            public DateTime Birthday { get; set; }

            public string Gender { get; set; }

            public string ProfilPicture { get; set; }
        }

        private class RequestObject
        {
            public Person person { get; set; }
        }

    }
}
