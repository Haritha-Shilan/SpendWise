import { jwtDecode } from "jwt-decode";
import { useEffect } from "react";
import { useDispatch } from "react-redux"
import { logout, login, initializeAuth } from "../store/authSlice";
import { jwtClaims } from "../config/jwtClaims";

export const useInitializeAuth = () => {
    const dispatch = useDispatch();

    useEffect(() => {
        const token = localStorage.getItem("spendWiseToken");

        if (!token) {
            dispatch(initializeAuth());
            return;
        }

        try {
            const decodedToken = jwtDecode(token);
            const currentTime = Date.now() / 1000;

            if (decodedToken.exp && decodedToken.exp < currentTime) {
                localStorage.removeItem("spendWiseToken");
                dispatch(logout());
                dispatch(initializeAuth());
                return;
            }

            const user = {
                id: decodedToken[jwtClaims.nameIdentifier],
                email: decodedToken[jwtClaims.email],
                fullName: decodedToken[jwtClaims.name]
            };

            const role = decodedToken[jwtClaims.role];

            dispatch(login(
                {
                    token,
                    user,
                    role
                }
            ));
            dispatch(initializeAuth());
        }
        catch {
            localStorage.removeItem("spendWiseToken");
            dispatch(logout());
            dispatch(initializeAuth());
        }
    }
        , [dispatch]);
};