using System;
using System.IO;
using System.Text;
using RestSharp;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace upload
{
    class Program
    {
        static void Main(string[] args)
        {

        string TWILIO_ACCOUNT_SID = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID");
        string TWILIO_AUTH_TOKEN = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN");

        const string serviceSid = "";
        const string assetSid = "";

        const string remote_path = "/mypath";
        const string remote_visibility = "Public";
        // const string asset_file = "my_asset_name";
        string asset_path = Path.Combine("assets","hello.txt");
        // const string asset_type = "text/plain";

        string upload_auth = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(TWILIO_ACCOUNT_SID + ":" + TWILIO_AUTH_TOKEN));

        string url = $"https://serverless-upload.twilio.com/v1/Services/{serviceSid}/Assets/{assetSid}/Versions";

        // Using RestSharp

        var client = new RestClient(url);
        client.Timeout = -1;

        var request = new RestRequest(Method.POST);
        request.AddHeader("Authorization", upload_auth);
        request.AddFile("Content", asset_path);
        request.AddParameter("Path", remote_path);
        request.AddParameter("Visibility", remote_visibility);

        IRestResponse response = client.Execute(request);
        Console.WriteLine(response.Content);

        // Send an SMS
        
        TwilioClient.Init(TWILIO_ACCOUNT_SID,TWILIO_AUTH_TOKEN);

        var message = MessageResource.Create(
		    body: "Greetings! The current time is: " + DateTime.Now + " GRS5QXSGUQAXNJT",
		    from: new Twilio.Types.PhoneNumber("+19724402326"),
		    to: new Twilio.Types.PhoneNumber("+12147502326")
		);



		Console.WriteLine(message.Sid);
        }
    }
}
