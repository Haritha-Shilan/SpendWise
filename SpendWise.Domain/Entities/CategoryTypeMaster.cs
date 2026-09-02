namespace SpendWise.Domain.Entities
{
    public class CategoryTypeMaster:IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } =string.Empty;
    }
}
