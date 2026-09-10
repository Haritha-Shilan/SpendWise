export const validateRegisterForm = (formData) => {
    const errors = {};

    if (formData.password !== formData.confirmPassword) {
        errors.confirmPassword = "Passwords do not match.";
    }
    return errors;
}
