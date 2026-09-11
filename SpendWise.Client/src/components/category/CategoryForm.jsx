import "./CategoryForm.css";
import { categoryTypes } from "../../config/categoryTypes";
import { useState } from "react";

function CategoryForm({ mode, category, onCancel, onSubmit, isSaving, saveError, defaultTypeId }) {
    const isEditMode = mode === "edit";

    const [formData, setFormData] = useState({
        name: category?.name ?? "",
        typeId: category?.typeId ?? defaultTypeId
    });

    const handleChange = (e) => {
        const { name, value } = e.target;

        setFormData((prev) => ({
            ...prev,
            [name]: value
        }));
    };

    const handleSubmit = (e) => {
        e.preventDefault();

        onSubmit(formData);
    };

    return (
        <div className="modal d-block" tabIndex="-1" role="dialog">
            <div className="modal-dialog modal-dialog-centered">
                <div className="modal-content category-form">

                    <div className="modal-header">
                        <div>
                            <h3 className="modal-title">
                                {isEditMode
                                    ? "Edit User Category"
                                    : "Add User Category"}
                            </h3>

                            <p className="category-form-subtitle">
                                {isEditMode
                                    ? "Update your personal category."
                                    : "Create a personal category for your account."}
                            </p>
                        </div>
                    </div>

                    <div className="modal-body">
                        <form onSubmit={handleSubmit}>
                            <div className="category-form-group">
                                <label htmlFor="categoryName">
                                    Category Name
                                </label>

                                <input
                                    type="text"
                                    id="categoryName"
                                    name="name"
                                    placeholder="e.g. Entertainment"
                                    value={formData.name}
                                    onChange={handleChange}
                                    required
                                />
                            </div>

                            <div className="category-form-group">
                                <label htmlFor="categoryType">
                                    Type
                                </label>

                                <select
                                    id="categoryType"
                                    name="typeId"
                                    value={formData.typeId}
                                    onChange={handleChange}
                                    required
                                >
                                    <option value="" disabled>
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

                            {saveError && (
                                <div className="auth-error">
                                    {saveError}
                                </div>
                            )}

                            <div className="category-form-actions">
                                <button
                                    type="button"
                                    className="category-cancel-button"
                                    onClick={onCancel}
                                >
                                    Cancel
                                </button>

                                <button
                                    type="submit"
                                    className="category-save-button"
                                    disabled={isSaving}
                                >
                                    {isSaving
                                        ? "Saving..."
                                        : isEditMode
                                            ? "Save Changes"
                                            : "Save Category"}
                                </button>
                            </div>
                        </form>
                    </div>

                </div>
            </div>
        </div>
    );
}

export default CategoryForm;