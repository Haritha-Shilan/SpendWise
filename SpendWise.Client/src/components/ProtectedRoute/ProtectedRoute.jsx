import { useSelector } from "react-redux"
import { Navigate, Outlet } from "react-router-dom";

function ProtectedRoute({ allowedRole }) {
    const { isAuthenticated, role, isInitializing } =
        useSelector((state) => state.auth);

    if (isInitializing) {
        return null;
    }

    if (!isAuthenticated) {
        console.log("redirecting..");
        return <Navigate to="/login" />
    }

    if (allowedRole && role !== allowedRole) {
        return <Navigate to="/login" />
    }

    return <Outlet />
}

export default ProtectedRoute