import { useState } from "react"
import { useNavigate } from "react-router-dom";
import { validateRegisterForm } from "../validations/authValidation";
import { registerUser } from "../services/authenticationService";

const initialState =
{
    fullName: "",
    email: "",
    password: "",
    confirmPassword: ""
}

export const useRegisterForm = () => {
    const navigate = useNavigate();

    const [formData, setFormData] = useState(initialState);
    const [errors, setErrors] = useState({});
    const [serverErrors, setServerErrors] = useState("");
    const [isSubmitting, setIsSubmitting] = useState(false);

    const handleChange = (e) => {
        const { name, value } = e.target;

        setFormData((prev) => ({
            ...prev, [name]: value
        }));
    }

    const validateForm = () => {
        const validationErrors = validateRegisterForm(formData);
        setErrors(validationErrors);

        return Object.keys(validationErrors).length === 0;
    }

    const handleSubmit = async (e) => {
        e.preventDefault();
        setServerErrors("");

        if (!validateForm())
            return;

        try {
            setIsSubmitting(true);

            await registerUser(formData);

            navigate("/login");
        }
        catch (error) {
            if (error.response?.status === 409) {
                setServerErrors("An account with this email already exists.");
            } else if (error.response?.status === 400) {
                setServerErrors(
                    error.response.data?.error || "Please check the entered details."
                );
            } else {
                setServerErrors("Registration failed. Please try again.");
            }
        }
        finally {
            setIsSubmitting(false);
        }
    }

    return { formData, errors, serverErrors, isSubmitting, handleChange, handleSubmit };
}