import axios from "axios";

const api = axios.create({ baseURL: "https://localhost:7050/api" });

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

export default api;