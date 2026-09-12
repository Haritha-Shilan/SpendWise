import "./RecentTransactions.css";
import TransactionTypeIcon from "../Common/TransactionTypeIcon";

function RecentTransactions({
    transactions = [],
    title = "Recent Transactions",
    subtitle = "Your latest activity",
    onViewAll,
}) {
    return (
        <div className="recent-transactions">
            <div className="recent-transactions-header">
                <div>
                    <h3>{title}</h3>
                    <p>{subtitle}</p>
                </div>
                {onViewAll && (
                    <button
                        type="button"
                        className="recent-transactions-view-all"
                        onClick={onViewAll}
                    >
                        View all transactions →
                    </button>
                )}
            </div>

            {transactions.length === 0 ? (
                <p className="recent-transactions-empty">
                    No recent transactions.
                </p>
            ) : (
                <div className="recent-transactions-list">
                    {transactions.map((transaction) => {
                        const isIncome =
                            transaction.type === "Income";

                        return (
                            <div
                                key={transaction.id}
                                className="recent-transaction-item"
                            >
                                <TransactionTypeIcon type={transaction.type} />
                                <div className="recent-transaction-info">
                                    <p className="recent-transaction-category">
                                        {transaction.categoryName}
                                    </p>

                                    <p className="recent-transaction-description">
                                        {transaction.description || "—"}
                                    </p>

                                    <p className="recent-transaction-date">
                                        {new Date(
                                            transaction.date
                                        ).toLocaleDateString()}
                                    </p>
                                </div>

                                <div
                                    className={
                                        isIncome
                                            ? "recent-transaction-amount income"
                                            : "recent-transaction-amount expense"
                                    }
                                >
                                    {isIncome ? "+" : "-"}
                                    ₹
                                    {Number(
                                        transaction.amount
                                    ).toLocaleString("en-IN")}
                                </div>
                            </div>
                        );
                    })}
                </div>
            )}
        </div>
    );
}

export default RecentTransactions;