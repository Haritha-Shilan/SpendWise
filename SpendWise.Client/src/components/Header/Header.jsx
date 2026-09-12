import { useLocation } from "react-router-dom"
import { pageTitles } from "../../config/navigation"
import './Header.css'
import { useSelector } from "react-redux";
import { getInitials } from "../../utils/getInitials";

function Header() {

    const location = useLocation();
    const title = pageTitles[location.pathname] || "SpendWise";
    const isAdmin = location.pathname.startsWith("/admin");
    const user = useSelector((state) => state.auth.user);

    return (
        <header className="header">
            <div className="header-title">
                <p className="header-breadcrumb">SpendWise / {isAdmin ? "Admin" : "Personal Finance"}</p>
                <h1>{title}</h1>
            </div>

            <div className="header-user">
                <div className="header-user-info">
                    <span className="header-user-name">
                        {user?.fullName ?? ""}
                    </span>
                    <span className="header-user-role">
                        {isAdmin ? "Admin" : "Personal account"}
                    </span>
                </div>

                <div className="header-avatar">
                    {user?.fullName ? getInitials(user.fullName) : ""}
                </div>
            </div>


        </header>
    )
}

export default Header