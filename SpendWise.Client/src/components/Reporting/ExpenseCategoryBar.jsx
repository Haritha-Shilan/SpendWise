import "./ExpenseCategoryBar.css";

function ExpenseCategoryBar({
    data = [],
    maxItems,
    showViewAll = false,
    onViewAll,
}) {
    const sortedData = [...data].sort(
        (a, b) => b.amount - a.amount
    );

    const displayData = maxItems
        ? sortedData.slice(0, maxItems)
        : sortedData;

    const maxAmount =
        displayData.length > 0
            ? Math.max(...displayData.map((item) => item.amount))
            : 0;

    return (
        <div className="expense-category-bar-list">
            {displayData.map((item) => {
                const barWidth =
                    maxAmount > 0
                        ? (item.amount / maxAmount) * 100
                        : 0;

                return (
                    <div
                        key={item.categoryName}
                        className="expense-category-bar-item"
                    >
                        <div className="expense-category-bar-label">
                            <span>{item.categoryName}</span>

                            <strong>
                                ₹{item.amount.toLocaleString("en-IN")}
                            </strong>
                        </div>

                        <div className="expense-category-bar-track">
                            <div
                                className="expense-category-bar-fill"
                                style={{
                                    width: `${barWidth}%`,
                                }}
                            />
                        </div>
                    </div>
                );
            })}

            {showViewAll &&
                maxItems &&
                data.length > maxItems && (
                    <button
                        type="button"
                        className="expense-category-bar-view-all"
                        onClick={onViewAll}
                    >
                        View all categories →
                    </button>
                )}
        </div>
    );
}

export default ExpenseCategoryBar;