import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom'


import LoginPage from './pages/LoginPage'
import RegisterPage from './pages/Register/RegisterPage'
import AdminDashboardPage from './pages/AdminDashboardPage'
import AdminCategoryMasterPage from './pages/AdminCategoryMasterPage'
import AdminNotificationsPage from './pages/AdminNotificationsPage'
import UserDashboardPage from './pages/UserDashboardPage'
import TransactionsPage from './pages/TransactionsPage'
import UserCategoriesPage from './pages/UserCategoriesPage'
import ReportsPage from './pages/ReportsPage'
import AdminLayout from './layouts/AdminLayout'
import UserLayout from './layouts/UserLayout'

function App() {
  return (
    <BrowserRouter>
      <Routes>
        {/* Root */}
        <Route path='/' element={<Navigate to='/login' />}></Route>

        {/* Public Routes */}
        <Route path='/login' element={<LoginPage />}></Route>
        <Route path='/register' element={<RegisterPage />}></Route>

        {/* Admin Routes */}
        <Route path='/admin' element={<AdminLayout />}>
          <Route path='dashboard' element={<AdminDashboardPage />}></Route>
          <Route path='categoryMaster' element={<AdminCategoryMasterPage />}></Route>
          <Route path='notifications' element={<AdminNotificationsPage />}></Route>
        </Route>

        {/* User Routes  */}
        <Route path='/user' element={<UserLayout />}>
          <Route path='dashboard' element={<UserDashboardPage />}></Route>
          <Route path='transactions' element={<TransactionsPage />}></Route>
          <Route path='categories' element={<UserCategoriesPage />}></Route>
          <Route path='reports' element={<ReportsPage />}></Route>
        </Route>
      </Routes>
    </BrowserRouter>
  )
}

export default App