using Library.Models;
using Library.Models.ModelsDTO;
using Library.Server.BL.Interface;

namespace Library.Server.BL.Service
{
    #region Interface

    public interface ICustomBookService<T> 
    {
        Task<IEnumerable<T>?> GetBookActivityUserAsync(Dictionary<string, object> param);
    }

    public interface IGetService<TEntity> :
        IGetAllService<TEntity>,
        IGetByParamService<TEntity>,
        IGetSingleByParam<TEntity>{}

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
        ICustomBookService<BookViewDto>,
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
