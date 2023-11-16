import { createSlice } from "@reduxjs/toolkit";

const initialState = {
  isSideNavOpen: false,
  darkMode: false,
  isMenuOpen: false,
  isBottomNavMenuOpen: false,
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

    setisBottomNavMenuOpen: (state) => {
      state.isBottomNavMenuOpen = !state.isBottomNavMenuOpen;
    },

    toggleDarkMode: (state) => {
      state.darkMode = !state.darkMode;
    },
  },
});

export const {
  setisSideNavOpen,
  setisMenuOpen,
  setisBottomNavMenuOpen,
  toggleDarkMode,
} = cartSlice.actions;

export default cartSlice.reducer;
