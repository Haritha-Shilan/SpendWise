import api from "./api";

export const getUserReports = async (filter) => {
    const response = await api.get("/user/reports", {
        params: filter,
    });

    return response.data;
};