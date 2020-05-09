import React from 'react';
import styled, { ThemeProvider } from 'styled-components';
import 'react-h5-audio-player/lib/styles.css';

//standard purple theme
const ThemeStandard = { 
    background: "#8c7ce2",    //"main color", like for buttons
    color: "#ffffff",         //text color
    input: "#000000",         //currently unused
    pageBackground: "#443063",//background color of bottom container (auth menu, add stream menu, & navbar)
    addBackground: "#b5ade0", //background color of file upload/rss input/youtube input in forms
    borderColor: "#ffffff",   //border color for buttons
    pageBgColor: "#181818",   //library/catalog/profile page background color for top container
    font: "Arial"
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
  pageBgColor: "#000000",
  font: "Arial"
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
  pageBgColor: "#ffffff",
  font: "Arial"
};
const ThemeLightChildren = ({ children }: any) => (
<ThemeProvider theme={ThemeLight}>{children}</ThemeProvider>
);
export { ThemeLightChildren };