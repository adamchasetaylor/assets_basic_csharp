using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using RestSharp;
using Twilio;
using Twilio.Rest.Serverless.V1;
using Twilio.Rest.Serverless.V1.Service;
using Twilio.Rest.Serverless.V1.Service.Environment;

namespace upload
{
    class Program
    {
        static void Main(string[] args)
        {
            // Your Account Sid and Auth Token from twilio.com/console

            string accountSid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID");
            string authToken = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN");

            // Setup Default Twilio Client
            TwilioClient.Init(accountSid, authToken);

            // Create string for Basic Auth using RestSharp
            string upload_auth = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(accountSid + ":" + authToken));


            // Create Service
            // https://www.twilio.com/docs/runtime/functions-assets-api/api/service#create-a-service-resource

            var service = ServiceResource.Create(
                includeCredentials: true,
                uniqueName: "my-new-app",
                friendlyName: "My New App"
            );

            // Create Environment
            // https://www.twilio.com/docs/runtime/functions-assets-api/api/environment#create-an-environment-resource

            var environment = EnvironmentResource.Create(
                domainSuffix: "stage",
                uniqueName: "staging",
                pathServiceSid: service.Sid
            );

            // Create Asset
            // https://www.twilio.com/docs/runtime/functions-assets-api/api/asset#create-an-asset-resource

            var asset = AssetResource.Create(
                friendlyName: "friendly_name",
                pathServiceSid: service.Sid
            );

            // Create Asset Verion

            const string remote_path = "/mypath";
            const string remote_visibility = "Public";
            const string asset_file = "hello.txt";
            const string asset_type = "text/plain";
            string asset_path = Path.Combine("assets",asset_file);

            string url = $"https://serverless-upload.twilio.com/v1/Services/{service.Sid}/Assets/{asset.Sid}/Versions";
            
            var client = new RestClient(url);
            client.Timeout = -1;

            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", upload_auth);
            request.AddFile("Content", asset_path, asset_type);
            request.AddParameter("Path", remote_path);
            request.AddParameter("Visibility", remote_visibility);

            IRestResponse response = client.Execute(request);

            var obj = JsonConvert.DeserializeObject<dynamic>(response.Content);          
            List<string> asset_versions = new List<string>() { obj.sid.ToString() };

            // Create a Build
            // https://www.twilio.com/docs/runtime/functions-assets-api/api/build#create-a-build-resource

            var build = BuildResource.Create(
                assetVersions: asset_versions,
                pathServiceSid: service.Sid
            );

            Console.WriteLine("Sleep for 10 seconds.");
            Thread.Sleep(10000);

            // Create a Deployment
            // https://www.twilio.com/docs/runtime/functions-assets-api/api/deployment

            var deployment = DeploymentResource.Create(
                buildSid: build.Sid,
                pathServiceSid: service.Sid,
                pathEnvironmentSid: environment.Sid
            );

            // Show URL

            Console.Write($"http://{environment.DomainName}{remote_path}");
            
        }
    }
}