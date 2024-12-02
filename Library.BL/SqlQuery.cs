namespace Library.BL
{
    public static class SqlQuery
    {
        #region User
        public static string GetAllUsers => @"SELECT * FROM view_users";

        public static string GetUserByParam => @"SELECT * FROM view_users WHERE 1=1";

        public static string AddUser => @"addUser";

        public static string DeleteUser => @"deleteUser";

        public static string NamePersonTable => @"table_persons";

        public static string NameUsersTable => @"table_users";

        //public static string DeleteAuthorByParam => @"DELETE FROM table_author WHERE ";
        #endregion

        #region Book

        public static string GetBooks => @"SELECT * FROM view_books_v2 ";
        public static string GetBookByParam => @"SELECT * FROM view_books_v2 WHERE 1=1";

        public static string AddBook => @"addBook";
        public static string NameBookTable => @"table_books";

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
