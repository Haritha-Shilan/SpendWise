import { FiUserCheck, FiUserX } from "react-icons/fi";
import { useUserManagement } from "../../hooks/useUserManagement"
import "./AdminUserManagementPage.css";
import { getInitials } from "../../utils/getInitials";

function AdminUserManagementPage() {
    const {
        users,
        isLoading,
        error,
        isUpdating,
        updateError,
        toggleUserStatus,
    } = useUserManagement();

    if (isLoading) {
        return (
            <div className="admin-users-page">
                <p>Loading users...</p>
            </div>
        );
    }

    if (error) {
        return (
            <div className="admin-users-page">
                <p className="admin-users-error">{error}</p>
            </div>
        );
    }

    return (
        <div className="admin-users-page">
            <div className="admin-users-header">
                <div>
                    <h2>User Management</h2>
                    <p>Manage registered SpendWise users.</p>
                </div>
            </div>

            {updateError && (
                <p className="admin-users-update-error">
                    {updateError}
                </p>
            )}

            <div className="admin-users-card">
                <div className="admin-users-card-header">
                    <div>
                        <h3>Users</h3>
                        <p>{users.length} registered users</p>
                    </div>
                </div>

                <div className="admin-users-list">
                    {users.length === 0 ? (
                        <p className="admin-users-empty">
                            No users found.
                        </p>
                    ) : (
                        users.map((user) => (
                            <div
                                key={user.id}
                                className="admin-user-item"
                            >
                                <div className="admin-user-details">
                                    <div className="admin-user-avatar">
                                        {getInitials(user.fullName)}
                                    </div>

                                    <div>
                                        <div className="admin-user-name-row">
                                            <p className="admin-user-name">
                                                {user.fullName}
                                            </p>

                                        </div>

                                        <p className="admin-user-email">
                                            {user.email}
                                        </p>

                                        <p className="admin-user-joined">
                                            Joined{" "}
                                            {new Date(user.createdAt).toLocaleDateString()}
                                        </p>
                                    </div>
                                </div>

                                <div className="admin-user-actions">

                                    <span
                                        className={
                                            user.isActive
                                                ? "status-badge active"
                                                : "status-badge inactive"
                                        }
                                    >
                                        {user.isActive ? "Active" : "Inactive"}
                                    </span>

                                    <button
                                        type="button"
                                        disabled={isUpdating}
                                        onClick={() =>
                                            toggleUserStatus(
                                                user.id,
                                                !user.isActive
                                            )
                                        }
                                        className={
                                            user.isActive
                                                ? "admin-user-deactivate-button"
                                                : "admin-user-activate-button"
                                        }
                                    >
                                        {user.isActive ? (
                                            <>
                                                <FiUserX />
                                                Deactivate
                                            </>
                                        ) : (
                                            <>
                                                <FiUserCheck />
                                                Activate
                                            </>
                                        )}
                                    </button>
                                </div>
                            </div>
                        ))
                    )}
                </div>
            </div>
        </div>
    );
}

export default AdminUserManagementPage;