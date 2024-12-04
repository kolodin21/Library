namespace Library.BL
{
    public interface ISqlBaseService
    {
        public string GetColumnAndTypeTable { get; }
    }
    public interface ISqlCommonService : ISqlBaseService
    {
        public string GetAll { get; }
        public string GetByParam { get; }
        public string Add { get; }
        public string Delete { get; }
    }
    public interface ISqlUserService : ISqlCommonService 
    {
        public string NameUserTable{ get; }
        public string NamePersonTable { get; }
    }

    public interface ISqlBookService : ISqlCommonService
    {
        public string NameBookTable { get; }
        public string NameLibraryBookTable { get; }
        //public string GetAllAuthor { get; }
        //public string GetAllPublisher { get; }
        //public string GetAllConditions { get; }
    }

    public interface ISqlTakeReturnBookProvider : ISqlCommonService
    {
        public string NameTakeReturnTable { get; }
        public string Return { get; }
    }

    public abstract class SqlBaseProvider : ISqlBaseService
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

    public class SqlUserProvider : SqlBaseProvider,ISqlUserService
    {
        public string GetAll => @"SELECT * FROM view_users";
        public string GetByParam => @"SELECT * FROM view_users WHERE 1=1";
        public string Add => @"addUser";
        public string Delete => @"deleteUser";

        public string NameUserTable => @"table_users";
        public string NamePersonTable => @"table_persons";
    }
    public class SqlBookProvider : SqlBaseProvider, ISqlBookService
    {
        public string GetAll => @"SELECT * FROM view_books_v2";
        public string GetByParam => @"SELECT * FROM view_books_v2 WHERE 1=1";
        public string Add => @"addBook";
        public string Delete => @"deleteBook";
        public string NameLibraryBookTable => @"table_library_books";
        public string NameBookTable => @"table_books";
        //public string GetAllAuthor => @"SELECT * FROM table_author";
        //public string GetAllPublisher => @"SELECT * FROM table_publishers";
        //public string GetAllConditions => @"SELECT * FROM table_conditions";
    }

    public class SqlTakeReturnBookProvider : SqlBaseProvider, ISqlTakeReturnBookProvider
    {
        public string GetAll => @"SELECT * FROM view_issuance_return_books";

        public string GetByParam => @"SELECT * FROM table_issuance_return_books WHERE 1=1";
        public string Add => @"takeBook";
        public string Return => @"returnBook";
        public string Delete => throw new NotImplementedException();
        public string NameTakeReturnTable => @"table_issuance_return_books";

    }
}
