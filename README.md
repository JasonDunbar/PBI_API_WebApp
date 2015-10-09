# Power BI Sandbox

Since the launch of some of the new API components (namely Groups, Dashboards, Tiles) I've been tinkering with examples in order to understand the order of events and the flow of information. Ultimately, my goal was to put together a small proof of concept app which could be further utilised in different web platforms when the requirements fit.

There's a great [Example App](https://github.com/PowerBI/Integrate-a-tile-into-an-app) from the Microsoft Power BI team to get you started, but I thought I could improve upon it. 

###Things I did differently:
- Where as the Web App provided by Microsoft relies on the click of a button to show a Tile, i've made it all dynamic with cascading dropdowns and an auto-refreshing iframe, determined by the tile that's selected.
- I've placed a TextBox on the page to show the JSON that is returned from the API calls
- User guidance! Partly the idea of this was to demonstrate the capabilities to lesser technical people in order to better explain concepts and sell benefits.
- I've put the javascript into a namespace and I'm using jQuery to make the javascript a little more readable

### Usage
Feel free to grab it and try it out. Though there's quite some setup before you do that:
- [Register your Web App in Azure AD](https://msdn.microsoft.com/en-us/library/dn985955.aspx) - what this does is two things, provides an authentication mechanism and acts as a security broker for the Power BI service
- [Authenticate Your Web App](https://msdn.microsoft.com/en-US/library/mt143610.aspx)
- Once cloned, you will need to use Nuget to restore the packages/references, otherwise it won't build

For a comprehensive guide on some of the javascript elements that are required to make this work, you might want to check out this [Power BI Developer blog post](https://msdn.microsoft.com/en-US/library/mt450498.aspx)

### Small Disclaimer
I'm not a developer and therefore it's likely that a lot of my code stinks and may not follow what's considered 'best practice' :-)
