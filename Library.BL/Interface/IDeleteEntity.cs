namespace Library.BL.Interface;

public interface IDeleteEntity<in T>
{
    bool DeleteEntity(T entity);
    bool DeleteEntity(Dictionary<string, object> param);
}