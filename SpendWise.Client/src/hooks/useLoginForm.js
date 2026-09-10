import { useState } from "react"
import { loginUser } from "../services/authenticationService";
import { jwtDecode } from "jwt-decode";
import { useDispatch } from "react-redux";
import { login } from '../store/authSlice';
import { jwtClaims } from "../config/jwtClaims";
import { useNavigate } from "react-router-dom";

const initialState =
{
    email: "",
    password: ""
}

export const useLoginForm = () => {
    const [formData, setFormData] = useState(initialState);
    const [serverError, setServerError] = useState("");
    const [isSubmitting, setIsSubmitting] = useState(false);

    const dispatch = useDispatch();
    const navigate = useNavigate();

    const handleChange = (e) => {
        const { name, value } = e.target;

        setFormData((prev) => (
            { ...prev, [name]: value }
        ));
        setServerError("");
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setServerError("");

        try {
            setIsSubmitting(true);

            const response = await loginUser(formData);

            localStorage.setItem("spendWiseToken", response.token);

            const decodedToken = jwtDecode(response.token)

            const user = {
                id: decodedToken[jwtClaims.nameIdentifier],
                email: decodedToken[jwtClaims.email],
                fullName: decodedToken[jwtClaims.name]
            };

            const role =
                decodedToken[jwtClaims.role];

            dispatch(login(
                {
                    token: response.token,
                    user,
                    role
                }
            ));

            if (role === "Admin") {
                navigate("/admin/dashboard");
            }
            else {
                navigate("/user/dashboard");
            }

        }
        catch (error) {
            if (error.response?.status === 401) {
                setServerError("Invalid email or password.");
            } else {
                setServerError("Login failed. Please try again.");
            }
        }
        finally {
            setIsSubmitting(false);
        }
    }

    return { formData, handleChange, handleSubmit, isSubmitting, serverError };
}