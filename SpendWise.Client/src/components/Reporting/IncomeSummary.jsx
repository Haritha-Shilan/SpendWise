import "./IncomeSummary.css";

function IncomeSummary({
    data = [],
    title = "Income Summary",
    subtitle = "Income sources for the selected period",
}) {
    return (
        <div className="income-summary">
            <div className="income-summary-header">
                <h3>{title}</h3>
                <p>{subtitle}</p>
            </div>

            {data.length === 0 ? (
                <p className="income-summary-empty">
                    No income data available.
                </p>
            ) : (
                <div className="income-summary-table">
                    <div className="income-summary-row income-summary-header-row">
                        <span>Income Category</span>
                        <span>Transactions</span>
                        <span>Amount</span>
                    </div>

                    {data.map((item) => (
                        <div
                            key={item.categoryName}
                            className="income-summary-row"
                        >
                            <span>{item.categoryName}</span>

                            <span>{item.transactionCount}</span>

                            <strong>
                                ₹{item.amount.toLocaleString("en-IN")}
                            </strong>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
}

export default IncomeSummary;