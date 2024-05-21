using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;

public static class MoodHttpTrigger
{
    [FunctionName("MoodHttpTrigger")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");

        string name = req.Query["name"];
        string mood = req.Query["mood"];

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        dynamic data = JsonConvert.DeserializeObject(requestBody);
        name = name ?? data?.name;
        mood = mood ?? data?.mood;

        string responseMessage = CreateResponseMessage(name, mood);

        return new OkObjectResult(responseMessage);
    }

    private static string CreateResponseMessage(string name, string mood)
    {
        if (string.IsNullOrEmpty(name))
        {
            return "Please provide your name in the query string or in the request body.";
        }

        string message;

        switch (mood?.ToLower())
        {
            case "happy":
                message = $"Hey {name}! It's awesome to see you happy! 😊";
                break;
            case "sad":
                message = $"Oh no, {name}. I'm here if you need to talk. 😢";
                break;
            case "excited":
                message = $"Wow {name}! Your excitement is contagious! 🎉";
                break;
            case "angry":
                message = $"Take a deep breath, {name}. Everything will be fine. 😡";
                break;
            case "bored":
                message = $"Hey {name}, why don't you try learning a new skill? 🤔";
                break;
            default:
                message = $"Hello {name}! Here's a joke for you: Why don't scientists trust atoms? Because they make up everything! 😂";
                break;
        }

        return message;
    }
}
