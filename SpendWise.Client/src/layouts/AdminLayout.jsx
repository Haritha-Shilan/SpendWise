import { Outlet } from "react-router-dom"
import Sidebar from "../components/Sidebar/Sidebar"
import Header from "../components/Header/Header"
import { adminMenuItems } from "../config/navigation"

function AdminLayout() {
  return (
    <div className="app-layout">
      <Sidebar menuItems={adminMenuItems} />

      <main className="main-content">
        <Header />

        <section className="page-content">
          <Outlet />
        </section>
      </main>
    </div>
  )
}

export default AdminLayout