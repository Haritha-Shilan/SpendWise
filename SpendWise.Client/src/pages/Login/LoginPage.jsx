
import LoginForm from "../../components/LoginForm/LoginForm";
import "./LoginPage.css";

function LoginPage() {
    return (
        <div className="auth-page">
            <section className="auth-intro">
                <div className="auth-intro-content">
                    <div className="auth-logo">
                        <div className="auth-logo-icon">S</div>
                        <span>SpendWise</span>
                    </div>

                    <h1>
                        Your money.
                        <br />
                        Made clearer.
                    </h1>

                    <p>
                        A simple personal finance workspace to track income, manage expenses, understand spending, and keep everything organized.
                    </p>
                </div>
            </section>

            <section className="auth-form-section">
                <LoginForm />
            </section>
        </div>
    );
}

export default LoginPage;