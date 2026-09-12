import { useEffect, useState } from "react";
import ReportFilter from "../../components/ReportFilter/ReportFilter";
import { useUserDashboard } from "../../hooks/useUserDashboard";
import "./UserDashboardPage.css";
import {
    FiArrowDownLeft,
    FiArrowUpRight,
} from "react-icons/fi";
import { FaRupeeSign } from "react-icons/fa";
import ExpenseByCategory from "../../components/Reporting/ExpenseByCategory";
import { useNavigate } from "react-router-dom";
import SummaryCard from "../../components/Dashboard/SummaryCard";
import MonthlySummary from "../../components/Reporting/MonthlySummary";
import RecentTransactions from "../../components/Transaction/RecentTransactions"

function UserDashboardPage() {
    const {
        dashboard,
        isLoading,
        error,
        loadDashboard,
    } = useUserDashboard();

    const initialFilter = {
        period: 1,
        fromDate: null,
        toDate: null,
    };

    const navigate = useNavigate();

    const [selectedPeriodLabel, setSelectedPeriodLabel] = useState("This Month");

    useEffect(() => {
        loadDashboard(initialFilter);
    }, []);

    const handleFilterApply = (filter) => {
        setSelectedPeriodLabel(filter.periodLabel);
        loadDashboard(filter);
    };

    return (
        <div className="user-dashboard-page">

            <div className="user-dashboard-header">
                <div>
                    <h2>My Dashboard</h2>
                    <p>Your personal financial overview.</p>
                </div>
            </div>

            <ReportFilter
                initialPeriod={1}
                onApply={handleFilterApply}
            />

            {isLoading && (
                <p>Loading dashboard...</p>
            )}

            {!isLoading && error && (
                <p className="user-dashboard-error">
                    {error}
                </p>
            )}

            {!isLoading && !error && dashboard && (
                <div>
                    <div className="user-dashboard-summary-grid">
                        <SummaryCard
                            title="Balance"
                            value={dashboard.balance.toFixed(2)}
                            description="Income minus expenses"
                            icon={FaRupeeSign}
                            variant="balance"
                        />

                        <SummaryCard
                            title="Income"
                            value={dashboard.totalIncome.toFixed(2)}
                            description={selectedPeriodLabel}
                            icon={FiArrowDownLeft}
                            variant="income"
                        />

                        <SummaryCard
                            title="Expenses"
                            value={dashboard.totalExpense.toFixed(2)}
                            description={selectedPeriodLabel}
                            icon={FiArrowUpRight}
                            variant="expense"
                        />
                    </div>

                    <div className="user-dashboard-chart-grid">
                        <MonthlySummary
                            data={dashboard.monthlySummary}
                        />

                        <ExpenseByCategory
                            data={dashboard.expenseByCategory}
                            variant="bar"
                            maxItems={5}
                            showViewAll={true}
                            onViewAll={() => navigate("/user/reports")}
                        />
                    </div>

                    <div className="user-dashboard-recent ">
                        <RecentTransactions
                            transactions={dashboard.recentTransactions}
                            onViewAll={() => navigate("/user/transactions")}
                        />
                    </div>
                </div>
            )}

        </div>
    );
}

export default UserDashboardPage;