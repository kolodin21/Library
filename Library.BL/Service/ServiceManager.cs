using Library.BL.Interface;
using Library.BL.ModelsDTO.BookDto;
using Library.BL.ModelsDTO.User;
using Library.DAL.Models;

namespace Library.BL.Service
{
    public interface IUserService :
        IGetService<User>,
        IAddService<UserAddDto>,
        //IDeleteService<UserAddDto>,
        //IDeleteServiceByParam,
        IDeleteServiceByIdProcedure,
        IUpdateService<UserUpdatePersonalInfoDto>,
        IUpdateService<UserUpdateContactInfoDto> { }

    public interface IBookService :
        IGetService<Book>,
        IAddService<BookAddDto>,
        IUpdateService<BookUpdateInfoDto>,
        IDeleteServiceByIdProcedure { }


    public interface ITakeReturnBookService :
        IGetService<TakeReturnBooks>{}

    public class ServiceManager
    {
        public IUserService UserService { get; }
        public IBookService BookService { get; }
        public ITakeReturnBookService TakeReturnBookService { get; }

        public ServiceManager(
            IUserService userService, 
            IBookService bookService, 
            ITakeReturnBookService takeReturnBookService)
        {
            UserService = userService;
            BookService = bookService;
            TakeReturnBookService = takeReturnBookService;
        }
    }
}
