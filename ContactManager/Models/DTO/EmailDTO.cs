namespace ContactManager_API.Models.DTO
{
    public class EmailDTO
    {
        public long Id { get; set; }
        public bool IsPrimary { get; set; }
        public string Address { get; set; } = default!;
    }
}
