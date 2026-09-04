namespace SpendWise.API.Features.UserCategory.DTOs
{
    public class UserCategoryCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(1,int.MaxValue)]
        public int TypeId { get; set; }
    }
}
