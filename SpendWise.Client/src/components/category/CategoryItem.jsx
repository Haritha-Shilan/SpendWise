import { FiArrowDownRight, FiArrowUpRight } from "react-icons/fi";
import './CategoryItem.css'

function CategoryItem({ category, onEdit, onToggleState }) {
    const actionLabel = category.isActive
        ? "Deactivate"
        : "Activate";

    const CategoryIcon =
        category.typeId === 1
            ? FiArrowDownRight
            : FiArrowUpRight;

    return (
        <div className="category-item">
            <div className="category-item-info">
                <div className="category-item-icon">
                    <CategoryIcon />
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
                    onClick={() => onEdit(category)}
                >
                    Edit
                </button>

                <button
                    type="button"
                    onClick={() => onToggleState(category)}
                >
                    {actionLabel}
                </button>
            </div>
        </div>
    );
}

export default CategoryItem;