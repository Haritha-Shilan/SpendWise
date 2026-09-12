import { useState } from "react";
import TransactionFilter from "../../components/Transaction/TransactionFilter";
import { useTransactions } from "../../hooks/useTransactions";
import TransactionList from "../../components/Transaction/TransactionList";
import "./TransactionsPage.css";
import TransactionForm from "../../components/Transaction/TransactionForm";
import { getTransactionAttachment } from "../../services/transactionService";
import DeleteTransactionModal from "../../components/Transaction/DeleteTransactionModal";

const initialFilters = {
    filterText: "",
    categoryTypeId: "",
    userCategoryId: "",
    fromDate: "",
    toDate: "",
    paymentMethodId: "",
    minAmount: "",
    maxAmount: "",
};

function TransactionsPage() {
    const {
        transactions,
        filterOptions,
        isLoading,
        error,
        clearSaveError,
        applyFilters,
        clearAppliedFilters,
        addTransaction,
        editTransaction,
        isSaving,
        saveError,
        removeTransaction,
        isDeleting,
        deleteError,
        clearDeleteError
    } = useTransactions();

    const [filters, setFilters] = useState(initialFilters);
    const [showForm, setShowForm] = useState(false);
    const [formMode, setFormMode] = useState("add");
    const [selectedTransaction, setSelectedTransaction] = useState(null);
    const [showDeleteModal, setShowDeleteModal] = useState(false);
    const [selectedTransactionForDelete, setSelectedTransactionForDelete] =
        useState(null);

    const handleDelete = (transaction) => {
        clearDeleteError();
        setSelectedTransactionForDelete(transaction);
        setShowDeleteModal(true);
    };

    const handleCloseDeleteModal = () => {
        clearDeleteError();
        setShowDeleteModal(false);
        setSelectedTransactionForDelete(null);
    };

    const handleConfirmDelete = async () => {
        if (!selectedTransactionForDelete) {
            return;
        }

        const success = await removeTransaction(
            selectedTransactionForDelete.id
        );

        if (success) {
            handleCloseDeleteModal();
        }
    };
    const handleAttachment = async (transaction) => {
        try {
            const response = await getTransactionAttachment(transaction.id);

            const blobUrl = window.URL.createObjectURL(response.data);

            window.open(blobUrl, "_blank");

            setTimeout(() => {
                window.URL.revokeObjectURL(blobUrl);
            }, 1000);
        } catch {
            // We'll add user-friendly error handling later.
        }
    };

    const handleFilterChange = (name, value) => {
        setFilters((current) => ({
            ...current,
            [name]: value,
        }));
    };

    const handleApply = () => {
        const requestFilters = {
            ...filters,
            categoryTypeId: filters.categoryTypeId || null,
            userCategoryId: filters.userCategoryId || null,
            fromDate: filters.fromDate || null,
            toDate: filters.toDate || null,
            paymentMethodId: filters.paymentMethodId || null,
            filterText: filters.filterText.trim() || null,
            minAmount: filters.minAmount || null,
            maxAmount: filters.maxAmount || null,
        };

        applyFilters(requestFilters);
    };

    const handleClear = async () => {
        setFilters(initialFilters);
        await clearAppliedFilters();
    };

    const handleAdd = () => {
        clearSaveError();
        setSelectedTransaction(null);
        setFormMode("add");
        setShowForm(true);
    };

    const handleEdit = (transaction) => {
        clearSaveError();
        setSelectedTransaction(transaction);
        setFormMode("edit");
        setShowForm(true);
    };

    const handleCloseForm = () => {
        clearSaveError();
        setShowForm(false);
        setSelectedTransaction(null);
    };

    const buildTransactionFormData = (formData) => {
        const data = new FormData();

        data.append("UserCategoryId", formData.userCategoryId);
        data.append("PaymentMethodId", formData.paymentMethodId);
        data.append("Amount", formData.amount);
        data.append("Date", formData.date);
        data.append("Description", formData.description || "");

        if (formData.attachment) {
            data.append("Attachment", formData.attachment);
        }

        if (formMode === "edit") {
            data.append(
                "RemoveAttachment",
                formData.removeAttachment ? "true" : "false"
            );
        }

        return data;
    };

    const handleSave = async (formData) => {
        const data = buildTransactionFormData(formData);

        let success;

        if (formMode === "edit") {
            success = await editTransaction(
                selectedTransaction.id,
                data
            );
        } else {
            success = await addTransaction(data);
        }

        if (success) {
            handleCloseForm();
        }
    };
    return (
        <div className="transactions-page">

            <div className="transactions-page-header">
                <div>
                    <h2>My Transactions</h2>
                    <p>Track and manage your income and expenses.</p>
                </div>
            </div>

            <TransactionFilter
                filterOptions={filterOptions}
                filters={filters}
                onFilterChange={handleFilterChange}
                onApply={handleApply}
                onClear={handleClear}
                onAdd={handleAdd}
            />

            <div className="transactions-list-card">
                <div className="transactions-list-header">
                    <div>
                        <h3>All Transactions</h3>
                        <p>{transactions.length} records</p>
                    </div>
                </div>

                {isLoading && (
                    <p>Loading transactions...</p>
                )}

                {!isLoading && error && (
                    <p className="transactions-error">
                        {error}
                    </p>
                )}

                {!isLoading && !error && transactions.length === 0 && (
                    <p className="transactions-empty">
                        No transactions found.
                    </p>
                )}

                {!isLoading && !error && transactions.length > 0 && (
                    <TransactionList
                        transactions={transactions}
                        onEdit={handleEdit}
                        onDelete={handleDelete}
                        onAttachment={handleAttachment}
                    />
                )}
            </div>
            {/* Add-Edit Modal */}
            {showForm && (
                <div
                    className="modal fade show"
                    style={{ display: "block" }}
                    tabIndex="-1"
                    role="dialog"
                >
                    <div className="modal-dialog modal-dialog-centered">
                        <div className="modal-content">

                            <div className="modal-header">
                                <h5 className="modal-title">
                                    {formMode === "edit"
                                        ? "Edit Transaction"
                                        : "Add Transaction"}
                                </h5>

                                <button
                                    type="button"
                                    className="btn-close"
                                    aria-label="Close"
                                    onClick={handleCloseForm}
                                />
                            </div>

                            <div className="modal-body">
                                {saveError && (
                                    <p className="transaction-form-error">
                                        {saveError}
                                    </p>
                                )}
                                <TransactionForm
                                    mode={formMode}
                                    transaction={selectedTransaction}
                                    filterOptions={filterOptions}
                                    onSave={handleSave}
                                    onCancel={handleCloseForm}
                                    isSaving={isSaving}
                                />
                            </div>

                        </div>
                    </div>
                </div>
            )}

            {/* Delete Modal */}
            {showDeleteModal && (
                <DeleteTransactionModal
                    transaction={selectedTransactionForDelete}
                    onConfirm={handleConfirmDelete}
                    onCancel={handleCloseDeleteModal}
                    isDeleting={isDeleting}
                    error={deleteError}
                />
            )}

            {showForm && <div className="modal-backdrop fade show" />}
        </div>
    );
}

export default TransactionsPage;