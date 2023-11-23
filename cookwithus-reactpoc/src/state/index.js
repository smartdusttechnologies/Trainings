import { createSlice } from "@reduxjs/toolkit";

const initialState = {
  isSideNavOpen: false,
  darkMode: false,
  isMenuOpen: false,
  isBottomNavMenuOpen: false,
  isRightSideNavigationOpen: false,
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

    setRightSideNavigationOpen: (state) => {
      state.isRightSideNavigationOpen = !state.isRightSideNavigationOpen;
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
  setRightSideNavigationOpen,
  toggleDarkMode,
} = cartSlice.actions;

export default cartSlice.reducer;
