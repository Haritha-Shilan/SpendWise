using SpendWise.Domain.Interface;

namespace SpendWise.Domain.Entities
{
    public class PaymentMethod:IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
}
