import { useState } from "react";
import { getUserDashboard } from "../services/userDashboardService";

export const useUserDashboard = () => {
    const [dashboard, setDashboard] = useState(null);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState("");

    const loadDashboard = async (filter) => {
        try {
            setIsLoading(true);
            setError("");

            const data = await getUserDashboard(filter);
            setDashboard(data);
        } catch {
            setError("Failed to load dashboard.");
        } finally {
            setIsLoading(false);
        }
    };

    return {
        dashboard,
        isLoading,
        error,
        loadDashboard,
    };
};