import api from "./api";

export const getUsers = async () => {
    const response = await api.get("/admin/users");
    return response.data;
};

export const setUserActiveStatus = async (id, isActive) => {
    const action = isActive ? "activate" : "deactivate";

    await api.patch(`/admin/users/${id}/${action}`);
};