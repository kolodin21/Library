﻿namespace Library.BL.ModelsDTO
{
    public class UpdateUserDto
    {
        public int Id { get; set; }
        public string? Surname { get; set; }
        public string? Name { get; set; }
        public string? Patronymic { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

    public class UserPersonalInfoDto
    {
        public int Id { get; set; }
        public string? Surname { get; set; }
        public string? Name { get; set; }
        public string? Patronymic { get; set; }
    }

    public class UserContactInfoDto
    {
        public int Id { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}
