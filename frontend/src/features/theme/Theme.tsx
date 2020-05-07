import React from 'react';
import styled, { ThemeProvider } from 'styled-components';
import 'react-h5-audio-player/lib/styles.css';

//standard purple theme
const ThemeStandard = { 
    background: "#8c7ce2",
    color: "#ffffff",
    input: "#000000",
    pageBackground: "#443063",
    addBackground: "#b5ade0",
    borderColor: "#ffffff",
    pageBgColor: "#181818"
  };
const ThemeStandardChildren = ({ children }: any) => (
  <ThemeProvider theme={ThemeStandard}>{children}</ThemeProvider>
);
export default ThemeStandardChildren;

//dark theme
const ThemeDark = { 
  background: "#384245",
  color: "#ffffff",
  input: "#000000",
  pageBackground: "#181818",
  addBackground: "#9c9c9c",
  borderColor: "#ffffff",
  pageBgColor: "#000000"
};
const ThemeDarkChildren = ({ children }: any) => (
<ThemeProvider theme={ThemeDark}>{children}</ThemeProvider>
);
export { ThemeDarkChildren };

//light theme
const ThemeLight = {
  background: "#b0ab8b",
  color: "#181818",
  input: "#000000",
  pageBackground: "#e3e0d1",
  addBackground: "#d6d3bf",
  borderColor: "#181818",
  pageBgColor: "#ffffff"
};
const ThemeLightChildren = ({ children }: any) => (
<ThemeProvider theme={ThemeLight}>{children}</ThemeProvider>
);
export { ThemeLightChildren };