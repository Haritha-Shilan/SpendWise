import { categoryTypes } from "../../config/categoryTypes";
import "./TransactionFilter.css";

function TransactionFilter({
    filterOptions,
    filters,
    onFilterChange,
    onApply,
    onClear,
    onAdd,
}) {
    return (
        <div className="transaction-filter">

            <input
                type="text"
                placeholder="Search transactions..."
                value={filters.filterText}
                onChange={(e) =>
                    onFilterChange("filterText", e.target.value)
                }
            />

            <select
                value={filters.categoryTypeId}
                onChange={(e) =>
                    onFilterChange("categoryTypeId", e.target.value)
                }
            >
                <option value="">All Types</option>

                <option value={categoryTypes.expense.id}>
                    {categoryTypes.expense.name}
                </option>

                <option value={categoryTypes.income.id}>
                    {categoryTypes.income.name}
                </option>
            </select>

            <select
                value={filters.userCategoryId}
                onChange={(e) =>
                    onFilterChange("userCategoryId", e.target.value)
                }
            >
                <option value="">All Categories</option>

                {filterOptions.categories.map((category) => (
                    <option
                        key={category.id}
                        value={category.id}
                    >
                        {category.name}
                    </option>
                ))}
            </select>

            <input
                type="date"
                value={filters.fromDate}
                onChange={(e) =>
                    onFilterChange("fromDate", e.target.value)
                }
            />

            <input
                type="date"
                value={filters.toDate}
                onChange={(e) =>
                    onFilterChange("toDate", e.target.value)
                }
            />

            <input
                type="number"
                placeholder="Min amount"
                min="0"
                step="0.01"
                value={filters.minAmount}
                onChange={(e) =>
                    onFilterChange("minAmount", e.target.value)
                }
            />

            <input
                type="number"
                placeholder="Max amount"
                min="0"
                step="0.01"
                value={filters.maxAmount}
                onChange={(e) =>
                    onFilterChange("maxAmount", e.target.value)
                }
            />

            <select
                value={filters.paymentMethodId}
                onChange={(e) =>
                    onFilterChange("paymentMethodId", e.target.value)
                }
            >
                <option value="">All Payment Methods</option>

                {filterOptions.paymentMethods.map((paymentMethod) => (
                    <option
                        key={paymentMethod.id}
                        value={paymentMethod.id}
                    >
                        {paymentMethod.name}
                    </option>
                ))}
            </select>

            <div className="transaction-filter-actions">
                <button
                    type="button"
                    className="transaction-filter-clear"
                    onClick={onClear}
                >
                    Clear
                </button>

                <button
                    type="button"
                    className="transaction-filter-apply"
                    onClick={onApply}
                >
                    Apply Filters
                </button>

                <button
                    type="button"
                    className="transaction-filter-add"
                    onClick={onAdd}
                >
                    + Add Transaction
                </button>
            </div>
        </div>
    );
}

export default TransactionFilter;