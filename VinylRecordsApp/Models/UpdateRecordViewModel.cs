namespace VinylRecordsApp.Models
{
    public class UpdateRecordViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Label { get; set; }

        public int Price { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string Description { get; set; }
    }
}
