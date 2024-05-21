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
                message = $"Hey {name}! ¡Es genial verte feliz!! 😊";
                break;
            case "sad":
                message = $"Oh no, {name}. Estoy aquí si necesitas hablar. 😢";
                break;
            case "excited":
                message = $"Wow {name}! Tu emoción es contagiosa! 🎉";
                break;
            case "angry":
                message = $"Respira hondo, {name}. Everything will be fine. 😡";
                break;
            case "bored":
                message = $"Hey {name}, ¿Por qué no intentas aprender una nueva habilidad? 🤔";
                break;
            default:
                message = $"Hola {name}! Aquí tienes un chiste: ¿Por qué los científicos no confían en los átomos? ¡Porque ellos lo componen todo! 😂";
                break;
        }

        return message;
    }
}
