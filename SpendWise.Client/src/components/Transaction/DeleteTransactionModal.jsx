import "./DeleteTransactionModal.css";

function DeleteTransactionModal({
    transaction,
    onConfirm,
    onCancel,
    isDeleting,
    error
}) {
    if (!transaction) {
        return null;
    }

    return (
        <>
            <div
                className="modal fade show"
                style={{ display: "block" }}
                tabIndex="-1"
                role="dialog"
            >
                <div className="modal-dialog modal-dialog-centered">
                    <div className="modal-content delete-transaction-modal">

                        <div className="modal-header">
                            <h5 className="modal-title">
                                Delete Transaction
                            </h5>

                            <button
                                type="button"
                                className="btn-close"
                                aria-label="Close"
                                onClick={onCancel}
                                disabled={isDeleting}
                            />
                        </div>

                        <div className="modal-body">
                            {error && (
                                <p className="delete-transaction-error">
                                    {error}
                                </p>
                            )}
                            <p className="delete-transaction-message">
                                Are you sure you want to delete this
                                transaction?
                            </p>

                            <div className="delete-transaction-summary">
                                <div>
                                    <span>Category</span>
                                    <strong>
                                        {transaction.userCategoryName}
                                    </strong>
                                </div>

                                <div>
                                    <span>Amount</span>
                                    <strong>
                                        {Number(
                                            transaction.amount
                                        ).toFixed(2)}
                                    </strong>
                                </div>
                            </div>

                            <p className="delete-transaction-warning">
                                This action cannot be undone.
                            </p>
                        </div>

                        <div className="delete-transaction-actions">
                            <button
                                type="button"
                                className="delete-transaction-cancel"
                                onClick={onCancel}
                                disabled={isDeleting}
                            >
                                Cancel
                            </button>

                            <button
                                type="button"
                                className="delete-transaction-confirm"
                                onClick={onConfirm}
                                disabled={isDeleting}
                            >
                                {isDeleting
                                    ? "Deleting..."
                                    : "Delete"}
                            </button>
                        </div>

                    </div>
                </div>
            </div>

            <div className="modal-backdrop fade show" />
        </>
    );
}

export default DeleteTransactionModal;