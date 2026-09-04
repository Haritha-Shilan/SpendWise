namespace SpendWise.API.Features.UserCategory.DTOs
{
    public class UserCategoryResponseDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int TypeId { get; set; }

        public string TypeName { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
}
