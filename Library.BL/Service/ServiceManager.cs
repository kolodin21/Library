using Library.BL.Interface;
using Library.DAL.Repositories;
using Library.Common;
using Library.BL.ModelsDTO;

namespace Library.BL.Service
{
    public class ServiceManager
    {
        public UserService UserService { get; }
        public BookService BookService { get; }

        public ServiceManager(IMessageLogger logger)
        {
            // Создаем конкретные реализации репозиториев
            var getRepository = new GetRepository(logger);
            var cudRepository = new ModificationRepository(logger);

            // Передаем их в RepositoryManager
            var repositoryManager = new RepositoryManager(getRepository, cudRepository);

            UserService = new UserService(repositoryManager);
            BookService = new BookService(repositoryManager);

           
        }
    }
}
