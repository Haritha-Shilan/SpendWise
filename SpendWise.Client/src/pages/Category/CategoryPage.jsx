import { useState } from "react";
import { categoryTypes } from "../../config/categoryTypes";
import { useCategories } from "../../hooks/useCategories";
import CategoryItem from "../../components/category/CategoryItem"
import CategoryForm from "../../components/category/CategoryForm"
import "./CategoryPage.css";

function CategoryPage({ basePath }) {
    const {
        categories,
        isLoading,
        error,
        addCategory,
        isSaving,
        saveError,
        editCategory,
        toggleCategoryState
    } = useCategories(basePath);

    const [selectedTypeId, setSelectedTypeId] = useState(
        categoryTypes.expense.id
    );

    const [isFormOpen, setIsFormOpen] = useState(false);
    const [selectedCategory, setSelectedCategory] = useState(null);

    const isExpenseSelected =
        selectedTypeId === categoryTypes.expense.id;

    const sectionTitle = isExpenseSelected
        ? "Expense Categories"
        : "Income Categories";

    const sectionDescription = isExpenseSelected
        ? "Categories used when recording money spent"
        : "Categories used when recording money received";

    const filteredCategories = categories.filter(
        (category) => category.typeId === selectedTypeId
    );

    const handleAddCategory = () => {
        setSelectedCategory(null);
        setIsFormOpen(true);
    };


    const handleEditCategory = (category) => {
        setSelectedCategory(category);
        setIsFormOpen(true);
    };

    const handleCancel = () => {
        setIsFormOpen(false);
        setSelectedCategory(null);
    };

    const handleSaveCategory = async (formData) => {
        const categoryData = {
            ...formData,
            typeId: Number(formData.typeId)
        };

        let success;

        if (selectedCategory) {
            success = await editCategory(
                selectedCategory.id,
                categoryData
            );
        } else {
            success = await addCategory(categoryData);
        }

        if (success) {
            handleCancel();
        }
    };

    return (
        <div className="category-page">

            {/* Page header */}
            <div className="category-page-header">
                <div>
                    <h2>My Categories</h2>
                    <p>Manage your income and expense categories.</p>
                </div>

                <button
                    type="button"
                    className="category-add-button"
                    onClick={handleAddCategory}
                >
                    + Add Category
                </button>
            </div>

            {/* Category container */}
            <div className="category-card">

                {/* Category tabs */}
                <div className="category-tabs">

                    <button
                        type="button"
                        className={
                            isExpenseSelected ? "active" : ""
                        }
                        onClick={() =>
                            setSelectedTypeId(categoryTypes.expense.id)
                        }
                    >
                        {categoryTypes.expense.name}
                    </button>

                    <button
                        type="button"
                        className={
                            !isExpenseSelected ? "active" : ""
                        }
                        onClick={() =>
                            setSelectedTypeId(categoryTypes.income.id)
                        }
                    >
                        {categoryTypes.income.name}
                    </button>

                </div>

                {/* Category content */}
                <div className="category-content">

                    <h3>{sectionTitle}</h3>

                    <p className="category-content-description">
                        {sectionDescription}
                    </p>

                    <div className="category-list">
                        {isLoading && <p>Loading categories...</p>}

                        {error && (
                            <p className="category-error">
                                {error}
                            </p>
                        )}

                        {!isLoading && !error && filteredCategories.length === 0 && (
                            <p>No categories found.</p>
                        )}

                        {!isLoading && !error && filteredCategories.length > 0 && (
                            filteredCategories.map((category) => (
                                <CategoryItem
                                    key={category.id}
                                    category={category}
                                    onEdit={handleEditCategory}
                                    onToggleState={toggleCategoryState}
                                />
                            ))
                        )}
                    </div>

                </div>

            </div>

            {isFormOpen && (
                <CategoryForm
                    mode={selectedCategory ? "edit" : "add"}
                    category={selectedCategory}
                    onCancel={handleCancel}
                    onSubmit={handleSaveCategory}
                    isSaving={isSaving}
                    saveError={saveError}
                    defaultTypeId={selectedTypeId}
                />
            )}

        </div>
    );
}

export default CategoryPage;