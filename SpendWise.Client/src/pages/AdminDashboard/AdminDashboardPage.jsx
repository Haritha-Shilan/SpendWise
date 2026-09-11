import {
    FiFolder,
    FiUser,
    FiUserCheck,
    FiUserX,
    FiUserPlus
} from "react-icons/fi";
import { useNavigate } from "react-router-dom";
import { FiBell } from "react-icons/fi";
import SummaryCard from "../../components/Dashboard/SummaryCard";
import { useAdminDashboard } from "../../hooks/useAdminDashboard"
import "./AdminDashboardPage.css";
import { getInitials } from "../../utils/getInitials";

function AdminDashboardPage() {
    const navigate = useNavigate();
    const {
        dashboard,
        isLoading,
        error
    } = useAdminDashboard();

    if (isLoading) {
        return (
            <div className="admin-dashboard-page">
                <p>Loading dashboard...</p>
            </div>
        );
    }

    if (error) {
        return (
            <div className="admin-dashboard-page">
                <p className="admin-dashboard-error">
                    {error}
                </p>
            </div>
        );
    }

    return (
        <div className="admin-dashboard-page">

            <div className="admin-dashboard-header">
                <div>
                    <h2>Admin Dashboard</h2>
                    <p>
                        Overview of users, categories and notifications.
                    </p>
                </div>
            </div>

            {dashboard.unreadNotificationCount > 0 && (
                <div className="admin-dashboard-notification-card">
                    <div className="admin-dashboard-notification-icon">
                        <FiBell />
                    </div>

                    <div className="admin-dashboard-notification-content">
                        <h3>Notifications</h3>

                        <p>
                            {dashboard.unreadNotificationCount} unread{" "}
                            {dashboard.unreadNotificationCount === 1
                                ? "notification"
                                : "notifications"}
                        </p>
                    </div>

                    <button
                        type="button"
                        className="admin-dashboard-notification-link"
                        onClick={() => navigate("/admin/notifications")}
                    >
                        View all notifications →
                    </button>
                </div>
            )}

            <div className="admin-dashboard-summary-grid">

                <SummaryCard
                    title="Total Users"
                    value={dashboard.totalUsers}
                    description="All registered users"
                    icon={FiUser}
                />

                <SummaryCard
                    title="Active Users"
                    value={dashboard.activeUsers}
                    description="Currently active users"
                    icon={FiUserCheck}
                />

                <SummaryCard
                    title="Inactive Users"
                    value={dashboard.inactiveUsers}
                    description="Deactivated users"
                    icon={FiUserX}
                />

                <SummaryCard
                    title="New This Month"
                    value={dashboard.newThisMonth}
                    description="New registrations"
                    icon={FiUserPlus}
                />

                <SummaryCard
                    title="Active Categories"
                    value={dashboard.activeCategories}
                    description="Across master categories"
                    icon={FiFolder}
                />

            </div>


            <div className="admin-dashboard-registrations-card">
                <div className="admin-dashboard-section-header">
                    <div>
                        <h3>Recent Registrations</h3>
                        <p>Latest users who joined SpendWise</p>
                    </div>
                </div>

                <div className="admin-dashboard-registrations-list">
                    {dashboard.recentRegistrations.length === 0 ? (
                        <p className="admin-dashboard-empty">
                            No recent registrations.
                        </p>
                    ) : (
                        dashboard.recentRegistrations.map((user) => (
                            <div
                                key={user.email}
                                className="admin-dashboard-registration-item"
                            >
                                <div className="admin-dashboard-registration-user">
                                    <div className="admin-dashboard-registration-avatar">
                                        {getInitials(user.fullName)}
                                    </div>

                                    <div>
                                        <p className="admin-dashboard-registration-name">
                                            {user.fullName}
                                        </p>

                                        <p className="admin-dashboard-registration-email">
                                            {user.email}
                                        </p>
                                    </div>
                                </div>

                                <div className="admin-dashboard-registration-meta">
                                    <span>
                                        {new Date(
                                            user.createdAt
                                        ).toLocaleDateString()}
                                    </span>

                                    <span
                                        className={
                                            user.isActive
                                                ? "status-badge active"
                                                : "status-badge inactive"
                                        }
                                    >
                                        {user.isActive
                                            ? "Active"
                                            : "Inactive"}
                                    </span>
                                </div>
                            </div>
                        ))
                    )}
                </div>
            </div>

        </div>
    );
}

export default AdminDashboardPage;