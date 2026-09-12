import { useState } from "react";
import { getUserReports } from "../services/userReportService";

function useUserReports() {
    const [report, setReport] = useState(null);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState("");

    const loadReport = async (filter) => {
        try {
            setIsLoading(true);
            setError("");

            const data = await getUserReports(filter);
            setReport(data);
        } catch {
            setError("Failed to load reports.");
        } finally {
            setIsLoading(false);
        }
    };

    return {
        report,
        isLoading,
        error,
        loadReport,
    };
}

export default useUserReports;