import api from "./api";

export const getTransactions = async () => {
    const response = await api.get("/transactions");
    return response.data;
};

export const getTransactionsByFilter = async (filters) => {
    const response = await api.get("/transactions/filter", {
        params: filters,
    });
    return response.data;
};

export const getTransactionFilterOptions = async () => {
    const response = await api.get("/transactions/filter-options");
    return response.data;
};

export const getTransactionById = async (id) => {
    const response = await api.get(`/transactions/${id}`);
    return response.data;
};

export const createTransaction = async (formData) => {
    const response = await api.post("/transactions", formData);
    return response.data;
};

export const updateTransaction = async (id, formData) => {
    await api.put(`/transactions/${id}`, formData);
};

export const deleteTransaction = async (id) => {
    await api.delete(`/transactions/${id}`);
};

export const getTransactionAttachment = async (id) => {
    const response = await api.get(
        `/transactions/${id}/attachment`,
        {
            responseType: "blob",
        }
    );

    return response;
};