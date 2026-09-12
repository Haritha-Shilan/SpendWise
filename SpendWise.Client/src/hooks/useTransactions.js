import { useEffect, useState } from "react";
import {
    getTransactions,
    getTransactionsByFilter,
    getTransactionFilterOptions,
    getTransactionById,
    createTransaction,
    updateTransaction,
    deleteTransaction,
} from "../services/transactionService";

export const useTransactions = () => {
    const [transactions, setTransactions] = useState([]);
    const [filterOptions, setFilterOptions] = useState({
        categories: [],
        paymentMethods: [],
    });

    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState("");

    const [isSaving, setIsSaving] = useState(false);
    const [saveError, setSaveError] = useState("");

    const [isDeleting, setIsDeleting] = useState(false);
    const [deleteError, setDeleteError] = useState("");

    const [appliedFilters, setAppliedFilters] = useState(null);

    const loadTransactions = async () => {
        try {
            setError("");

            const data = await getTransactions();
            setTransactions(data);
        } catch {
            setError("Failed to load transactions.");
        } finally {
            setIsLoading(false);
        }
    };

    const loadFilterOptions = async () => {
        try {
            const data = await getTransactionFilterOptions();
            setFilterOptions(data);
        } catch {
            setError("Failed to load transaction filter options.");
        }
    };

    const applyFilters = async (filters) => {
        try {
            setError("");
            setIsLoading(true);

            const data = await getTransactionsByFilter(filters);
            setTransactions(data);
            setAppliedFilters(filters);
        } catch {
            setError("Failed to load transactions.");
        } finally {
            setIsLoading(false);
        }
    };

    const reloadTransactions = async () => {
        if (appliedFilters) {
            const data = await getTransactionsByFilter(appliedFilters);
            setTransactions(data);
        } else {
            const data = await getTransactions();
            setTransactions(data);
        }
    };

    const getTransaction = async (id) => {
        return await getTransactionById(id);
    };

    const addTransaction = async (formData) => {
        try {
            setIsSaving(true);
            setSaveError("");

            await createTransaction(formData);

            await reloadTransactions();

            return true;
        } catch (error) {
            console.log(error);
            setSaveError("Failed to save transaction.");
            return false;
        } finally {
            setIsSaving(false);
        }
    };

    const editTransaction = async (id, formData) => {
        try {
            setIsSaving(true);
            setSaveError("");

            await updateTransaction(id, formData);
            await reloadTransactions();

            return true;
        } catch {
            setSaveError("Failed to update transaction.");
            return false;
        } finally {
            setIsSaving(false);
        }
    };

    const removeTransaction = async (id) => {
        try {
            setIsDeleting(true);
            setDeleteError("");

            await deleteTransaction(id);
            await reloadTransactions();

            return true;
        } catch {
            setDeleteError("Failed to delete transaction.");
            return false;
        } finally {
            setIsDeleting(false);
        }
    };

    const clearAppliedFilters = async () => {
        setAppliedFilters(null);

        try {
            setError("");
            setIsLoading(true);

            const data = await getTransactions();
            setTransactions(data);
        } catch {
            setError("Failed to load transactions.");
        } finally {
            setIsLoading(false);
        }
    };
    const clearSaveError = () => {
        setSaveError("");
    };

    const clearDeleteError = () => {
        setDeleteError("");
    };

    useEffect(() => {
        loadTransactions();
        loadFilterOptions();
    }, []);

    return {
        transactions,
        filterOptions,
        isLoading,
        error,
        isSaving,
        saveError,
        clearSaveError,
        isDeleting,
        deleteError,
        clearDeleteError,
        loadTransactions,
        applyFilters,
        clearAppliedFilters,
        getTransaction,
        addTransaction,
        editTransaction,
        removeTransaction,
    };
};