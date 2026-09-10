import { Link } from "react-router-dom"
import './RegisterForm.css'
import { useRegisterForm } from "../../hooks/useRegisterForm"

function RegisterForm() {

    const { formData, errors, serverErrors, isSubmitting, handleChange, handleSubmit } = useRegisterForm();

    return (
        <div className="auth-card">
            <h2>Create your account</h2>

            <p className="auth-card-subtitle">
                It only takes a minute to get started
            </p>

            <form onSubmit={handleSubmit}>
                {/* Server Error */}
                {serverErrors &&
                    <div className="auth-error">
                        {serverErrors}
                    </div>}


                {/* FullName */}
                <div className="auth-form-group">
                    <label htmlFor="fullName">FullName</label>
                    <input id="fullName"
                        type="text"
                        placeholder="Enter your full name"
                        required
                        value={formData.fullName}
                        onChange={handleChange}
                        name="fullName"
                    />
                </div>

                {/* Email */}
                <div className="auth-form-group">
                    <label htmlFor="email">Email</label>
                    <input
                        id="email"
                        name="email"
                        type="email"
                        placeholder="Enter your mail address"
                        required
                        value={formData.email}
                        onChange={handleChange}
                    />
                </div>

                {/* Password */}
                <div className="auth-form-group">
                    <label htmlFor="password">Password</label>
                    <input
                        id="password"
                        name="password"
                        type="password"
                        placeholder="Create a password"
                        required
                        value={formData.password}
                        onChange={handleChange} />
                </div>

                {/* Confirm Password */}
                <div className="auth-form-group">
                    <label htmlFor="confirmPassword">Confirm Password</label>
                    <input
                        id="confirmPassword"
                        name="confirmPassword"
                        type="password"
                        placeholder="Re-enter password"
                        required
                        value={formData.confirmPassword}
                        onChange={handleChange}
                    />

                    {errors.confirmPassword &&
                        (<div className="auth-error"> {errors.confirmPassword} </div>)}
                </div>

                {/* Submit Button */}
                <button
                    type="submit"
                    className="auth-submit-button"
                    disabled={isSubmitting}
                >
                    {isSubmitting ?
                        (
                            <>
                                <span className="auth-spinner"></span>
                                "Creating Account...."
                            </>
                        ) : "Create Account"}
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