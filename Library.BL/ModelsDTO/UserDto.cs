namespace Library.BL.ModelsDTO
{
    public class UserDto(string surname, string name, string? patronymic, string login, string password, string email, string phone)
    {
        public string Surname { get; set; } = surname;
        public string Name { get; set; } = name;
        public string? Patronymic { get; set; } = patronymic;
        public string Login { get; set; } = login;
        public string Password { get; set; } = password;
        public string Phone { get; set; } = phone;
        public string Email { get; set; } = email;
    }
}
