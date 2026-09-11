import { useEffect, useState } from "react";
import {
    getUsers,
    setUserActiveStatus,
} from "../services/userManagementService";

export const useUserManagement = () => {
    const [users, setUsers] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState("");
    const [isUpdating, setIsUpdating] = useState(false);
    const [updateError, setUpdateError] = useState("");

    const loadUsers = async () => {
        try {
            setError("");

            const data = await getUsers();
            setUsers(data);
        } catch {
            setError("Failed to load users.");
        } finally {
            setIsLoading(false);
        }
    };

    const toggleUserStatus = async (id, isActive) => {
        try {
            setIsUpdating(true);
            setUpdateError("");

            await setUserActiveStatus(id, isActive);

            await loadUsers();

            return true;
        } catch {
            setUpdateError("Failed to update user status.");
            return false;
        } finally {
            setIsUpdating(false);
        }
    };

    useEffect(() => {
        loadUsers();
    }, []);

    return {
        users,
        isLoading,
        error,
        isUpdating,
        updateError,
        toggleUserStatus,
    };
};