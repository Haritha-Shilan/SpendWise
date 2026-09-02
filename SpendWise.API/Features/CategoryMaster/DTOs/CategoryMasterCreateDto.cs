namespace SpendWise.API.Features.CategoryMaster.DTOs
{
    public class CategoryMasterCreateDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Range(1,int.MaxValue)]
        public int TypeId { get; set; }
    }
}
