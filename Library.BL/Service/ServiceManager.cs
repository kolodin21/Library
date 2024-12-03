using Library.BL.Interface;
using Library.DAL.Repositories;
using Library.Common;
using Library.BL.ModelsDTO;
using Library.BL.ModelsDTO.BookDto;
using Library.BL.ModelsDTO.User;
using Library.DAL.Models;
using Library.BL.ModelsDTO.UserDto.UserDto;

namespace Library.BL.Service
{
    public interface IUserService : 
        IGetService<User>, 
        IAddEntity<UserAddDto>, 
        IDeleteEntity<UserAddDto>, 
        IUpdateService<UserUpdatePersonalInfoDto> , 
        IUpdateService<UserUpdateContactInfoDto> { }

    public interface IBookService : 
        IGetService<Book>, 
        IAddEntity<BookAddDto>, 
        IUpdateService<BookUpdateInfoDto> { }

    public class ServiceManager
    {
        public IUserService UserService { get; }
        public IBookService BookService { get; }

        public ServiceManager(IUserService userService, IBookService bookService)
        {
            UserService = userService;
            BookService = bookService;
        }
    }
}
