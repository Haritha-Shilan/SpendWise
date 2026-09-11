import { useNotifications } from "../../hooks/useNotifications"
import "./AdminNotificationsPage.css";
import NotificationItem from "../../components/Notification/NotificationItem"

function AdminNotificationsPage() {
    const {
        notifications,
        isLoading,
        error,
        isUpdating,
        markAsRead,
        markAllAsRead
    } = useNotifications();

    return (
        <div className="notifications-page">

            <div className="notifications-card">

                <div className="notifications-card-header">
                    <div>
                        <h2>Notifications</h2>
                        <p>
                            Registration and system notifications
                        </p>
                    </div>

                    {!isLoading &&
                        !error &&
                        notifications.length > 0 && (
                            <button
                                type="button"
                                className="mark-all-button"
                                onClick={markAllAsRead}
                                disabled={isUpdating}
                            >
                                Mark all as read
                            </button>
                        )}
                </div>

                {isLoading && (
                    <p>Loading notifications...</p>
                )}

                {error && (
                    <p className="notifications-error">
                        {error}
                    </p>
                )}

                {!isLoading &&
                    !error &&
                    notifications.length === 0 && (
                        <p className="notifications-empty">
                            No new notifications.
                        </p>
                    )}

                {!isLoading &&
                    !error &&
                    notifications.length > 0 && (
                        <div className="notifications-list">
                            {notifications.map((notification) => (
                                <NotificationItem
                                    key={notification.id}
                                    notification={notification}
                                    onMarkAsRead={markAsRead}
                                    isUpdating={isUpdating}
                                />
                            ))}
                        </div>
                    )}

            </div>

        </div>
    );
}

export default AdminNotificationsPage;