import { useEffect, useState } from "react";
import { categoryTypes } from "../../config/categoryTypes";
import "./TransactionForm.css";
import Select from "react-select";

const initialFormData = {
    categoryTypeId: "",
    userCategoryId: "",
    paymentMethodId: "",
    amount: "",
    date: "",
    description: "",
    attachment: null,
    removeAttachment: false,
};

function TransactionForm({
    mode,
    transaction,
    filterOptions,
    onSave,
    onCancel,
    isSaving,
}) {
    const [formData, setFormData] = useState(initialFormData);
    const isEditMode = mode === "edit";

    useEffect(() => {
        if (isEditMode && transaction) {
            setFormData({
                categoryTypeId: transaction.categoryTypeId,
                userCategoryId: transaction.userCategoryId,
                paymentMethodId: transaction.paymentMethodId,
                amount: transaction.amount,
                date: transaction.date
                    ? transaction.date.split("T")[0]
                    : "",
                description: transaction.description || "",
                attachment: null,
                removeAttachment: false,
            });
        } else {
            setFormData(initialFormData);
        }
    }, [isEditMode, transaction]);

    const activeCategories = filterOptions.categories.filter(
        (category) => category.isActive
    );

    const categoryOptions = activeCategories
        .filter(
            (category) =>
                category.typeId === Number(formData.categoryTypeId)
        )
        .map((category) => ({
            value: category.id,
            label: category.name,
        }));

    const activePaymentMethods = filterOptions.paymentMethods.filter(
        (paymentMethod) => paymentMethod.isActive
    );

    const paymentMethodOptions = activePaymentMethods.map(
        (paymentMethod) => ({
            value: paymentMethod.id,
            label: paymentMethod.name,
        })
    );

    const handleChange = (event) => {
        const { name, value } = event.target;

        setFormData((current) => ({
            ...current,
            [name]: value,
        }));
    };

    const handleFileChange = (event) => {
        const file = event.target.files[0] || null;

        setFormData((current) => ({
            ...current,
            attachment: file,
            removeAttachment: false,
        }));
    };

    const handleRemoveAttachment = () => {
        setFormData((current) => ({
            ...current,
            attachment: null,
            removeAttachment: true,
        }));
    };

    const handleSubmit = (event) => {
        event.preventDefault();

        onSave(formData);
    };

    return (
        <form
            className="transaction-form"
            onSubmit={handleSubmit}
        >

            <div className="transaction-form-field">
                <label htmlFor="categoryTypeId">
                    Category Type
                </label>

                <select
                    id="categoryTypeId"
                    name="categoryTypeId"
                    value={formData.categoryTypeId}
                    onChange={(event) => {
                        setFormData((current) => ({
                            ...current,
                            categoryTypeId: event.target.value,
                            userCategoryId: "",
                        }));
                    }}
                    required
                >
                    <option value="">
                        Select type
                    </option>

                    <option value={categoryTypes.expense.id}>
                        {categoryTypes.expense.name}
                    </option>

                    <option value={categoryTypes.income.id}>
                        {categoryTypes.income.name}
                    </option>
                </select>
            </div>

            <div className="transaction-form-field">
                <label htmlFor="userCategoryId">
                    Category
                </label>

                <Select
                    inputId="userCategoryId"
                    options={categoryOptions}
                    value={
                        categoryOptions.find(
                            (option) =>
                                option.value === Number(formData.userCategoryId)
                        ) || null
                    }
                    onChange={(option) => {
                        setFormData((current) => ({
                            ...current,
                            userCategoryId: option ? option.value : "",
                        }));
                    }}
                    placeholder={
                        formData.categoryTypeId
                            ? "Select category"
                            : "Select category type first"
                    }
                    isDisabled={!formData.categoryTypeId}
                    isClearable
                    isSearchable
                    required
                    classNames={{
                        control: (state) =>
                            state.isFocused
                                ? "transaction-select__control transaction-select__control--is-focused"
                                : "transaction-select__control",
                        valueContainer: () => "transaction-select__value-container",
                        placeholder: () => "transaction-select__placeholder",
                        singleValue: () => "transaction-select__single-value",
                        input: () => "transaction-select__input-container",
                        menu: () => "transaction-select__menu",
                        option: (state) =>
                            state.isFocused
                                ? "transaction-select__option transaction-select__option--is-focused"
                                : state.isSelected
                                    ? "transaction-select__option transaction-select__option--is-selected"
                                    : "transaction-select__option",
                        indicatorSeparator: () =>
                            "transaction-select__indicator-separator",
                        dropdownIndicator: () =>
                            "transaction-select__dropdown-indicator",
                    }}
                />
            </div>

            <div className="transaction-form-field">
                <label htmlFor="paymentMethodId">
                    Payment Method
                </label>

                <Select
                    inputId="paymentMethodId"
                    options={paymentMethodOptions}
                    value={
                        paymentMethodOptions.find(
                            (option) =>
                                option.value === Number(formData.paymentMethodId)
                        ) || null
                    }
                    onChange={(option) => {
                        setFormData((current) => ({
                            ...current,
                            paymentMethodId: option ? option.value : "",
                        }));
                    }}
                    placeholder="Select payment method"
                    isClearable
                    isSearchable
                    required
                    classNames={{
                        control: (state) =>
                            state.isFocused
                                ? "transaction-select__control transaction-select__control--is-focused"
                                : "transaction-select__control",
                        valueContainer: () => "transaction-select__value-container",
                        placeholder: () => "transaction-select__placeholder",
                        singleValue: () => "transaction-select__single-value",
                        input: () => "transaction-select__input-container",
                        menu: () => "transaction-select__menu",
                        option: (state) =>
                            state.isFocused
                                ? "transaction-select__option transaction-select__option--is-focused"
                                : state.isSelected
                                    ? "transaction-select__option transaction-select__option--is-selected"
                                    : "transaction-select__option",
                        indicatorSeparator: () =>
                            "transaction-select__indicator-separator",
                        dropdownIndicator: () =>
                            "transaction-select__dropdown-indicator",
                    }}
                />
            </div>

            <div className="transaction-form-row">
                <div className="transaction-form-field">
                    <label htmlFor="amount">
                        Amount
                    </label>

                    <input
                        id="amount"
                        name="amount"
                        type="number"
                        min="0.01"
                        step="0.01"
                        value={formData.amount}
                        onChange={handleChange}
                        required
                    />
                </div>

                <div className="transaction-form-field">
                    <label htmlFor="date">
                        Date
                    </label>

                    <input
                        id="date"
                        name="date"
                        type="date"
                        value={formData.date}
                        onChange={handleChange}
                        required
                    />
                </div>
            </div>

            <div className="transaction-form-field">
                <label htmlFor="description">
                    Description
                </label>

                <textarea
                    id="description"
                    name="description"
                    maxLength="500"
                    rows="3"
                    value={formData.description}
                    onChange={handleChange}
                    required
                />
            </div>

            <div className="transaction-form-field">
                <label htmlFor="attachment">
                    Attachment
                </label>

                {isEditMode &&
                    transaction?.hasAttachment &&
                    !formData.removeAttachment && (
                        <div className="transaction-existing-attachment">
                            <span>
                                {transaction.attachmentFileName}
                            </span>

                            <button
                                type="button"
                                onClick={handleRemoveAttachment}
                            >
                                Remove
                            </button>
                        </div>
                    )}

                <input
                    id="attachment"
                    name="attachment"
                    type="file"
                    onChange={handleFileChange}
                />

                {formData.attachment && (
                    <p className="transaction-selected-file">
                        Selected: {formData.attachment.name}
                    </p>
                )}
            </div>

            <div className="transaction-form-actions">
                <button
                    type="button"
                    className="transaction-form-cancel"
                    onClick={onCancel}
                >
                    Cancel
                </button>

                <button
                    type="submit"
                    className="transaction-form-save"
                    disabled={isSaving}
                >
                    {isSaving
                        ? "Saving..."
                        : isEditMode
                            ? "Update Transaction"
                            : "Save Transaction"}
                </button>
            </div>
        </form>
    );
}

export default TransactionForm;