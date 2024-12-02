using System.Collections;
using Library.BL;
using Library.BL.ModelsDTO;
using Library.BL.Service;
using Library.Common;


var logger = new ConsoleLogger();

var serviceManager = new ServiceManager(logger);

//var users = serviceManager.UserService.GetAllEntities();
//var books = serviceManager.BookService.GetAllEntities();
//Print(books);

var param = new Dictionary<string, object>()
{
    {"name","Иван"}
};
var books = serviceManager.UserService.GetEntitiesByParam(param);
//Console.WriteLine(books);
Print(books);

//var book = serviceManager.BookService.GetSingleEntityByParam(param);
//Console.WriteLine(user);

//var newDto = new UserPersonalInfoDto
//{
//    Id = 1,
//    Surname = "Колодин"
//};

//serviceManager.UserService.UpdateEntity(newDto);

//var userByParam = serviceManager.UserService.GetUserByParam(param);

//Console.WriteLine(userByParam);


//var userDto = new UserDto(
//    "Andreev",
//    "Ale", 
//    "ich",
//    "petrov",
//    "supersessword123", 
//    "petrov@example.com",
//    "+79018234567");

//serviceManager.UserService.DeleteEntity(param);

//logger.Log(serviceManager.UserService.ExecuteEntity(userDto) ? "Успешно" : "Не успешно");

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