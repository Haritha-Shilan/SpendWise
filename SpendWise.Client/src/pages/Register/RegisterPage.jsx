import RegisterForm from "../../components/RegisterForm/RegisterForm"

function RegisterPage() {
  return (
    <div className="auth-page">
      <section className="auth-intro">
        <div className="auth-intro-content">
          <div className="auth-logo">
            <div className="auth-logo-icon">S</div>
            <span>SpendWise</span>
          </div>

          <h1>Start managing
            <br />
            your money.
          </h1>

          <p>
            Create your SpendWise account and get a simple workspace for
            tracking income, expenses, categories and reports.
          </p>
        </div>
      </section>

      <section className="auth-form-section">
        <RegisterForm />
      </section>

    </div>
  )
}

export default RegisterPage