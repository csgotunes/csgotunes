# Overview

CS:GO Tunes allows users to listen to music when playing with automatic pause/play when you spawn or die. 

## How do I use it?

To use CS:GO Tunes, simply navigate to the website [https://cstunes.com](https://cstunes.com) and login with your Spotify account. You will then get instructions on how to hook it up to your CS:GO client.

## How does it work?

Using gamestate integration, CS:GO Tunes is able to get events transmited to it when you spawn or die. Using this information, calls are made to the Spotify player to control playback accordingly. For more information about gamestate integration, see: https://developer.valvesoftware.com/wiki/Counter-Strike:_Global_Offensive_Game_State_Integration

# Development

## Getting Started

### Dependencies

First, make sure you have the latest version of Docker on your machine (you will also need compose). You can find instructions for your operating system here: https://docs.docker.com/get-docker/

Next, install the latest version of Node.js using the instructions here: https://nodejs.org/en/download/

Lastly, you will need to install the latest .NET SDK (minimum version 6): https://dotnet.microsoft.com/en-us/download

### Spotify Developer Setup

If you want to be able to fully test the application locally, you will need to create an application and client ID/client secret in the developer portal.

> To use the Web API, start by creating a Spotify user account (Premium or Free). To do that, simply sign up at www.spotify.com.
> When you have a user account, go to the Dashboard page at the Spotify Developer website and, if necessary, log in. Accept the latest Developer Terms of Service to complete your account set up.
> You need to create and register a new application to generate valid credentials. You’ll need these credentials later to perform API calls.

Once you have a client ID and client secret for your application, open the `API/local.settings.json` file and replace `CSGOTunes__SpotifyClientID` and `CSGOTunes__SpotifyClientSecret` with the newly generated credentials.

### Starting Azure Storage Emulator

In your terminal or command prompt, run the following command to start the Azure Storage emulator:

```bash
cd Meta
docker compose up -d
```

If you would like to completely delete your local database and re-create it, you may use this command to destroy the emulated database (then simply run the above command to re-create it):

```bash
cd Meta
docker compose down
```

### Running API

Using your IDE of choice (ie: JetBrains Rider or Visual Studio), you should be able to launch the API simply using the built-in controls. The solution file is located in the API directory.

If you would prefer to launch from the command line/terminal, simply run the following command:
```bash
cd API
dotnet install
dotnet build
dotnet run
```

### Running Website

You should be able to use an IDE's built-in controls (such as JetBrains WebStorm or Visual Studio Code) to run the website, which is located in the Website directory.

If you would prefer to launch from the command line/terminal, simply run the following command:
```bash
cd Website
npm install
npm run build
npm run start
```

# Contributing

To contribute to CS:GO Tunes, you may report issues or make pull requests to improve the application. You may also donate, but currently the costs are very minimal so it is completely unnecessary (though appreciated!)

Please remember that by making pull requests you are agreeing to the terms of the [MIT License](https://github.com/csgotunes/csgotunes/blob/main/LICENSE)