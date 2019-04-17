using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

namespace KostenVoranSchlagConsoleParser.Helpers
{
    /// <summary>
    /// Класс для получения сервиса 2016 CRM
    /// </summary>
    class ConnectHelper
    {
        public static OrganizationServiceProxy CrmService 
        {
            get
            {
                var config = GreateConfiguration();
                return new OrganizationServiceProxy(config.OrganizationUri, config.HomeRealmUri,
                                                            config.Credentials, config.DeviceCredentials);
            }
        }

        #region private methods

        private static Configuration GreateConfiguration()
        {
            var connStr = ConfigurationManager.ConnectionStrings["Xrm"].ConnectionString;

            Dictionary<string, string> connStringParts = connStr.Split(';')
                .Select(t => t.Split(new char[] {'='}, 2))
                .ToDictionary(t => t[0].Trim(), t => t[1].Trim(), StringComparer.InvariantCultureIgnoreCase);

            var serverUrl = connStringParts["Server"];
            var login = connStringParts["Username"];
            var pass = connStringParts["Password"];

            var cfg = new Configuration
            {
                DiscoveryUri = GetDiscoveryUri(serverUrl),
                OrganizationUri = new Uri(String.Format("{0}/XRMServices/2011/Organization.svc", serverUrl)),
                HomeRealmUri = null,
                DeviceCredentials = null,
                Credentials = new ClientCredentials()
            };

            cfg.Credentials.UserName.UserName = login;
            cfg.Credentials.UserName.Password = pass;
            
            return cfg;
        }

        private static Uri GetDiscoveryUri(string serverUrl)
        {
            Uri uri = null;
            var server = serverUrl.Substring(serverUrl.IndexOf(@"//") + 2);
            if (server.EndsWith(@"/", StringComparison.InvariantCultureIgnoreCase))
            {
                server = server.Remove(server.Length - 1);
            }
            // One of the Microsoft Dynamics CRM Online data centers.
            if (server.EndsWith(".dynamics.com", StringComparison.InvariantCultureIgnoreCase))
            {
                // Check if the organization is provisioned in Microsoft Office 365.
                if (GetOrgType(server))
                {
                    uri = new Uri(String.Format("https://disco.{0}/XRMServices/2011/Discovery.svc", server));
                }
                else
                {
                    uri = new Uri(String.Format("https://dev.{0}/XRMServices/2011/Discovery.svc", server));

                    // Get or set the device credentials. This is required for Microsoft account authentication. 
                    //config.DeviceCredentials = GetDeviceCredentials();
                }
            }
            // Check if the server uses Secure Socket Layer (https).
            else if (IsSsl(serverUrl))
                uri = new Uri(String.Format("https://{0}/XRMServices/2011/Discovery.svc", server));
            else
                uri = new Uri(String.Format("http://{0}/XRMServices/2011/Discovery.svc", server));

            return uri;
        }

        /// <summary>
        /// Is this organization provisioned in Microsoft Office 365?
        /// </summary>
        /// <param name="server">The server's network name.</param>
        private static Boolean GetOrgType(String server)
        {
            
            Boolean isO365Org = false;
            if (String.IsNullOrWhiteSpace(server))
                return isO365Org;
            if (server.IndexOf('.') == -1)
                return isO365Org;
            
            return true;
            /*Console.Write("Is this organization provisioned in Microsoft Office 365 (y/n) [y]: ");
            String answer = Console.ReadLine();

            if (answer == "y" || answer == "Y" || answer.Equals(String.Empty))
                isO365Org = true;

            return isO365Org;*/
        }

        /// <summary>
        /// Obtains the name and port of the server running the Microsoft Dynamics CRM
        /// Discovery service.
        /// </summary>
        /// <returns>The server's network name and optional TCP/IP port.</returns>
        private static bool IsSsl(string server)
        {
            bool ssl = false;

            if (server.EndsWith(".dynamics.com") || String.IsNullOrWhiteSpace(server))
            {
                ssl = true;
            }
            else
            {
                ssl = server.Length > 0 && server.Substring(0, 5).Equals("https");
            }

            return ssl;
        }

        #endregion

        #region Inner classes
        /// <summary>
        /// Stores Microsoft Dynamics CRM server configuration information.
        /// </summary>
        public class Configuration
        {
            public String ServerAddress;
            public String OrganizationName;
            public Uri DiscoveryUri;
            public Uri OrganizationUri;
            public Uri HomeRealmUri = null;
            public ClientCredentials DeviceCredentials = null;
            public ClientCredentials Credentials = null;
            public AuthenticationProviderType EndpointType;
            public String UserPrincipalName;
            #region internal members of the class
            internal IServiceManagement<IOrganizationService> OrganizationServiceManagement;
            internal SecurityTokenResponse OrganizationTokenResponse;
            internal Int16 AuthFailureCount = 0;
            #endregion

            public override bool Equals(object obj)
            {
                //Check for null and compare run-time types.
                if (obj == null || GetType() != obj.GetType()) return false;

                Configuration c = (Configuration)obj;

                if (!this.ServerAddress.Equals(c.ServerAddress, StringComparison.InvariantCultureIgnoreCase))
                    return false;
                if (!this.OrganizationName.Equals(c.OrganizationName, StringComparison.InvariantCultureIgnoreCase))
                    return false;
                if (this.EndpointType != c.EndpointType)
                    return false;
                if (null != this.Credentials && null != c.Credentials)
                {
                    if (this.EndpointType == AuthenticationProviderType.ActiveDirectory)
                    {

                        if (!this.Credentials.Windows.ClientCredential.Domain.Equals(
                            c.Credentials.Windows.ClientCredential.Domain, StringComparison.InvariantCultureIgnoreCase))
                            return false;
                        if (!this.Credentials.Windows.ClientCredential.UserName.Equals(
                            c.Credentials.Windows.ClientCredential.UserName, StringComparison.InvariantCultureIgnoreCase))
                            return false;

                    }
                    else if (this.EndpointType == AuthenticationProviderType.LiveId)
                    {
                        if (!this.Credentials.UserName.UserName.Equals(c.Credentials.UserName.UserName,
                            StringComparison.InvariantCultureIgnoreCase))
                            return false;
                        if (!this.DeviceCredentials.UserName.UserName.Equals(
                            c.DeviceCredentials.UserName.UserName, StringComparison.InvariantCultureIgnoreCase))
                            return false;
                        if (!this.DeviceCredentials.UserName.Password.Equals(
                            c.DeviceCredentials.UserName.Password, StringComparison.InvariantCultureIgnoreCase))
                            return false;
                    }
                    else
                    {

                        if (!this.Credentials.UserName.UserName.Equals(c.Credentials.UserName.UserName,
                            StringComparison.InvariantCultureIgnoreCase))
                            return false;

                    }
                }
                return true;
            }

            public override int GetHashCode()
            {
                int returnHashCode = this.ServerAddress.GetHashCode()
                    ^ this.OrganizationName.GetHashCode()
                    ^ this.EndpointType.GetHashCode();
                if (null != this.Credentials)
                {
                    if (this.EndpointType == AuthenticationProviderType.ActiveDirectory)
                        returnHashCode = returnHashCode
                            ^ this.Credentials.Windows.ClientCredential.UserName.GetHashCode()
                            ^ this.Credentials.Windows.ClientCredential.Domain.GetHashCode();
                    else if (this.EndpointType == AuthenticationProviderType.LiveId)
                        returnHashCode = returnHashCode
                            ^ this.Credentials.UserName.UserName.GetHashCode()
                            ^ this.DeviceCredentials.UserName.UserName.GetHashCode()
                            ^ this.DeviceCredentials.UserName.Password.GetHashCode();
                    else
                        returnHashCode = returnHashCode
                            ^ this.Credentials.UserName.UserName.GetHashCode();
                }
                return returnHashCode;
            }

        }
        #endregion Inner classes
    }
}
