using SpendWise.Domain.Interface;

namespace SpendWise.Domain.Entities
{
    public class CategoryMaster:IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int TypeId { get; set; }

        public CategoryTypeMaster? CategoryTypeMaster { get; set; }

        public bool IsActive { get; set; }
    }
}
