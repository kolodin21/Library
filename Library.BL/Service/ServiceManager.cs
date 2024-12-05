using Library.BL.Interface;
using Library.BL.ModelsDTO.BookDto;
using Library.BL.ModelsDTO.Others;
using Library.BL.ModelsDTO.TakeReturn;
using Library.BL.ModelsDTO.User;
using Library.DAL.Models;

namespace Library.BL.Service
{
    #region Interface

    public interface IGetService<TEntity> :
        IGetAllService<TEntity>,
        IGetSingleByParam<TEntity>,
        IGetByParamService<TEntity>{}

    public interface IEntityService<TEntity> :
        IGetAllService<TEntity>,
        IGetSingleByParam<TEntity>,
        IDeleteService<TEntity>,
        IUpdateService<TEntity>
    { }

    public interface IUserService : 
        IGetService<User>,
        IAddService<UserAddDto>,
        IDeleteServiceByIdProcedure,
        IUpdateService<UserUpdatePersonalInfoDto>,
        IUpdateService<UserUpdateContactInfoDto> { }

    public interface IBookService : 
        IGetService<Book>,
        IAddService<BookAddDto>,
        IUpdateService<BookUpdateInfoDto>,
        IDeleteServiceByIdProcedure { }


    public interface ITakeReturnBookService : 
        IGetService<TakeReturnBooks>,
        IAddService<TakeBookDto>,
        IAddService<ReturnBookDto>{}

    public interface IAuthorService : IEntityService<Author>, IAddService<AuthorDto>;

    public interface IConditionService : IEntityService<Condition>, IAddService<ConditionDto>;

    public interface IPublisherService : IEntityService<Publisher>, IAddService<PublisherDto>;

    //Todo
    //Переработать интерфейсы

    #endregion

    #region ServiceManager
    public class ServiceManager
    {
        public UserService UserService { get; }
        public BookService BookService { get; }
        public TakeReturnBookService TakeReturnBookService { get; }
        public AuthorService AuthorService { get; }
        public ConditionService ConditionService { get; }
        public PublisherService PublisherService { get; }

        public ServiceManager
        (
            UserService userService, 
            BookService bookService, 
            TakeReturnBookService takeReturnBookService,
            AuthorService authorService,
            ConditionService conditionService,
            PublisherService publisherService)
        {
            UserService = userService;
            BookService = bookService;
            TakeReturnBookService = takeReturnBookService;
            AuthorService = authorService;
            ConditionService = conditionService;
            PublisherService = publisherService;
        }
    }
    #endregion
}
