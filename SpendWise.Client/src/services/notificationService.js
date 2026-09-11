import api from "./api";

export const getNotifications = async () => {
    const response = await api.get("/notifications");

    return response.data;
};

export const markNotificationAsRead = async (id) => {
    await api.patch(`/notifications/${id}/read`);
};

export const markAllNotificationsAsRead = async () => {
    await api.patch("/notifications/read-all");
};