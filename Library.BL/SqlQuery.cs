namespace Library.BL
{

    public interface ISqlQueryUserService
    {
        public string GetAllUsers { get; }
        public string GetUserByParam { get; }
        public string AddUser { get; }
        public string DeleteUser { get; }
        public string NamePersonTable { get; }
        public string NameUsersTable { get; }
    }

    public  class SqlQueryUserProvider : ISqlQueryUserService
    {
        public string GetAllUsers => @"SELECT * FROM view_users";
        public  string GetUserByParam => @"SELECT * FROM view_users WHERE 1=1";
        public string AddUser => @"addUser";
        public string DeleteUser => @"deleteUser";
        public string NamePersonTable => @"table_persons";
        public string NameUsersTable => @"table_users";
    }

    public interface ISqlQueryBookService
    {
        public string GetBooks { get; }
        public string GetBookByParam { get; }
        public  string AddBook { get; }
        public string NameBookTable { get; }
    }

    public class SqlQueryBookProvider : ISqlQueryBookService
    {
    //public static string DeleteAuthorByParam => @"DELETE FROM table_author WHERE ";

        #region Book

        public string GetBooks => @"SELECT * FROM view_books_v2 ";
        public string GetBookByParam => @"SELECT * FROM view_books_v2 WHERE 1=1";

        public string AddBook => @"addBook";
        public string NameBookTable => @"table_books";

        #endregion

        public static string GetAllAuthor => @"SELECT * FROM table_author";
        public static string GetAllPublisher => @"SELECT * FROM table_publishers";
        public static string GetAllConditions => @"SELECT * FROM table_conditions";

        //================================================================================//


        // Запрос для получения названий колонок и их типов
        public static string GetColumnAndTypeTable => @"
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
}
