using System.Collections;
using Library.BL;
using Library.BL.ModelsDTO;
using Library.BL.ModelsDTO.BookDto;
using Library.BL.ModelsDTO.TakeReturn;
using Library.BL.Service;
using Library.Common;
using Library.DAL.Interface;
using Library.DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;


#region DI

// Создание контейнера DI
var services = new ServiceCollection();

// Регистрация репозиториев
services.AddScoped<IGetRepository, GetRepository>();
services.AddScoped<IModificationRepository, ModificationRepository>();
services.AddScoped<IRepositoryManager, RepositoryManager>();

services.AddSingleton<ISqlUserService, SqlUserProvider>();
services.AddSingleton<ISqlBookService, SqlBookProvider>();
services.AddSingleton<ISqlTakeReturnBookProvider, SqlTakeReturnBookProvider>();

// Регистрация сервисов
services.AddScoped<IUserService, UserService>();
services.AddScoped<IBookService, BookService>();
services.AddScoped<ITakeReturnBookService, TakeReturnBookService>();

services.AddScoped<ServiceManager>();

// Регистрация логгера
services.AddScoped<IMessageLogger, ConsoleLogger>();

// Построение провайдера
var serviceProvider = services.BuildServiceProvider();

// Получение ServiceManager
var serviceManager = serviceProvider.GetRequiredService<ServiceManager>();
#endregion



var takeReturn =  serviceManager.TakeReturnBookService.GetAllEntities();

List<TakeReturnBookDto> books = [];

foreach ( var book in takeReturn)
{
    var takeBook = new TakeReturnBookDto
    {
        NameAuthor = book.NameAuthor,
        TitleBook = book.TitleBook,
        DateIssuance = book.DateIssuance,
        DateReturn = book.DateReturn

    };
    books.Add(takeBook);
}

Print(books);

//var param = new Dictionary<string, object>()
//{
//    { "login", "kolodin21" },
//    { "password", "978509qq" }
//};

//var user = serviceManager.UserService.GetSingleEntityByParam(param);
//Console.WriteLine(user);

//serviceManager.UserService.DeleteEntity(param);

//var users = serviceManager.UserService.GetAllEntities();
//////var books = serviceManager.BookService.GetAllEntities();
//Print(users);

//{
//    {"name","Иван"}
//};
//.UserService.GetSingleEntityByParam(param);

//Console.WriteLine(books);
//Print(books);


//var book = serviceManager.BookService.GetSingleEntityByParam(param);
//Console.WriteLine(user);

//var newDto = new UserUpdatePersonalInfoDto
//{
//    Id = 1,
//    Surname = "Колодин"
//};

//serviceManager.UserService.UpdateEntity(newDto);

//var userByParam = serviceManager.UserService.GetUserByParam(param);

//Console.WriteLine(userByParam);


//var userDto = new UserAddDto(
//    "Andreev",
//    "Ale",
//    "ich",
//    "petrov",
//    "supersessword123",
//    "petrov@example.com",
//    "+79018234567");

//var book = new BookAddDto(
//    "Тест",
//    1,
//    1842,
//    3,
//    4,
//    12
//    );

//serviceManager.BookService.AddEntity(book);

//var bookUpdate = new BookUpdateInfoDto{Id = 1,Quantity = 15};

//serviceManager.BookService.UpdateEntity(bookUpdate);

//serviceManager.UserService.DeleteEntity(param);

//logger.Log(serviceManager.UserService.AddEntity(userDto) ? "Успешно" : "Не успешно");

//var param = new Dictionary<string, object>()
//{
//    { "id", 5 },
//};

//var param1 = new Dictionary<string, object>()
//{
//    { "surname", "Алексеев" },
//    { "id",5 }
//};


//serviceManager.UserService.AddUser(userDto);
//serviceManager.UserService.DeleteUser(25);
//serviceManager.UserService.UpdateUser(param1);

//Print(users);

return;      

void Print(IEnumerable? entity)
{
    if (entity == null) return;
    foreach (var ent in entity)
    {
        Console.WriteLine(ent);
    }

    Console.WriteLine();
}

public class ConsoleLogger : IMessageLogger
{
    public void Log(string message)
    {
        Console.WriteLine(message);
    }
}