import { FiEdit2, FiPaperclip, FiTrash2 } from "react-icons/fi";
import "./TransactionItem.css";

function TransactionItem({
    transaction,
    onEdit,
    onDelete,
    onAttachment,
}) {
    const isIncome = transaction.categoryTypeId === 2;

    return (
        <div className="transaction-item">

            <div className="transaction-item-date">
                {new Date(transaction.date).toLocaleDateString()}
            </div>

            <div
                className={
                    isIncome
                        ? "transaction-type income"
                        : "transaction-type expense"
                }
            >
                {transaction.categoryTypeName}
            </div>

            <div className="transaction-item-category">
                {transaction.userCategoryName}
            </div>

            <div
                className={
                    isIncome
                        ? "transaction-item-amount income"
                        : "transaction-item-amount expense"
                }
            >
                {isIncome ? "+" : "-"}
                {Number(transaction.amount).toFixed(2)}
            </div>

            <div className="transaction-item-payment">
                {transaction.paymentMethodName}
            </div>

            <div className="transaction-item-description">
                <span>{transaction.description || "—"}</span>

                {transaction.hasAttachment && (
                    <button
                        type="button"
                        className="transaction-attachment-button"
                        title={transaction.attachmentFileName}
                        onClick={() => onAttachment(transaction)}
                    >
                        <FiPaperclip />
                    </button>
                )}
            </div>

            <div className="transaction-item-actions">
                <button
                    type="button"
                    className="transaction-edit-button"
                    onClick={() => onEdit(transaction)}
                    title="Edit"
                >
                    <FiEdit2 />
                </button>

                <button
                    type="button"
                    className="transaction-delete-button"
                    onClick={() => onDelete(transaction)}
                    title="Delete"
                >
                    <FiTrash2 />
                </button>
            </div>

        </div>
    );
}

export default TransactionItem;