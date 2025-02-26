﻿namespace Library.Models.ModelsDTO
{
    public class BookUpdateInfoDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int? AuthorId { get; set; }
        public int? Year { get; set; }
        public int? PublisherId { get; set; }
        public int? ConditionId { get; set; }
        public int? Quantity { get; set; }
    }
}