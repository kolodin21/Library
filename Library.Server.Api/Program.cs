using Library.Infrastructure;
using Library.Server.BL.Service;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Text.Json;
using Library.Models.ModelsDTO;

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
 var Logger = LogManager.GetCurrentClassLogger();


#region User

app.MapGet("/AllUsers",async () =>
{
    Logger.Info("Запрос на получение всех пользователей отправлен!");
    return await serviceManager.UserService.GetAllEntitiesAsync();
});


app.MapPost("/SingleUser", async ([FromBody] Dictionary<string, JsonElement> param) =>
    await serviceManager.UserService.GetSingleEntityByParamAsync(ParsedParam(param)));



app.MapPost("/AddUser", async (UserAddDto userAddDto) =>
    await serviceManager.UserService.AddEntityAsync(userAddDto));

#endregion

#region Book

app.MapPost("/ActivityBooks", async ([FromBody] Dictionary<string, JsonElement> param) =>
    await serviceManager.BookService.GetBookActivityUserAsync(ParsedParam(param)));

#endregion


#region Methods

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

static Dictionary<string, object> ParsedParam(Dictionary<string, JsonElement> param)
{
    var parsedParams = param.ToDictionary(
        kvp => kvp.Key,
        kvp => JsonElementToObject(kvp.Value)
    );
    return parsedParams;
}

#endregion

app.Run();