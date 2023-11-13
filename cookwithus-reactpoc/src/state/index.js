import { createSlice } from "@reduxjs/toolkit";

const initialState = {
  isSideNavOpen: false,
  darkMode: false,
};

export const cartSlice = createSlice({
  name: "cart",
  initialState,
  reducers: {
    setisSideNavOpen: (state) => {
      state.isSideNavOpen = !state.isSideNavOpen;
    },

    toggleDarkMode: (state) => {
      state.darkMode = !state.darkMode;
    },
  },
});

export const {
  setisSideNavOpen,
  toggleDarkMode,
} = cartSlice.actions;

export default cartSlice.reducer;
