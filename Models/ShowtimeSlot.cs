namespace MovieTicketsBookings.Models
{
    public class ShowtimeSlot
    {
        public int ShowtimeId { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string RoomName { get; set; }
    }
}
