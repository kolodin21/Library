using Library.BL.Models;

namespace Library.BL
{
    #region Interface
    public interface ISqlBaseProvider
    {
        public string GetColumnAndTypeTable { get; }
    }

    public interface ISqlDelete
    {
        public string Delete { get; }
    }

    public interface ISqlProvider<T> : ISqlBaseProvider
    {
        public string GetAll { get; }
        public string GetByParam { get; }
        public string Add { get; }
        public string MainNameTable { get; }
    }

    //-------------------------------------//
    public interface ISqlUserProvider : ISqlProvider<User>,ISqlDelete
    {
        public string NamePersonTable { get; }
    }

    public interface ISqlTakeReturnBookProvider : ISqlProvider<TakeReturnBooks>
    {
        public string ReturnBook { get; }
    }

    public interface ISqlBookProvider : ISqlProvider<Book>, ISqlDelete{}

    #endregion

    #region Class
    public abstract class SqlBaseProvider : ISqlBaseProvider
    {
        public string GetColumnAndTypeTable =>
            @"
                 SELECT 
                a.attname AS column_name
            FROM 
                pg_catalog.pg_attribute a
            WHERE 
                a.attrelid = (
                    SELECT oid 
                    FROM pg_catalog.pg_class
                    WHERE relname = @TableName
                      AND relnamespace = (
                          SELECT oid 
                          FROM pg_catalog.pg_namespace 
                          WHERE nspname = @SchemaName
                      )
                )
            AND a.attnum > 0 
            AND NOT a.attisdropped;";
    }

    public class SqlUserProvider : SqlBaseProvider, ISqlUserProvider
    {
        public string GetAll => @"SELECT * FROM view_users";
        public string GetByParam => @"SELECT * FROM view_users WHERE 1=1";
        public string Add => @"addUser";
        public string Delete => @"deleteUser";
        public string MainNameTable => @"table_users";
        public string NamePersonTable => @"table_persons";
    }

    public class SqlBookProvider : SqlBaseProvider, ISqlBookProvider
    {
        public string GetAll => @"SELECT * FROM view_books_v2";
        public string GetByParam => @"SELECT * FROM view_books_v2 WHERE 1=1";
        public string Add => @"addBook";
        public string Delete => @"deleteBook";
        public string MainNameTable => @"table_books";
    }

    public class SqlTakeReturnBookProvider : SqlBaseProvider, ISqlTakeReturnBookProvider
    {
        public string GetAll => @"SELECT * FROM view_issuance_return_books";
        public string GetByParam => @"SELECT * FROM table_issuance_return_books WHERE 1=1";
        public string Add => @"takeBook";
        public string ReturnBook => @"returnBook";
        public string MainNameTable => @"table_issuance_return_books";
    }

    public class SqlAuthorProvider : SqlBaseProvider,ISqlProvider<Author>
    {
        public string GetAll => @"SELECT * FROM table_author";
        public string GetByParam => @"SELECT * FROM table_author WHERE 1=1";
        public string Add => @"addAuthor";
        public string MainNameTable => @"table_author";
    }

    public class SqlPublisherProvider : SqlBaseProvider, ISqlProvider<Publisher>
    {
        public string GetAll => @"SELECT * FROM table_publishers";
        public string GetByParam => @"SELECT * FROM table_publishers WHERE 1=1";
        public string Add => @"addPublisher";
        public string MainNameTable => @"table_publishers";
    }

    public class SqlConditionProvider : SqlBaseProvider, ISqlProvider<Condition>
    {
        public string GetAll => @"SELECT * FROM table_conditions";
        public string GetByParam => @"SELECT * FROM table_conditions WHERE 1=1";
        public string Add => @"addCondition";
        public string MainNameTable =>@"table_conditions";
    }

    #endregion
}
