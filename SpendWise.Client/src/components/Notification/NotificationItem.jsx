import { FiBell } from "react-icons/fi";
import "./NotificationItem.css";

function NotificationItem({
    notification,
    onMarkAsRead,
    isUpdating
}) {
    return (
        <div className="notification-item">
            <div className="notification-item-icon">
                <FiBell />
            </div>

            <div className="notification-item-content">
                <p className="notification-item-message">
                    {notification.message}
                </p>

                <span className="notification-item-date">
                    {new Date(notification.createdAt).toLocaleDateString()}
                    {" · Notification remains until marked as read."}
                </span>
            </div>

            <button
                type="button"
                className="notification-item-action"
                disabled={isUpdating}
                onClick={() => onMarkAsRead(notification.id)}
            >
                Mark as read
            </button>
        </div>
    );
}

export default NotificationItem;