import { Outlet } from "react-router-dom"
import Sidebar from "../components/Sidebar/Sidebar"
import { userMenuItems } from "../config/navigation"
import Header from "../components/Header/Header"

function UserLayout() {
  return (
    <div className="app-layout">

      <Sidebar menuItems={userMenuItems} />

      <main className="main-content">
        <Header />

        <section className="page-content">
          <Outlet />
        </section>
      </main>
    </div>
  )
}

export default UserLayout