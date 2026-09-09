import { useLocation } from "react-router-dom"
import { pageTitles } from "../../config/navigation"
import  './Header.css'

function Header() {

    const location = useLocation();
    const title = pageTitles[location.pathname] || "SpendWise";
    const isAdmin = location.pathname.startsWith("/admin");

    return (
        <header className="header">
            <div className="header-title">
                <p className="header-breadcrumb">SpendWise / {isAdmin ? "Admin" : "User"}</p>
                <h1>{title}</h1>
            </div>

            <div className="header-user">
                <div className="header-user-info">
                    <span className="header-user-name">
                        {isAdmin ? "Administrator" : "User"}
                    </span>
                    <span className="header-user-role">
                        {isAdmin ? "Admin" : "Personal account"}
                    </span>
                </div>

                <div className="header-avatar">{isAdmin ? "A" : "U"}</div>
            </div>


        </header>
    )
}

export default Header