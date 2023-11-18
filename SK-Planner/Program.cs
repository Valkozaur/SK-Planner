using Microsoft.Extensions.Configuration;
using System;

namespace SK_Planner
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<Program>(); // Program is your console application class

            IConfigurationRoot configuration = builder.Build();
            var endpoint = configuration["Endpoint"];
            var key = configuration["Key"];
            var completionModel = configuration["CompletionModel"];
            var embeddingModel = configuration["EmbeddingModel"];

            Console.WriteLine($"Endpoint: {endpoint}");
            Console.WriteLine($"Key: {key}");
            Console.WriteLine($"CompletionModel: {completionModel}");
            Console.WriteLine($"EmbeddingModel: {embeddingModel}");

            Console.WriteLine("Hello, World!");
        }
    }
}
