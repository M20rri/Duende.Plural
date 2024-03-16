namespace Airlines.Api.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Passenger { get; set; } = default!;
        public string NationalId { get; set; } = default!;
        public string From { get; set; } = default!;
        public string To { get; set; } = default!;
        public int Status { get; set; } = default!;
    }
}
