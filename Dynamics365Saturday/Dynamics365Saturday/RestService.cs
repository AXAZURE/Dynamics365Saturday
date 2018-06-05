using AuthenticationUtility;
using System;
using System.Threading.Tasks;
using System.Text;
using System.Net.Http;

namespace AXZDynamicsSaturday
{
    public class RestService
    {
        public static string appointmentIdUrl = "api/services/AXZTMSAppointmentServiceGroup/AXZTMSAppointmentService/getAppointmentId";
    
        public async Task<string> getAppointmentId(string matricula)
        {
            // Prepare HTTP client, including authentication
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ClientConfiguration.Default.UriString);
            client.DefaultRequestHeaders.Add(OAuthHelper.OAuthHeader, OAuthHelper.GetAuthenticationHeader());

            // Define parameters
            string json = "{'_contract':{'Matricula':'" + matricula + "'}}";

            // Create a request
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, appointmentIdUrl);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            // Run the service
            var result = client.SendAsync(request).Result;

            // Display result to console
            if (result.IsSuccessStatusCode)
            {
                string ret = result.Content.ReadAsStringAsync().Result;
                if (ret == "\"\"")
                    return string.Empty;

                return result.Content.ReadAsStringAsync().Result;
            }
            else
            {
                return result.StatusCode.ToString();
            }
        }
    }
}