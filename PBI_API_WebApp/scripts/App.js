/// <reference name="MicrosoftAjax.js"/>

"use strict";

Type.registerNamespace("PBI");
PBI = PBI || {};

PBI.EmbedTile = {
    
    frameHeight: 400,
    frameWidth: 400,
    TileUrl: "",

    registerTileChangeEvent: function() {

        // use jQuery to register an event on the dropdown list
        $("#TilesDropdown").on("change", PBI.EmbedTile.updateEmbedTile)

        //How to navigate from a Power BI Tile to the dashboard
        // listen for message to receive the tile click messages.
        if (window.addEventListener) {
            window.addEventListener("message", PBI.EmbedTile.receiveMessage, false);
        } else {
            window.attachEvent("onmessage", PBI.EmbedTile.receiveMessage);
        }

        //How to handle server side post backs
        // handle server side post backs, optimize for reload scenarios
        // show embedded tile if all fields were filled in.
        var accessTokenElement = $('#AccessTokenTextbox');
        if (null !== accessTokenElement) {
            var accessToken = accessTokenElement.text();
            if ("" !== accessToken){
                PBI.EmbedTile.updateEmbedTile();
            }
        }
    },

    // Event Handler
    //How to navigate from a Power BI Tile to the dashboard
    // The embedded tile posts message to the parent window on click.
    // Listen and handle as appropriate
    // The sample shows how to open the tile source.
    receiveMessage: function(event){
        if (event.data) {
            try {
                var messageData = JSON.parse(event.data);
                if (messageData.event === "tileClicked") {
                    //Get IFrame source and construct dashboard url
                    var iFrameSrc = $("#PowerBIFrame").attr("src");

                    //Split IFrame source to get dashboard id
                    var dashboardId = iFrameSrc.split("dashboardId=")[1].split("&")[0];

                    //Get PowerBI service url
                    var dashboardUrl = iFrameSrc.split("/embed")[0] + "/dashboards/{0}";
                    dashboardUrl = dashboardUrl.replace("{0}", dashboardId);

                    window.open(dashboardUrl);
                }
            }
            catch (e) {
                // In a production app, handle exception
            }
        }
    },

    // Event Handler
    // Update the embedded tile
    updateEmbedTile: function(){
        // check if the embed url was selected
        PBI.EmbedTile.TileUrl = $("#TilesDropdown option:selected").attr("value");

        // if no option has been selected yet, don't proceed to update the iframe
        if (undefined === PBI.EmbedTile.TileUrl || "" === PBI.EmbedTile.TileUrl) {
            return;
        }

        // to load a tile do the following:
        // 1: set the url, include size.
        // 2: add a onload handler to submit the auth token
        var iframe = $('#PowerBIFrame');
        iframe.attr("src", PBI.EmbedTile.TileUrl + "&width=" + PBI.EmbedTile.frameWidth + "&height=" + PBI.EmbedTile.frameHeight);
        iframe.on("load", PBI.EmbedTile.postActionLoadTile);
    },

    // iframe onload Event Handler
    // Post the auth token to the iframe
    postActionLoadTile: function () {
        // get the access token.
        var accessToken = $('#AccessTokenTextbox').text();

        // return if no a
        if ("" === accessToken) {
            return;
        }

        // construct the push message structure
        var m = { action: "loadTile", accessToken: accessToken, height: PBI.EmbedTile.frameHeight, width: PBI.EmbedTile.frameWidth };
        var message = JSON.stringify(m);

        // push the message.
        var iframe = document.getElementById('PowerBIFrame');
        iframe.contentWindow.postMessage(message, "*");
    }
}