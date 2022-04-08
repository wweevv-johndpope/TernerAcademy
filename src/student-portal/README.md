# Web Frontend App Guide (Student Portal)

## Prerequisites
- C#/.NET experience at a beginner level
- Knowledge on Blazor WASM
- Knowledge on Provisioning Azure Resources 
- Local installations of the .NET SDK (6.0) and Visual Studio 2022
- Read first the infrastructure setup posted on the main [README.md](../../README.md)

## Azure Cloud Prerequisites
- Create/Provision Azure Static App / Azure App Service

## Instruction
- After completing the Azure Cloud Prerequisites
- Open Terner.Student.Portal.sln on Visual Studio
- Look for a file named ***Server.cs***
- Update the necessary values and point it to your API, please see [API Setup Guide](../backend/README.Backend.md)
- Set **Client.App** Project as Startup Project
- Click Run or Press F5.
- If you wish to publish the app to Azure App Service, please see [Guide](https://docs.microsoft.com/en-us/visualstudio/deployment/quickstart-deploy-aspnet-web-app?view=vs-2022&tabs=azure)
