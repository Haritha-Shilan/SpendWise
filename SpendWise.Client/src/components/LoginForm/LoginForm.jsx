import { useLoginForm } from '../../hooks/useLoginForm'
import './LoginForm.css'

function LoginForm() {
    const { formData, handleChange, handleSubmit, isSubmitting, serverError } = useLoginForm();
    return (
        <div className="login-form-container">
            <div className="login-form-header">
                <h2>Welcome back</h2>
                <p>Sign in to continue to SpendWise</p>
            </div>

            <form onSubmit={handleSubmit}>

                {serverError && (
                    <div className="auth-error">
                        {serverError}
                    </div>
                )}

                {/* Email */}
                <div className="login-form-group">
                    <label htmlFor="email">Email</label>
                    <input
                        type="email"
                        id="email"
                        name="email"
                        value={formData.email}
                        onChange={handleChange}
                        placeholder="Enter your email"
                        required
                    />
                </div>

                {/* Password */}
                <div className="login-form-group">
                    <label htmlFor="password">Password</label>
                    <input
                        type="password"
                        id="password"
                        name="password"
                        value={formData.password}
                        onChange={handleChange}
                        placeholder="Enter your password"
                        required
                    />
                </div>

                <button type="submit" className="login-submit-button" disabled={isSubmitting}>
                    {isSubmitting ? (
                        <>
                            <span className="auth-spinner"></span>
                            Signing in...
                        </>
                    ) : (
                        "Sign in"
                    )}
                </button>
            </form>
            <p className="login-register-link">
                New to SpendWise? <a href="/register">Create an account</a>
            </p>
        </div>
    )
}

export default LoginForm