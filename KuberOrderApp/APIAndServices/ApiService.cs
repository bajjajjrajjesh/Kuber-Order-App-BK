using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using KuberOrderApp.Pages.Authentication;
using KuberOrderApp.Utilities;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace KuberOrderApp.APIAndServices
{
    public class ApiService
    {
        public static string resp;
        async public static Task<T> PostRequest<T>(string MethodName, object requestBody = null, IDictionary<string, string> parameters = null)
        {
            try
            {
                 var client = new HttpClient();

                client.BaseAddress = new Uri(ApiPathString.BaseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", (string)Helper.GetPreference("Token"));
                var token = (string)Helper.GetPreference("Token");
                if(token != null)
                    client.DefaultRequestHeaders.Add("Authorization", token);



                StringContent data = null;
                FormUrlEncodedContent encodedContent = null;
                HttpResponseMessage response = new HttpResponseMessage();
                if (requestBody != null)
                {
                    var json = JsonConvert.SerializeObject(requestBody);
                    data = new StringContent(json, Encoding.UTF8, "application/json");
                    response = await client.PostAsync($"{ApiPathString.BaseUrl}{MethodName}", data);
                }
                else
                {
                    encodedContent = new FormUrlEncodedContent(parameters);
                    response = await client.PostAsync($"{ApiPathString.BaseUrl}{MethodName}", encodedContent).ConfigureAwait(false);
                }

                response.EnsureSuccessStatusCode();
                var resp = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Helper.DisplayAlert("User is Unauthorized. Please Login again.");
                    return (T)await Task.FromResult<object>(null);
                }
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    return (T)await Task.FromResult<object>(null);

                return JsonConvert.DeserializeObject<T>(resp);

            }
            catch (Newtonsoft.Json.JsonSerializationException jse)
            {
                return (T)await Task.FromResult<object>(null);
            }
            catch (TaskCanceledException ex)
            {
                string strResponse = JsonConvert.DeserializeObject<string>(resp);
                if (strResponse == "unauthorised")
                    Helper.DisplayAlert("User is Unauthorized. Please Login again.");

                return (T)await Task.FromResult<object>(null);
            }
            catch (Exception e)
            {

                return (T)await Task.FromResult<object>(null);
            }

            //return (response.StatusCode.ToString(), response.Content);
        }

        async public static Task<T> GetRequest<T>(string MethodName, object requestBody = null, IDictionary<string, string> parameters = null)
        {
            try
            {
                var client = new HttpClient();

                client.BaseAddress = new Uri(ApiPathString.BaseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                StringContent data = null;
                HttpResponseMessage response = new HttpResponseMessage();

                var token = (string)Helper.GetPreference("Token");
                client.DefaultRequestHeaders.Add("Authorization", token);


                if (requestBody != null)
                {
                    var json = JsonConvert.SerializeObject(requestBody);
                    data = new StringContent(json, Encoding.UTF8, "application/json");
                   
                    response = await client.GetAsync($"{ApiPathString.BaseUrl}{MethodName}?{data}");
                }
                else
                {
                    response = await client.GetAsync($"{ApiPathString.BaseUrl}{MethodName}").ConfigureAwait(false);
                }

                response.EnsureSuccessStatusCode();
                resp = await response.Content.ReadAsStringAsync();

                if (resp == "401")
                {
                 
                    var check = await UserDialogs.Instance.ConfirmAsync("User is Unauthorized. Please Login again.", "Unauthorized", "ok","close");
                    if (check)
                    {
                        await App.Database.DropTableAndCreateNew();
                        Helper.SavePreference("LoginResponse", "");
                        Helper.SavePreference("IsLoginComplete", false);
                        App.Current.MainPage = new NavigationPage(new LoginPage());
                    }
                    else
                    {
                        await App.Database.DropTableAndCreateNew();
                        Helper.SavePreference("LoginResponse", "");
                        Helper.SavePreference("IsLoginComplete", false);
                        App.Current.MainPage = new NavigationPage(new LoginPage());
                    }
             
                   
                    //return ;
                }

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized )
                {
                    Helper.DisplayAlert("User is Unauthorized. Please Login again.");
                    
                    return (T)await Task.FromResult<object>(null);
                }
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    return (T)await Task.FromResult<object>(null);

                return JsonConvert.DeserializeObject<T>(resp);

            }
         /*   catch (Newtonsoft.Json.JsonSerializationException jse)
            {
                string strResponse = JsonConvert.DeserializeObject<string>(resp);
                if (strResponse == "unauthorised")
                    Helper.DisplayAlert("User is Unauthorized. Please Login again.");

                throw jse;
            }*/
            catch (TaskCanceledException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw e;
            }

            //return (response.StatusCode.ToString(), response.Content);
        }


        
    }
}
