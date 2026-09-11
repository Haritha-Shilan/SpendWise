import { useEffect, useState } from "react"
import { getCategories, createCategory, updateCategory, activateCategory, deactivateCategory } from "../services/categoryService";

export const useCategories = (basePath) => {
    const [categories, setCategories] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState("");
    const [isSaving, setIsSaving] = useState(false);
    const [saveError, setSaveError] = useState("");

    useEffect(() => {
        const loadCategories = async () => {
            try {
                setIsLoading(true);
                setError("");

                const data = await getCategories(basePath);
                setCategories(data);
            }
            catch {
                setError("Failed to load categories.");
            }
            finally {
                setIsLoading(false);
            }
        };

        loadCategories();
    }, [basePath]);

    // Add
    const addCategory = async (categoryData) => {
        try {
            setIsSaving(true);
            setSaveError("");

            await createCategory(basePath, categoryData);

            const data = await getCategories(basePath);

            setCategories(data);

            return true;
        } catch (error) {
            if (error.response?.status === 409) {
                setSaveError(error.response.data);
            } else if (error.response?.status === 400) {
                setSaveError("Please check the entered category details.");
            } else {
                setSaveError("Failed to save category. Please try again.");
            }

            return false;
        } finally {
            setIsSaving(false);
        }
    };

    //Edit
    const editCategory = async (id, categoryData) => {
        try {
            setIsSaving(true);
            setSaveError("");

            await updateCategory(basePath, id, categoryData);

            const data = await getCategories(basePath);

            setCategories(data);

            return true;
        } catch (error) {
            if (error.response?.status === 409) {
                setSaveError(error.response.data);
            } else if (error.response?.status === 404) {
                setSaveError("Category not found.");
            } else if (error.response?.status === 400) {
                setSaveError("Please check the entered category details.");
            } else {
                setSaveError("Failed to update category. Please try again.");
            }

            return false;
        } finally {
            setIsSaving(false);
        }
    };

    //Activate/Deactivate
    const toggleCategoryState = async (category) => {
        try {
            setIsSaving(true);
            setSaveError("");

            if (category.isActive) {
                await deactivateCategory(basePath, category.id);
            } else {
                await activateCategory(basePath, category.id);
            }

            const data = await getCategories(basePath);

            setCategories(data);

            return true;
        } catch (error) {
            if (error.response?.status === 404) {
                setSaveError("Category not found.");
            } else if (error.response?.status === 400) {
                setSaveError("Unable to change category status.");
            } else {
                setSaveError("Failed to update category status. Please try again.");
            }

            return false;
        } finally {
            setIsSaving(false);
        }
    };
    return {
        categories,
        isLoading,
        error,
        addCategory,
        isSaving,
        saveError,
        editCategory,
        toggleCategoryState
    };
}