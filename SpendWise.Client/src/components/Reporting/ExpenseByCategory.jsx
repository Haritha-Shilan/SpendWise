import ExpenseCategoryBar from "./ExpenseCategoryBar";
import ExpenseCategoryPie from "./ExpenseCategoryPie";
import "./ExpenseByCategory.css";

function ExpenseByCategory({
    data = [],
    title = "Expenses by Category",
    subtitle = "This month",
    variant = "bar",
    maxItems,
    showViewAll = false,
    onViewAll,
}) {
    return (
        <div className="expense-by-category">
            <div className="expense-by-category-header">
                <div>
                    <h3>{title}</h3>
                    <p>{subtitle}</p>
                </div>

                {showViewAll && onViewAll && (
                    <button
                        type="button"
                        className="expense-by-category-view-all"
                        onClick={onViewAll}
                    >
                        View all categories →
                    </button>
                )}
            </div>

            {data.length === 0 ? (
                <p className="expense-by-category-empty">
                    No expense data available.
                </p>
            ) : variant === "pie" ? (
                <ExpenseCategoryPie data={data} />
            ) : (
                <ExpenseCategoryBar
                    data={data}
                    maxItems={maxItems}
                />
            )}
        </div>
    );
}

export default ExpenseByCategory;