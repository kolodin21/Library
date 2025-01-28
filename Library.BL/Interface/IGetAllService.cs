namespace Library.Server.BL.Interface
{
    public interface IGetAllService<T>
    {
        Task<IEnumerable<T>?> GetAllEntitiesAsync();
    }
}
