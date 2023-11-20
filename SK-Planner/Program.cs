using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.AI.OpenAI;
using Microsoft.SemanticKernel.Planners;

namespace SK_Planner
{
    class Program
    {
        static async Task Main()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<Program>(); // Program is your console application class

            IConfigurationRoot configuration = builder.Build();
            var endpoint = configuration["Endpoint"]!;
            var key = configuration["Key"]!;
            var completionModel = configuration["CompletionModel"]!;
            var embeddingModel = configuration["EmbeddingModel"]!;

            IKernel kernel = new KernelBuilder()
                .WithLoggerFactory(LoggerFactory.Create(b =>
                {
                    b.AddConsole();
                    b.SetMinimumLevel(LogLevel.Trace);
                }))
                .WithAzureOpenAIChatCompletionService(deploymentName: completionModel, endpoint: endpoint, apiKey: key)
                .WithAzureOpenAITextEmbeddingGenerationService(deploymentName: embeddingModel, endpoint: endpoint, apiKey: key)
                .Build();

            var mathPlugin = kernel.ImportFunctions(new MathSkill(), "MathPlugin");

            // Create planner
            var planner = new SequentialPlanner(kernel);

            var ask = "If my investment of 2130.23 dollars increased by 23%, how much would I have after I spent $5 on a latte?";

            var plan = await planner.CreatePlanAsync(ask);

            // Execute the plan
            var result = await kernel.RunAsync(plan);

            Console.WriteLine("Plan results:");
            Console.WriteLine(result.GetValue<string>()!.Trim());
        }
    }
}
