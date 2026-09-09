namespace SpendWise.API.Features.UserDashboard
{
    public class UserDashboardService : IUserDashboardService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITransactionQueryService _transactionQueryService;
        private readonly IMonthlySummaryService _monthlySummaryService;
        private readonly IDateRangeService _dateRangeService;
        private readonly IExpenseByCategoryService _expenseByCategoryService;
        private readonly IMapper _mapper;
        private readonly string _userId;      
        public UserDashboardService(ITransactionQueryService transactionQueryService,
                                    IDateRangeService dateRangeService,
                                    IHttpContextAccessor httpContextAccessor,
                                    IMapper mapper,
                                    IMonthlySummaryService monthlySummaryService,
                                    IExpenseByCategoryService expenseByCategoryService)
        {
            _httpContextAccessor = httpContextAccessor;
            _transactionQueryService = transactionQueryService;
            _dateRangeService = dateRangeService;
            _userId=GetCurrentUserId();
            _mapper = mapper;        
            _monthlySummaryService = monthlySummaryService;
            _expenseByCategoryService = expenseByCategoryService;
        }
        public async Task<ServiceResult<DashboardResponseDto>> GetDashboard(ReportFilterDto filter)
        {
            var dateRangeResult = _dateRangeService.GetDateRange(filter.Period, filter.FromDate, filter.ToDate);
            if(dateRangeResult.Status!=ServiceResultStatus.Success)
            {
                return new ServiceResult<DashboardResponseDto>
                {
                    Status = dateRangeResult.Status,
                    Error = dateRangeResult.Error
                };
            }

            var transactions = await _transactionQueryService.GetUserTransactionsAsync(_userId,dateRangeResult.Data!);
            
            var totalIncome = transactions
                .Where(x => x.UserCategory!.TypeId == (int)CategoryType.Income)
                .Sum(x => x.Amount);

            var totalExpenses = transactions
                .Where(x => x.UserCategory!.TypeId == (int)CategoryType.Expense)
                .Sum(x => x.Amount);

            var balance = totalIncome - totalExpenses;

            var monthlySummary =_monthlySummaryService.GetMonthlySummary(transactions, filter.Period, dateRangeResult.Data!);
            var expenseByCategory =_expenseByCategoryService.GetExpenseByCategory(transactions);
            var recentTransactions = transactions.Take(5).ToList();

            return new ServiceResult<DashboardResponseDto>
            {
                Status = ServiceResultStatus.Success,
                Data = new DashboardResponseDto
                {
                    TotalIncome = totalIncome,
                    TotalExpense = totalExpenses,
                    Balance = balance,
                    ExpenseByCategory = expenseByCategory,
                    MonthlySummary = monthlySummary,
                    RecentTransactions = _mapper.Map<IEnumerable<RecentTransactionDto>>(recentTransactions)
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
