import { categoryTypes } from "../../config/categoryTypes";
import "./TransactionFilter.css";
import Select from "react-select";

function TransactionFilter({
    filterOptions,
    filters,
    onFilterChange,
    onApply,
    onClear,
    onAdd,
}) {

    const expenseCategories = filterOptions.categories.filter(
        (category) => category.typeId === categoryTypes.expense.id
    );

    const incomeCategories = filterOptions.categories.filter(
        (category) => category.typeId === categoryTypes.income.id
    );

    const paymentMethodOptions = filterOptions.paymentMethods.map(
        (paymentMethod) => ({
            value: paymentMethod.id,
            label: paymentMethod.name,
        })
    );

    const categoryOptions = [];

    if (filters.categoryTypeId === "") {
        categoryOptions.push({
            label: "Expense",
            options: expenseCategories.map((category) => ({
                value: category.id,
                label: category.name,
            })),
        });

        categoryOptions.push({
            label: "Income",
            options: incomeCategories.map((category) => ({
                value: category.id,
                label: category.name,
            })),
        });
    } else {
        categoryOptions.push(
            ...filterOptions.categories
                .filter(
                    (category) =>
                        category.typeId === Number(filters.categoryTypeId)
                )
                .map((category) => ({
                    value: category.id,
                    label: category.name,
                }))
        );
    }

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

            <Select
                inputId="transaction-category-filter"
                options={categoryOptions}
                value={
                    categoryOptions
                        .flatMap((group) => group.options ?? [group])
                        .find(
                            (option) =>
                                option.value === Number(filters.userCategoryId)
                        ) || null
                }
                onChange={(option) =>
                    onFilterChange(
                        "userCategoryId",
                        option ? option.value : ""
                    )
                }
                placeholder="All Categories"
                isClearable
                isSearchable
                className="transaction-filter-category"
                classNames={{
                    control: (state) =>
                        state.isFocused
                            ? "transaction-select__control transaction-select__control--is-focused"
                            : "transaction-select__control",
                    valueContainer: () =>
                        "transaction-select__value-container",
                    placeholder: () =>
                        "transaction-select__placeholder",
                    singleValue: () =>
                        "transaction-select__single-value",
                    input: () =>
                        "transaction-select__input-container",
                    menu: () =>
                        "transaction-select__menu",
                    option: (state) =>
                        state.isFocused
                            ? "transaction-select__option transaction-select__option--is-focused"
                            : state.isSelected
                                ? "transaction-select__option transaction-select__option--is-selected"
                                : "transaction-select__option",
                }}
            />


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
            <Select
                inputId="transaction-payment-method-filter"
                options={paymentMethodOptions}
                value={
                    paymentMethodOptions.find(
                        (option) =>
                            option.value === Number(filters.paymentMethodId)
                    ) || null
                }
                onChange={(option) =>
                    onFilterChange(
                        "paymentMethodId",
                        option ? option.value : ""
                    )
                }
                placeholder="All Payment Methods"
                isClearable
                isSearchable
                className="transaction-filter-payment-method"
                classNames={{
                    control: (state) =>
                        state.isFocused
                            ? "transaction-select__control transaction-select__control--is-focused"
                            : "transaction-select__control",
                    valueContainer: () =>
                        "transaction-select__value-container",
                    placeholder: () =>
                        "transaction-select__placeholder",
                    singleValue: () =>
                        "transaction-select__single-value",
                    input: () =>
                        "transaction-select__input-container",
                    menu: () =>
                        "transaction-select__menu",
                    option: (state) =>
                        state.isFocused
                            ? "transaction-select__option transaction-select__option--is-focused"
                            : state.isSelected
                                ? "transaction-select__option transaction-select__option--is-selected"
                                : "transaction-select__option",
                    indicatorSeparator: () =>
                        null,
                    dropdownIndicator: () =>
                        "transaction-select__dropdown-indicator",
                }}
            />

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