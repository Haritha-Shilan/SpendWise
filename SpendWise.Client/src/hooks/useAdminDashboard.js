import { useEffect, useState } from "react";
import { getAdminDashboard } from "../services/adminDashboardService";

export const useAdminDashboard = () => {
    const [dashboard, setDashboard] = useState(null);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState("");

    useEffect(() => {
        const loadDashboard = async () => {
            try {
                setIsLoading(true);
                setError("");

                const data = await getAdminDashboard();

                setDashboard(data);
            } catch {
                setError("Failed to load dashboard.");
            } finally {
                setIsLoading(false);
            }
        };

        loadDashboard();
    }, []);

    return {
        dashboard,
        isLoading,
        error
    };
};