import './CategoryItem.css'
import TransactionTypeIcon from "../Common/TransactionTypeIcon";
function CategoryItem({ category, onEdit, onToggleState }) {
    const actionLabel = category.isActive
        ? "Deactivate"
        : "Activate";

    console.log(category);
    return (
        <div className="category-item">
            <div className="category-item-info">
                <div className="category-item-icon">
                    {/* <CategoryIcon /> */}
                    <TransactionTypeIcon type={category.typeName} />
                </div>

                <div>
                    <div className="category-item-name">
                        {category.name}
                    </div>

                    <div
                        className={
                            category.isActive
                                ? "category-item-status active"
                                : "category-item-status inactive"
                        }
                    >
                        {category.isActive ? "Active" : "Inactive"}
                    </div>
                </div>
            </div>

            <div className="category-item-actions">
                <button
                    type="button"
                    className="category-action-button category-edit-button"
                    onClick={() => onEdit(category)}
                >
                    Edit
                </button>

                <button
                    type="button"
                    className={`category-action-button ${category.isActive
                        ? "category-deactivate-button"
                        : "category-activate-button"
                        }`}
                    onClick={() => onToggleState(category)}
                >
                    {actionLabel}
                </button>
            </div>
        </div>
    );
}

export default CategoryItem;