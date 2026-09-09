import { useState } from "react"
import { Link } from "react-router-dom"
import { validateRegisterForm } from "../../validations/authValidation";
import './RegisterForm.css'

function RegisterForm() {
    const initialState = {
        fullName: "",
        email: "",
        password: "",
        confirmPassword: ""
    }
    const [formData, setFormData] = useState(initialState);
    const [erros, setErrors] = useState({});

    const handleSubmit = (e) => {
        e.preventDefault();

        if (validateForm()) {
            console.log("created");
        }
    }

    const validateForm = () => {
        const validationErrors = validateRegisterForm(formData);
        setErrors(validationErrors);

        return Object.keys(validationErrors).length === 0;
    }

    return (
        <div className="auth-card">
            <h2>Create your account</h2>

            <p className="auth-card-subtitle">
                It only takes a minute to get started
            </p>

            <form onSubmit={handleSubmit}>
                {/* FullName */}
                <div className="auth-form-group">
                    <label htmlFor="fullName">FullName</label>
                    <input id="fullName"
                        type="text"
                        placeholder="Enter your full name"
                        required
                        value={formData.fullName}
                        onChange={(e) => {
                            setFormData({ ...formData, fullName: e.target.value })
                        }}
                    />
                </div>

                {/* Email */}
                <div className="auth-form-group">
                    <label htmlFor="email">Email</label>
                    <input
                        id="email"
                        type="email"
                        placeholder="Enter your mail address"
                        required
                        value={formData.email}
                        onChange={(e) => {
                            setFormData({ ...formData, email: e.target.value })
                        }}
                    />
                </div>

                {/* Password */}
                <div className="auth-form-group">
                    <label htmlFor="password">Password</label>
                    <input
                        id="password"
                        type="password"
                        placeholder="Create a password"
                        required
                        value={formData.password}
                        onChange={(e) => {
                            setFormData({ ...formData, password: e.target.value })
                        }} />
                </div>

                {/* Confirm Password */}
                <div className="auth-form-group">
                    <label htmlFor="confirmPassword">Confirm Password</label>
                    <input
                        id="confirmPassword"
                        type="password"
                        placeholder="Re-enter password"
                        required
                        value={formData.confirmPassword}
                        onChange={(e) => {
                            setFormData({ ...formData, confirmPassword: e.target.value })
                        }}
                    />

                    {erros.confirmPassword &&
                        (<div className="auth-error"> {erros.confirmPassword} </div>)}
                </div>

                {/* Submit Button */}
                <button
                    type="submit"
                    className="auth-submit-button"
                >
                    Create Account
                </button>
            </form>

            <p className="auth-switch">
                Already have an account?{" "}
                <Link to="/login">Sign in</Link>
            </p>
        </div>
    )
}

export default RegisterForm