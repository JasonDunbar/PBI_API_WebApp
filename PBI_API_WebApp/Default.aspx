<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PBI_API_Sandbox.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Power BI Web App</title>
    <link type="text/css" rel="stylesheet" href="./css/default.css" />
    <script type="text/javascript" src="//ajax.aspnetcdn.com/ajax/4.0/1/MicrosoftAjax.js"></script>
    <script type="text/javascript" src="http://code.jquery.com/jquery-2.1.4.min.js"></script>
    <script type="text/javascript" src="./scripts/App.js"></script>
    <script type="text/javascript">
        window.onload = function () {
            PBI.EmbedTile.registerTileChangeEvent();
        }
    </script>
</head>
<body>
    <form runat="server">
    <div>
        <!-- Show this once the user is authenticated -->
        <asp:Panel ID="PowerBIPanel" runat="server">
            
            <!-- Intro and Properties -->
            
            <h1>The Power BI API Sandbox</h1>
            <p>If you're seeing this, then you're authenticated against the CTP Power BI Service (linked to the ctp2.onmicrosoft.com tenant)</p>
            <div class="container">
                <div class="leftColumn titleBold">The curent user: </div>
                <div class="rightColumn"><asp:Label ID="UserLabel" runat="server" /></div>
                <div class="leftColumn titleBold">Your Access Token: </div>
                <a>(hover mouse here to show access token)</a>
                <div class="rightColumn hiddenElement"><asp:Label ID="AccessTokenTextbox" runat="server" /></div>
            </div>

            <!-- User guidance -->
            <div style="clear: both;">
                <h2>User Guide</h2>
                <ol>
                    <li>Hitting the 'Get Dashboards' button will fill the Dashboards dropdown with the dashboards that you have access to in Power BI. 
                        It will also refresh the Tiles dropdown to show the Tiles availaible from the selected Dashboard.</li>
                    <li>Selecting a dashboard from the list will refresh the Tils dropdown, loading in the Tiles for the newly selected dashboard.</li>
                    <li>Select a Tile from the dropdown to have that tile displayed in the iframe below.</li>
                    <li>Hovering over the Power BI tile will display the Tool Tips; Clicking will open the dashboard in a new window.</li>
                </ol>
                For each request, the response is put in the text box below.
            </div>

            <!-- Buttons -->
            <!-- <asp:Button ID="GetDataSetsButton" runat="server" Text="Get Me DataSets!" OnClick="GetDataSetsButton_Click" /> -->
            
            
            <br />
            
            <!-- Dashboards Dropdown -->
            <div class="spacing">
                <span class="titleBold">Dashboards: </span>
                <asp:DropDownList ID="DashboardsDropdown" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DashboardsDropdown_SelectedIndexChanged" />
                <asp:Button ID="GetDashboardsButton" runat="server" Text="Get Dashboards" OnClick="GetDashboardsButton_Click" />
            </div>
            
            <!-- Tiles Dropdown -->
            <div class="spacing">
                <span class="titleBold">Tiles: </span>
                <asp:DropDownList ID="TilesDropdown" runat="server" />
            </div>
            
            <!-- json output TextBox -->
            <div class="spacing">
                <asp:TextBox ID="ResultsTextBox" runat="server" Height="200px" Width="800px" TextMode="MultiLine" Wrap="False" />
            </div>
            
            <!-- Power BI Tile iframe -->
            <div class="spacing">
                <h2>Here's the embedded Tile in an iframe</h2>
                <iframe id="PowerBIFrame" seamless="seamless" height="400" width="400"></iframe>
            </div>

        </asp:Panel>

        <!-- Show this when the user is not yet authenticated -->
        <asp:Panel ID="SignInPanel" runat="server" Visible="true">
            <p>
                By clicking the button below, you will use AAD to authenticate against the Power BI Service. 
                <br />this will return an access token for Power BI API calls
            </p>
            <asp:Button ID="SignInButton" runat="server" Text="Login to Power BI" OnClick="SignInButton_Click" />
        </asp:Panel>
    </div>
    </form>
</body>
</html>