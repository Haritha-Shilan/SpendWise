import api from './api'

export const registerUser = async (registerData) => {
    const response = await api.post("/authentication/register", registerData);

    return response.data;
}