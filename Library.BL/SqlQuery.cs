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

        public static string NamePersonTable => @"table_persons";

        public static string NameUsersTable => @"table_users";
        #endregion

        #region Book

        //public static string GetAllBooks => @"SELECT * FROM view_books_library";

        public static string GetAllBooks => @" SELECT
                                            tb.id as Id,
                                            tb.title as Title,
                                            tb.year as Year,
                                            tb.quantity as Quantity,
                                            lb.balance_book as BalanceBook,
                                            lb.count_activity as CountActivity,
                                            lb.count_book_issuance as CountBookIssuance,
                                            tau.id as Author_Id,
                                            tau.name as AuthorName,          
                                            tp.id as Publisher_Id,
                                            tp.name as PublisherName,       
                                            tc.id as Condition_Id,
                                            tc.name as ConditionName         
                                        FROM table_books as tb
                                        JOIN library.table_author as tau ON tb.author_id = tau.id
                                        JOIN library.table_publishers as tp on tb.publisher_id = tp.id
                                        JOIN library.table_conditions  as tc ON tb.condition_id = tc.id
                                        JOIN library.table_library_books as lb on tb.id = lb.book_id";

        public static string GetBook => @"SELECT * FROM view_books ";

        public static string GetBookByParam => @"SELECT * FROM view_books WHERE 1=1";


        #endregion


    }
}
