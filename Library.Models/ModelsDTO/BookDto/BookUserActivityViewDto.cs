namespace Library.Models.ModelsDTO
{
    public record BookUserActivityViewDto : BaseBook
    {
        public DateTime DateIssuance { get; set; }
    }

    public record BookUserHistoryViewDto : BaseBook
    {
        public DateTime DateIssuance { get; set; }
        public DateTime DateReturn { get; set; }
    }
}
