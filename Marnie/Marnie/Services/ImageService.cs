using System;
using System.Diagnostics;
using Marnie.Model;
using Marnie.MultilingualResources;
using Newtonsoft.Json;
using Plugin.Media.Abstractions;
using RestSharp;
using RestSharp.Extensions;
using Xamarin.Forms;

namespace Marnie.Services
{
    public class ImageService
    {
        private MediaFile _mediaFile;
        public bool SavePicture(MediaFile photo)
        {
            _mediaFile = photo;
            byte[] bytes;
            try
            {
                var stream = _mediaFile.GetStream();
                bytes = new byte[stream.Length];
                bytes = stream.ReadAsBytes();
            }
            catch (Exception e)
            {
                throw e;
            }

            string base64ImageString = Convert.ToBase64String(bytes);

            var imgurClient = new RestClient(AppResources.ImgurEndpoint);
            var request = new RestRequest("image", Method.POST);
            request.AddHeader("Authorization", "Client-ID " + AppResources.ImgurClientId);
            var json = request.JsonSerializer.Serialize(new MyImage() {image = base64ImageString});

            request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);
            request.Timeout = 120000;

            IRestResponse response = imgurClient.Execute(request);
            var num = (int)response.StatusCode;
            if (num >= 200 && num <= 299)
            {
                Debug.WriteLine(response.StatusCode);
                dynamic data = JsonConvert.DeserializeObject(response.Content);
                string link = data.data.link;
                PersonService ps = new PersonService();
                Person updatedPerson = ps.GetPersonById((int)Application.Current.Properties["PersonId"]);
                updatedPerson.ProfilePicture = link;
                if(new PersonService().UpdatePerson(updatedPerson))
                return true;
            }
                return false;
        }
        private class MyImage
        {
            public string image { get; set; }
        }
    }
}
