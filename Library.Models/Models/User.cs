﻿namespace Library.Models
{
    public record User
    {
        public int Id { get; set; }
        public required string Surname { get; set; }
        public required string Name { get; set; }
        public string? Patronymic { get; set; }
        public required string Login { get; set; }
        public required string Phone { get; set; }
        public required string Email { get; set; }
        public int CountBooksActivity { get; set; }
        public int CountBooksTaken { get; set; }
        public bool IsActiveUser { get; set; }
    }
}