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
using Android.OS;
using Org.Apache.Http.Client.Params;
using Debug = System.Diagnostics.Debug;

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
                IdToken idToken = new IdToken();
                idToken.id_token = Application.Current.Properties["id_token"] as string;
                var authId = GetUserIdFromAuth(idToken);
                
                var person = GetPersonByAuthId(authId);
                if (person != null)
                {
                    if (Application.Current.Properties.ContainsKey("UserName"))
                    {
                        Application.Current.Properties["UserName"] = person.Name;
                    }
                    else
                    {
                        Application.Current.Properties.Add("UserName", person.Name);
                    }
                    if (Application.Current.Properties.ContainsKey("PersonId"))
                    {
                        Application.Current.Properties["PersonId"] = person.Id;
                    }
                    else
                    {
                        Application.Current.Properties.Add("PersonId", person.Id);
                    }
                    SaveChanges();
                }
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

        private Person GetPersonByAuthId(string authId)
        {
            var marnieClient = new RestClient("http://marnie-001-site1.atempurl.com/api");
            var request = new RestRequest("Person", Method.GET);
            request.AddParameter("authId", authId);

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

                newPerson.Name = name;
                newPerson.Birthday = birthdate;
                newPerson.ProfilePicture = picturePath;
                newPerson.Gender = gender;
                //todo Add check if Person is saved to db successfully
                var saveToDb = SavePersonToDb(newPerson);

                status = true;//account created
                //Perform login
                if (!Login(username, password))
                {
                    status = false;//login failed
                }
            }
            return status;// account created and login successful
        }

        private bool SavePersonToDb(Person newPerson)
        {
            var marnieClient = new RestClient("http://marnie-001-site1.atempurl.com/api");
            var request = new RestRequest("Person", Method.POST);
            var json = request.JsonSerializer.Serialize(newPerson);

            request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);


            IRestResponse response = marnieClient.Execute(request);
            //get responce status code, then return true if succsessfull (between 200 and 299) else return false
            var num = (int)response.StatusCode;
            if (num >= 200 && num <= 299)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public String GetUserIdFromAuth(IdToken idToken)
        {
            var client = new RestClient("https://olek.eu.auth0.com");
            var request = new RestRequest("tokeninfo", Method.POST);


            var json = request.JsonSerializer.Serialize(idToken);
            request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            UserId userId = JsonConvert.DeserializeObject<UserId>(response.Content);

            Debug.WriteLine("user_id = " + userId.user_id);

            return userId.user_id.Replace("auth0|", "");

        }



        public async void SaveChanges()
        {
            await Application.Current.SavePropertiesAsync();
        }

        private class PersonJ
        {
            [JsonProperty("_id")]
            public string AuthId { get; set; }

        }

        private class UserId
        {
            public string user_id { get; set; }

        }
        public class IdToken
        {
            public string id_token { get; set; }

        }

    }
}
