using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using AXZDynamicsSaturday.TMSAppointmentServiceReference;
using AuthenticationUtility;

namespace SoapUtility
{
    public class SoapHelper
    {
        public static string GetSoapServiceUriString(string serviceName, string aosUriString)
        {
            var soapServiceUriStringTemplate = "{0}/soap/services/{1}";
            var soapServiceUriString = string.Format(soapServiceUriStringTemplate, aosUriString.TrimEnd('/'), serviceName);
            return soapServiceUriString;
        }

        public static Binding GetBinding()
        {
            var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);

            // Set binding timeout and other configuration settings
            binding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
            binding.ReaderQuotas.MaxArrayLength = int.MaxValue;
            binding.ReaderQuotas.MaxNameTableCharCount = int.MaxValue;

            binding.ReceiveTimeout = TimeSpan.MaxValue;
            binding.SendTimeout = TimeSpan.MaxValue;
            binding.MaxReceivedMessageSize = int.MaxValue;

            var httpsTransportBindingElement = binding.CreateBindingElements().OfType<HttpsTransportBindingElement>().FirstOrDefault();
            
            return binding;
        }

        public static string GetAppointment(string matricula)
        {
            var aosUriString = ClientConfiguration.Default.UriString;

            var oauthHeader = OAuthHelper.GetAuthenticationHeader();
            var serviceUriString = SoapUtility.SoapHelper.GetSoapServiceUriString("AXZTMSAppointmentServiceGroup", aosUriString);

            var endpointAddress = new System.ServiceModel.EndpointAddress(serviceUriString);
            var binding = SoapUtility.SoapHelper.GetBinding();

            var client = new AXZTMSAppointmentServiceClient(binding, endpointAddress);
            var channel = client.InnerChannel;

            CallContext callContext = new CallContext();
            callContext.Company = "USMF";
            callContext.Language = "es";

            MatriculaDC matriculaDC = new MatriculaDC();
            matriculaDC.Matricula = matricula;

            string appointmentId = "";

            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;
                appointmentId = ((AXZTMSAppointmentService)channel).getAppointmentIdAsync(matriculaDC).Result;
            }

            return appointmentId;
        }
    }
}
