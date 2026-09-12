import "./ReportSummary.css";

function ReportSummary({
    totalIncome = 0,
    totalExpense = 0,
    balance = 0,
    expenseRatio = 0,
    title = "Report Summary",
    subtitle = "Quick view of the selected period",
}) {
    return (
        <div className="report-summary">
            <div className="report-summary-header">
                <h3>{title}</h3>
                <p>{subtitle}</p>
            </div>

            <div className="report-summary-list">
                <div className="report-summary-row">
                    <span>Income</span>
                    <strong className="income">
                        ₹{totalIncome.toLocaleString("en-IN")}
                    </strong>
                </div>

                <div className="report-summary-row">
                    <span>Expenses</span>
                    <strong className="expense">
                        ₹{totalExpense.toLocaleString("en-IN")}
                    </strong>
                </div>

                <div className="report-summary-row">
                    <span>Balance</span>
                    <strong className="balance">
                        ₹{balance.toLocaleString("en-IN")}
                    </strong>
                </div>

                <div className="report-summary-row">
                    <span>Expense ratio</span>
                    <strong className="expense-ratio">
                        {expenseRatio.toFixed(2)}%
                    </strong>
                </div>
            </div>
        </div>
    );
}

export default ReportSummary;