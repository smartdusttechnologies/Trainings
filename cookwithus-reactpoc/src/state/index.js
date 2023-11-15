import { createSlice } from "@reduxjs/toolkit";

const initialState = {
  isSideNavOpen: false,
  darkMode: false,
  isMenuOpen: false,
};

export const cartSlice = createSlice({
  name: "cart",
  initialState,
  reducers: {
    setisSideNavOpen: (state) => {
      state.isSideNavOpen = !state.isSideNavOpen;
    },

    setisMenuOpen: (state) => {
      state.isMenuOpen = !state.isMenuOpen;
    },

    toggleDarkMode: (state) => {
      state.darkMode = !state.darkMode;
    },
  },
});

export const {
  setisSideNavOpen,
  setisMenuOpen,
  toggleDarkMode,
} = cartSlice.actions;

export default cartSlice.reducer;
