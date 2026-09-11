import api from './api'

export const getCategories = async (basePath) => {
    const response = await api.get(`/${basePath}`);
    return response.data
}

export const getCategoryById = async (basePath, id) => {
    const response = await api.get(`/${basePath}/${id}`);
    return response.data;
}

export const createCategory = async (basePath, categoryData) => {
    const response = await api.post(`/${basePath}`, categoryData);
    return response.data;
}

export const updateCategory = async (basePath, id, categoryData) => {
    await api.put(`/${basePath}/${id}`, categoryData);
}

export const activateCategory = async (basePath, id) => {
    await api.patch(`/${basePath}/${id}/activate`);
}


export const deactivateCategory = async (basePath, id) => {
    await api.patch(`/${basePath}/${id}/deactivate`);
}