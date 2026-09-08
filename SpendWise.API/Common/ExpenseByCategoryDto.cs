namespace SpendWise.API.Common
{
    public class ExpenseByCategoryDto
    {
        public string CategoryName { get; set; } = string.Empty;

        public decimal Amount { get; set; }
    }
}
