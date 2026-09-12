import api from "./api";

export const getUserDashboard = async (filter) => {
    const response = await api.get("/user/dashboard", {
        params: filter,
    });

    return response.data;
};