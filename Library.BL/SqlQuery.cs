namespace Library.BL
{
    public static class SqlQuery
    {
        #region User
        public static string GetAllUsers => @"SELECT * FROM view_users";

        public static string GetUserByParam => @"SELECT * FROM view_users WHERE 1=1";

        public static string AddUser => @"addUser";

        public static string DeleteUser => @"deleteUser";

        public static string DeleteAuthorByParam => @"DELETE FROM table_author WHERE ";

        public static string UserTableName => @"table_persons";
        #endregion

        #region Book

        public static string GetAllBooks => @"SELECT * FROM view_books_library";

        public static string GetBookByParam => @"SELECT * FROM view_books_library WHERE 1=1";


        #endregion


    }
}
