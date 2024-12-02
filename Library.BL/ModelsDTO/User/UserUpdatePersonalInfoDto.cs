namespace Library.BL.ModelsDTO.User;

public class UserUpdatePersonalInfoDto
{
    public int Id { get; set; }
    public string? Surname { get; set; }
    public string? Name { get; set; }
    public string? Patronymic { get; set; }
}