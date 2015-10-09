using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace PBI_API_Sandbox
{
    public partial class Default : System.Web.UI.Page
    {
        public AuthenticationResult authResult { get; set; }
        private String responseContent { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {
            //Test for AuthenticationResult
            if (Session["authResult"] != null)
            {
                //Get the authentication result from the session
                authResult = (AuthenticationResult)Session["authResult"];

                //Show Power BI Panel
                PowerBIPanel.Visible = true;
                SignInPanel.Visible = false;

                //Set user and token from authentication result
                UserLabel.Text = authResult.UserInfo.DisplayableId;
                AccessTokenTextbox.Text = authResult.AccessToken;
            }
            else
            {
                PowerBIPanel.Visible = false;
            }
        }

        private void GetDashboardTiles(string dashboardID)
        {
            responseContent = string.Empty;
            ResultsTextBox.Text = string.Empty;
            TilesDropdown.Items.Clear();
            TilesDropdown.Items.Add(new ListItem("Please Select...", ""));

            //The resource Uri to the Power BI REST API resource
            string tilesUri = "https://api.powerbi.com/beta/myorg/dashboards/" + dashboardID + "/tiles";

            //Configure datasets request
            System.Net.WebRequest request = System.Net.WebRequest.Create(tilesUri) as System.Net.HttpWebRequest;
            request.Method = "GET";
            request.ContentLength = 0;
            request.Headers.Add("Authorization", String.Format("Bearer {0}", authResult.AccessToken));

            using (var response = request.GetResponse() as System.Net.HttpWebResponse)
            {
                //Get reader from response stream
                using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    responseContent = reader.ReadToEnd();
                    ResultsTextBox.Text = responseContent;

                    //Deserialize JSON string
                    //JavaScriptSerializer class is in System.Web.Script.Serialization
                    JavaScriptSerializer json = new JavaScriptSerializer();
                    Tiles tiles = (Tiles)json.Deserialize(responseContent, typeof(Tiles));

                    foreach (Tile t in tiles.value)
                    {
                        TilesDropdown.Items.Add(new ListItem(t.Title, t.EmbedUrl));
                        ResultsTextBox.Text += String.Format("{0}\t{1}\n", t.Id, t.Title);
                    }
                }
            }
        }

        // Sign In Button
        protected void SignInButton_Click(object sender, EventArgs e)
        {
            //Create a query string
            //Create a sign-in NameValueCollection for query string
            var @params = new NameValueCollection
            {
                //Azure AD will return an authorization code. 
                //See the Redirect class to see how "code" is used to AcquireTokenByAuthorizationCode
                {"response_type", "code"},

                //Client ID is used by the application to identify themselves to the users that they are requesting permissions from. 
                //You get the client id when you register your Azure app.
                {"client_id", Properties.Settings.Default.ClientID},

                // The resource Uri to the Power BI resource to be authorized. You must use this exact Uri.
                {"resource", "https://analysis.windows.net/powerbi/api"},

                // After user authenticates, Azure AD will redirect back to the web app
                // A redirect uri gives AAD more details about the specific application that it will authenticate.
                {"redirect_uri", Properties.Settings.Default.RedirectUri}
            };

            // Create sign-in query string
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString.Add(@params);

            // Redirect to the authority
            // Authority Uri is an Azure resource that takes a client id to get an Access token
            // this access token is then used again with this same Web App in order to make Power BI API Calls
            string authorityUri = "https://login.windows.net/common/oauth2/authorize/";
            Response.Redirect(String.Format("{0}?{1}", authorityUri, queryString));
        }

        // Get Datasets Button
        protected void GetDataSetsButton_Click(object sender, EventArgs e)
        {
            responseContent = string.Empty;
            ResultsTextBox.Text = string.Empty;

            //The resource Uri to the Power BI REST API resource
            string datasetsUri = "https://api.powerbi.com/v1.0/myorg/datasets";

            //Configure datasets request
            System.Net.WebRequest request = System.Net.WebRequest.Create(datasetsUri) as System.Net.HttpWebRequest;
            request.Method = "GET";
            request.ContentLength = 0;
            request.Headers.Add("Authorization", String.Format("Bearer {0}", authResult.AccessToken));

            //Get datasets response from request.GetResponse()
            using (var response = request.GetResponse() as System.Net.HttpWebResponse)
            {
                //Get reader from response stream
                using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    responseContent = reader.ReadToEnd();

                    //Deserialize JSON string
                    //JavaScriptSerializer class is in System.Web.Script.Serialization
                    JavaScriptSerializer json = new JavaScriptSerializer();
                    Datasets datasets = (Datasets)json.Deserialize(responseContent, typeof(Datasets));

                    //Get each Dataset from 
                    foreach (Dataset ds in datasets.value)
                    {
                        ResultsTextBox.Text += String.Format("{0}\t{1}\n", ds.Id, ds.Name);
                    }
                }
            }
        }

        // Get Dashboards Button
        protected void GetDashboardsButton_Click(object sender, EventArgs e)
        {
            responseContent = string.Empty;
            ResultsTextBox.Text = string.Empty;
            DashboardsDropdown.Items.Clear();
            //DashboardsDropdown.Items.Add(new ListItem("Please Select...", ""));

            //The resource Uri to the Power BI REST API resource
            string dashboardsUri = "https://api.powerbi.com/beta/myorg/dashboards";

            //Configure datasets request
            System.Net.WebRequest request = System.Net.WebRequest.Create(dashboardsUri) as System.Net.HttpWebRequest;
            request.Method = "GET";
            request.ContentLength = 0;
            request.Headers.Add("Authorization", String.Format("Bearer {0}", authResult.AccessToken));

            //Get datasets response from request.GetResponse()
            using (var response = request.GetResponse() as System.Net.HttpWebResponse)
            {
                //Get reader from response stream
                using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    responseContent = reader.ReadToEnd();

                    //Deserialize JSON string
                    //JavaScriptSerializer class is in System.Web.Script.Serialization
                    JavaScriptSerializer json = new JavaScriptSerializer();
                    Dashboards dashboards = (Dashboards)json.Deserialize(responseContent, typeof(Dashboards));

                    foreach (Dashboard d in dashboards.value)
                    {
                        DashboardsDropdown.Items.Add(new ListItem(d.DisplayName, d.Id));
                        ResultsTextBox.Text += String.Format("{0}\t{1}\n", d.Id, d.DisplayName);
                    }

                }
            }

            GetDashboardTiles(DashboardsDropdown.Items[0].Value);
        }

        // Dashboards Dropdown
        protected void DashboardsDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetDashboardTiles(DashboardsDropdown.SelectedValue);
        }

        protected void DashboardsDropdown_DataBound(object sender, EventArgs e)
        {
            GetDashboardTiles(DashboardsDropdown.Items[0].Value);
        }
    }

    // Datasets
    public class Datasets
    {
        public Dataset[] value { get; set; }
    }
    public class Dataset
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    // Dashboards
    public class Dashboards
    {
        public Dashboard[] value { get; set; }
    }
    public class Dashboard
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
    }

    // Tiles
    public class Tiles
    {
        public Tile[] value { get; set; }
    }
    public class Tile
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string EmbedUrl { get; set; }
    }
}