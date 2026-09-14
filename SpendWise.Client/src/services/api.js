import axios from "axios";

const api = axios.create({
    baseURL: "https://localhost:7050/api"
});

api.interceptors.request.use(
    (config) => {
        const token = localStorage.getItem("spendWiseToken");

        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }

        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

api.interceptors.response.use(
    (response) => {
        return response;
    },
    (error) => {
        if (error.response?.status === 401) {
            localStorage.removeItem("spendWiseToken");

            window.location.href = "/login";
        }

        return Promise.reject(error);
    }
);

export default api;