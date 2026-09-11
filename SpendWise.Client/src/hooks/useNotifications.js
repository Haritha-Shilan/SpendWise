import { useEffect, useState } from "react";
import {
    getNotifications,
    markNotificationAsRead,
    markAllNotificationsAsRead
} from "../services/notificationService";

export const useNotifications = () => {
    const [notifications, setNotifications] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState("");
    const [isUpdating, setIsUpdating] = useState(false);



    useEffect(() => {
        const loadNotifications = async () => {
            try {
                setIsLoading(true);
                setError("");

                const data = await getNotifications();

                setNotifications(data);
            } catch {
                setError("Failed to load notifications.");
            } finally {
                setIsLoading(false);
            }
        };
        loadNotifications();
    }, []);

    const markAsRead = async (id) => {
        try {
            setIsUpdating(true);
            setError("");

            await markNotificationAsRead(id);

            setNotifications((prev) =>
                prev.filter((notification) => notification.id !== id)
            );

            return true;
        } catch (error) {
            if (error.response?.status === 404) {
                setError("Notification not found.");
            } else {
                setError("Failed to mark notification as read.");
            }

            return false;
        } finally {
            setIsUpdating(false);
        }
    };

    const markAllAsRead = async () => {
        try {
            setIsUpdating(true);
            setError("");

            await markAllNotificationsAsRead();

            setNotifications([]);

            return true;
        } catch {
            setError("Failed to mark all notifications as read.");
            return false;
        } finally {
            setIsUpdating(false);
        }
    };
    return {
        notifications,
        isLoading,
        error,
        isUpdating,
        markAsRead,
        markAllAsRead
    };
};