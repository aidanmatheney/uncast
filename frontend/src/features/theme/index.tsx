import React, { FunctionComponent } from 'react';
import { ThemeProvider } from 'styled-components';
// import 'react-h5-audio-player/lib/styles.css';

export interface Theme {
  /** Main color (like for buttons). */
  background: string;

  /** Text color. */
  color: string;

  /** Currently unused. */
  input: string;

  /** Background color of bottom container (auth menu, add stream menu, & navbar). */
  pageBackground: string;

  /** Background color of file upload/RSS input input in forms. */
  addBackground: string;

  /** Border color for buttons. */
  borderColor: string;

  /** Library/catalog/profile page background color for top container. */
  pageBgColor: string;

  /** Font name. */
  font: string;
}

export const standardTheme: Theme = {
  background: "#8c7ce2",
  color: "#ffffff",
  input: "#000000",
  pageBackground: "#443063",
  addBackground: "#b5ade0",
  borderColor: "#ffffff",
  pageBgColor: "#181818",
  font: "Arial"
};

export const StandardThemeChildren: FunctionComponent = ({ children }) => (
  <ThemeProvider theme={standardTheme}>{children}</ThemeProvider>
);

export const darkTheme: Theme = {
  background: "#384245",
  color: "#ffffff",
  input: "#000000",
  pageBackground: "#181818",
  addBackground: "#9c9c9c",
  borderColor: "#ffffff",
  pageBgColor: "#000000",
  font: "Arial"
};

export const DarkThemeChildren: FunctionComponent = ({ children }) => (
  <ThemeProvider theme={darkTheme}>{children}</ThemeProvider>
);

export const lightTheme: Theme = {
  background: "#b0ab8b",
  color: "#181818",
  input: "#000000",
  pageBackground: "#e3e0d1",
  addBackground: "#d6d3bf",
  borderColor: "#181818",
  pageBgColor: "#ffffff",
  font: "Arial"
};

export const LightThemeChildren: FunctionComponent = ({ children }) => (
  <ThemeProvider theme={lightTheme}>{children}</ThemeProvider>
);
