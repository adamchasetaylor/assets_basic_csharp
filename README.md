# assets_basic_csharp

This is a much simpler example than https://github.com/adamchasetaylor/assets_in_10_seconds

Unlike assets in 10 seconds, it does not perform any cleanup.

This script relies on [RestSharp](http://restsharp.org/) and [Json.NET](https://www.newtonsoft.com/json)

This script reads TWILO_ACCOUNT_SID and TWILIO_AUTH_TOKEN from your environment.

Then the script does the following:

1. Creates a Service using Helper Library
2. Creates an Environment using Helper Library
3. Creates an Asset using Helper Library
4. Creates an Asset Version with Public Visibility using RestSharp
5. Creates a Build Using Helper Library
6. Waits 10 seconds for Build to Finish
7. Creates a Deployment Using Helper Library
8. Prints the Domain Name and Path of Asset
