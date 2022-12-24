
using FactGenerator.Helpers;

Console.WriteLine($"[{DateTimeOffset.UtcNow}] [INF]: Starting Fact Service...");
Task.Run(async () =>
{
    await HtmlHelper.RunAsync();
});


Console.ReadKey();
