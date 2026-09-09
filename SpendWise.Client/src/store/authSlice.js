import { createSlice } from "@reduxjs/toolkit"

const initialState={
    token:null,
    user:null,
    role:null,
    isAuthenticated:false
}

const authSlice = createSlice({
    name:"auth",
    initialState,
    reducers:{
     login:(state,action)=>{
        state.token =action.payload.token;
        state.user=action.payload.user;
        state.role=action.payload.role;
        state.isAuthenticated=true;
     },

     logout:(state)=>
     {
        state.token =null;
        state.user=null;
        state.role=null;
        state.isAuthenticated=false;
     }
    },
});

export default authSlice.reducer;
export const {login,logout} = authSlice.actions;