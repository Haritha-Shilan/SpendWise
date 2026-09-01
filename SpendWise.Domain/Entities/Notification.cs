using SpendWise.Domain.Interface;

namespace SpendWise.Domain.Entities
{
    public class Notification :IEntity
    {
        public int Id { get; set; }

        public string Message { get; set; } = string.Empty;

        public bool IsRead { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? ReadAt { get; set; }
    }
}
