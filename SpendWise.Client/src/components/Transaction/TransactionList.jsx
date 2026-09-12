import TransactionItem from "./TransactionItem";
import "./TransactionList.css";

function TransactionList({
    transactions,
    onEdit,
    onDelete,
    onAttachment,
}) {
    return (
        <div className="transaction-list">

            <div className="transaction-list-header-row">
                <div>Date</div>
                <div>Type</div>
                <div>Category</div>
                <div>Amount</div>
                <div>Payment Method</div>
                <div>Description</div>
                <div>Actions</div>
            </div>

            {transactions.map((transaction) => (
                <TransactionItem
                    key={transaction.id}
                    transaction={transaction}
                    onEdit={onEdit}
                    onDelete={onDelete}
                    onAttachment={onAttachment}
                />
            ))}
        </div>
    );
}

export default TransactionList;