    import { FiBarChart2,FiBell,FiCreditCard,FiFolder,FiGrid } from "react-icons/fi"
    
    export const userMenuItems = [
        { label: "Dashboard", path: "/user/dashboard", icon:FiGrid },
        { label: "Transactions", path: "/user/transactions" , icon:FiCreditCard},
        { label: "Categories", path: "/user/categories" ,icon:FiFolder},
        { label: "Reports", path: "/user/reports",icon:FiBarChart2},
    ]

    export const adminMenuItems = [
        { label: "Dashboard", path: "/admin/dashboard",icon:FiGrid },
        { label: "Category Master", path: "/admin/categoryMaster" ,icon:FiFolder},
        { label: "Notifications", path: "/admin/notifications" ,icon:FiBell},
    ]

    export   const pageTitles = {
    "/user/dashboard": "Dashboard",
    "/user/transactions": "Transactions",
    "/user/categories": "Categories",
    "/user/reports": "Reports",
    "/admin/dashboard": "Dashboard",
    "/admin/categoryMaster": "Category Master",
    "/admin/notifications": "Notifications",
  };