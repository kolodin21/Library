using Library.Infrastructure;
using Library.Server.BL.Service;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseHttpsRedirection();


#region CreateDi
//Создание DI контейнера
IServiceCollection service = new ServiceCollection();

service.AddInfrastructure();

IServiceProvider serviceProvider = service.BuildServiceProvider();

var serviceManager = serviceProvider.GetRequiredService<ServiceManager>();

#endregion

//Логгер
 Logger Logger = LogManager.GetCurrentClassLogger();


 #region User

app.MapGet("/AllUsers",async () =>
{
    Logger.Info("Запрос на получение всех пользователей отправлен!");
    return await serviceManager.UserService.GetAllEntitiesAsync();
});


app.MapPost("/SingleUser", async ([FromBody] Dictionary<string, JsonElement> param) =>
{
    var parsedParams = param.ToDictionary(
        kvp => kvp.Key,
        kvp => JsonElementToObject(kvp.Value)
    );
    return await serviceManager.UserService.GetSingleEntityByParamAsync(parsedParams);
});

 #endregion

 static object JsonElementToObject(JsonElement element)
{
    return element.ValueKind switch
    {
        JsonValueKind.String => element.GetString()!,
        JsonValueKind.Number => element.TryGetInt64(out long l) ? l : element.GetDouble(),
        JsonValueKind.True => true,
        JsonValueKind.False => false,
        JsonValueKind.Null => null!,
        _ => element.GetRawText()
    };
}


app.Run();