namespace Library.BL.ModelsDTO.User;

public class UserUpdatePersonalInfoDto
{
    public int Id { get; set; }
    public string? Surname { get; set; }
    public string? Name { get; set; }
    public string? Patronymic { get; set; }

    public UserUpdatePersonalInfoDto(int id, string surname, string name, string patronymic)
    {
        Id = id;
        Surname = surname; 
        Name = name; 
        Patronymic = patronymic;
    }

    public UserUpdatePersonalInfoDto(int id, string surname, string name)
    {
        Id = id;
        Surname = surname;
        Name = name;
    }

    public UserUpdatePersonalInfoDto(int id, string surname)
    {
        Id = id;
        Surname = surname;
    }
    public UserUpdatePersonalInfoDto (){}
}