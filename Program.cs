using QuestionnaireFillingService;
using System;
using System.Text.RegularExpressions;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddControllers();
        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession();
        var app = builder.Build();
        app.UseSession();
        app.UseStaticFiles();
        app.Map("/", async (context) =>
        {
            var response = context.Response;
            response.ContentType = "text/html; charset=utf-8";
            await response.SendFileAsync("html/index.html");
        });

        app.MapControllers();
        app.Run();
    }
}
