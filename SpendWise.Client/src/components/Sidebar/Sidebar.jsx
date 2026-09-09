import { NavLink } from "react-router-dom";
import { FiLogOut } from "react-icons/fi";
import './Sidebar.css'

function Sidebar({ menuItems }) {
    return (
        <aside className="sidebar">
            <div className="sidebar-logo">
                <div className="sidebar-logo-icon">S</div>
                <span>SpendWise</span>
            </div>

            <div className="sidebar-section-title">
                WORKSPACE
            </div>

            <nav className="sidebar-nav">
                {
                    menuItems.map((item) => {

                        const Icon = item.icon;

                        return (
                            <NavLink key={item.path}
                                to={item.path}
                                className={({ isActive }) =>
                                    isActive ? "sidebar-link active" : "sidebar-link"
                                }
                            >
                                <Icon className="sidebar-icon" />
                                <span>{item.label}</span>

                            </NavLink>
                        );
                    })
                }
            </nav>

            <div className="sidebar-footer">
                <button type="button" className="sidebar-logout">
                    <FiLogOut className="sidebar-icon" />
                    <span>Logout</span>
                </button>
            </div>
        </aside>
    )
}

export default Sidebar