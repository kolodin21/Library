namespace Library.BL
{
    public interface ISqlQueryBaseService
    {
        public string GetColumnAndTypeTable { get; }
    }
    public interface ISqlQueryCommonService : ISqlQueryBaseService
    {
        public string GetAll { get; }
        public string GetByParam { get; }
        public string Add { get; }
        public string Delete { get; }
    }
    public interface ISqlQueryUserService : ISqlQueryCommonService 
    {
        public string NameUserTable{ get; }
        public string NamePersonTable { get; }
    }

    public interface ISqlQueryBookService : ISqlQueryCommonService
    {
        public string NameBookTable { get; }
        public string GetAllAuthor { get; }
        public string GetAllPublisher { get; }
        public string GetAllConditions { get; }
    }


    public abstract class SqlQueryBaseProvider : ISqlQueryBaseService
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

    public class SqlQueryUserProvider : SqlQueryBaseProvider,ISqlQueryUserService
    {
        public string GetAll => @"SELECT * FROM view_users";
        public string GetByParam => @"SELECT * FROM view_users WHERE 1=1";
        public string Add => @"addUser";
        public string Delete => @"deleteUser";

        public string NameUserTable => @"table_users";
        public string NamePersonTable => @"table_persons";
    }
    public class SqlQueryBookProvider : SqlQueryBaseProvider, ISqlQueryBookService
    {
        public string GetAll => @"SELECT * FROM view_books_v2";
        public string GetByParam => @"SELECT * FROM view_books_v2 WHERE 1=1";
        public string Add => @"addBook";
        public string Delete => @"table_books";//Fixme ==========================================

        public string NameBookTable => @"table_books";
        public string GetAllAuthor => @"SELECT * FROM table_author";
        public string GetAllPublisher => @"SELECT * FROM table_publishers";
        public string GetAllConditions => @"SELECT * FROM table_conditions";
    }
}
