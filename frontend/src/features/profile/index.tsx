import React, { memo, useState, FunctionComponent, Component } from 'react';
import styled, { css, ThemeProvider, ThemedStyledInterface } from 'styled-components';
import ThemeStandardChildren, { Theme, ThemeLightChildren, ThemeDarkChildren } from "../theme/Theme";

const Container = styled.div`
  background: ${props => props.theme.pgBgColor};
  color: ${props => props.theme.color};
  height: 100%;

  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(8rem, 1fr));
  align-content: start;
`;

const Button = styled.button.attrs(props => ({
  
}))`
  text-align: center;
  margin: 0.25rem;
  cursor: pointer;  
  background: ${props => props.theme.background};
  color: ${props => props.theme.color};
  border: 2px solid ${props => props.theme.borderColor};
  border-radius: 3px;
`;


const Profile: FunctionComponent = () => {
  return (
    <></>
  );
};

export default Profile;
