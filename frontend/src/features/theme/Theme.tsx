import React from 'react';
import styled, { ThemeProvider } from 'styled-components';
import 'react-h5-audio-player/lib/styles.css';

const ThemeStandard = {
    background: "#8c7ce2",
    color: "#ffffff",
    input: "000000"

    
  };
  
  const ThemeStandardChildren = ({ children }: any) => (
    <ThemeProvider theme={ThemeStandard}>{children}</ThemeProvider>
  );
  
  export default ThemeStandardChildren;