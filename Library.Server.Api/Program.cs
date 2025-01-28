using Library.BL.Service;
using Library.Infrastructure;

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


app.MapGet("/AllUsers",async ()=> await serviceManager.UserService.GetAllEntitiesAsync());





app.Run();

