import { FiArrowUpRight, FiArrowDownLeft } from "react-icons/fi";
import './TransactionTypeIcon.css'

function TransactionTypeIcon({ type }) {
    const isIncome = type === "Income";

    return (
        <div className={`transaction-type-icon ${isIncome ? "income" : "expense"}`}>
            {isIncome ? <FiArrowUpRight /> : <FiArrowDownLeft />}
        </div>
    );
}

export default TransactionTypeIcon;