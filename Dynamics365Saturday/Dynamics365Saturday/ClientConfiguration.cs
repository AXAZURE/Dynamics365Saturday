using System;
using AXZDynamicsSaturday;

namespace AuthenticationUtility
{
    public partial class ClientConfiguration
    {
        public static ClientConfiguration Default { get { return ClientConfiguration.OneBox; } }

        public static ClientConfiguration OneBox = new ClientConfiguration()
        {
            UriString = AXZSecrets.uriString,
            UserName = "",            
            // Insert the correct password here for the actual test.
            Password = "",

            ActiveDirectoryResource = AXZSecrets.uriString,
            ActiveDirectoryTenant = AXZSecrets.ADTenant,
            ActiveDirectoryClientAppId = AXZSecrets.ADClientAppId,
            // Insert here the application secret when authenticate with AAD by the application
            ActiveDirectoryClientAppSecret = AXZSecrets.ADClientAppSecret,

            // Change TLS version of HTTP request from the client here
            // Ex: TLSVersion = "1.2"
            // Leave it empty if want to use the default version
            TLSVersion = "",
        };

        public string TLSVersion { get; set; }
        public string UriString { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ActiveDirectoryResource { get; set; }
        public String ActiveDirectoryTenant { get; set; }
        public String ActiveDirectoryClientAppId { get; set; }
        public string ActiveDirectoryClientAppSecret { get; set; }
    }
}
