using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Threading.Tasks;

namespace PBI_API_Sandbox
{
    public partial class Redirect : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Redirect uri must match the redirect_uri used when requesting Authorization code. 
            string redirectUri = Properties.Settings.Default.RedirectUri;
            string authorityUri = "https://login.windows.net/common/oauth2/authorize/";

            // Get the auth code 
            string code = Request.Params.GetValues(0)[0];

            // Get auth token from auth code        
            TokenCache TC = new TokenCache();

            AuthenticationContext AC = new AuthenticationContext(authorityUri, TC);


            ClientCredential cc = new ClientCredential
                (Properties.Settings.Default.ClientID,
                Properties.Settings.Default.ClientSecretKey);

            AuthenticationResult AR = AC.AcquireTokenByAuthorizationCode(code, new Uri(redirectUri), cc);


            //Set Session "authResult" index string to the AuthenticationResult 
            Session["authResult"] = AR;


            //Redirect back to Default.aspx 
            Response.Redirect("/Default.aspx");
        }
    }
}