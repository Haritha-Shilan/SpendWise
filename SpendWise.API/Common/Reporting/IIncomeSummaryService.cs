namespace SpendWise.API.Common.Reporting
{
    public interface IIncomeSummaryService
    {
       public IEnumerable<IncomeSummaryDto> GetIncomeSummary( IEnumerable<TransactionEntity> transactions);
    }
}
