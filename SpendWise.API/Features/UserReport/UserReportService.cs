namespace SpendWise.API.Features.UserReport
{
    public class UserReportService : IUserReportService
    {
        private readonly ITransactionQueryService _transactionQueryService;
        private readonly IDateRangeService _dateRangeService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMonthlySummaryService _monthlySummaryService;
        private readonly IExpenseByCategoryService _expenseByCategoryService;
        private readonly IIncomeSummaryService _incomeSummaryService;
        private readonly string _userId;
        public UserReportService(ITransactionQueryService transactionQueryService,
                                 IDateRangeService dateRangeService,
                                 IHttpContextAccessor httpContextAccessor,
                                 IMonthlySummaryService monthlySummaryService,
                                 IExpenseByCategoryService expenseByCategoryService,
                                 IIncomeSummaryService incomeSummaryService)
        {
            _transactionQueryService= transactionQueryService;
            _dateRangeService= dateRangeService;
            _httpContextAccessor= httpContextAccessor;
            _monthlySummaryService = monthlySummaryService;
            _expenseByCategoryService = expenseByCategoryService;
            _incomeSummaryService = incomeSummaryService;
            _userId = GetCurrentUserId();
        }
        public async Task<ServiceResult<UserReportResponseDto>> GetReport(ReportFilterDto filter)
        {
            var dateRangeResult = _dateRangeService.GetDateRange(filter.Period,filter.FromDate,filter.ToDate);

            if (dateRangeResult.Status != ServiceResultStatus.Success)
            {
                return new ServiceResult<UserReportResponseDto>
                {
                    Status = dateRangeResult.Status,
                    Error = dateRangeResult.Error
                };
            }

            var transactions =
                await _transactionQueryService.GetUserTransactionsAsync(
                    _userId,
                    dateRangeResult.Data!);

            var totalIncome = transactions
                .Where(x =>
                    x.UserCategory!.TypeId == (int)CategoryType.Income)
                .Sum(x => x.Amount);

            var totalExpenses = transactions
                .Where(x =>
                    x.UserCategory!.TypeId == (int)CategoryType.Expense)
                .Sum(x => x.Amount);

            var balance = totalIncome - totalExpenses;

            var expenseRatio = totalIncome == 0
                ? 0
                : (totalExpenses / totalIncome) * 100;

            var monthlySummary =_monthlySummaryService.GetMonthlySummary(transactions,filter.Period,dateRangeResult.Data!);

            var expenseByCategory =_expenseByCategoryService.GetExpenseByCategory(transactions);

            var incomeSummary =_incomeSummaryService.GetIncomeSummary(transactions);

            return new ServiceResult<UserReportResponseDto>
            {
                Status = ServiceResultStatus.Success,
                Data = new UserReportResponseDto
                {
                    TotalIncome = totalIncome,
                    TotalExpense = totalExpenses,
                    Balance = balance,
                    ExpenseRatio = expenseRatio,
                    MonthlySummary = monthlySummary,
                    ExpenseByCategory = expenseByCategory,
                    IncomeSummary = incomeSummary
                }
            };
        }

        private string GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext?
                .User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedAccessException();
        }
    }
}
