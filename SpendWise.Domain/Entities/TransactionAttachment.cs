using SpendWise.Domain.Interface;

namespace SpendWise.Domain.Entities
{
    public class TransactionAttachment:IEntity
    {
        public int Id { get; set; }

        public int TransactionId { get; set; }

        public Transaction? Transaction { get; set; }

        public string FileName { get; set; } = string.Empty;

        public string FilePath { get; set; }=string.Empty;

        public string ContentType { get; set; }= string.Empty;

        public DateTime UploadedAt { get; set; }
    }
}
