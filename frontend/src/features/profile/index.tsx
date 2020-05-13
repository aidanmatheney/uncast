import React, { FunctionComponent } from 'react';
import styled from 'styled-components';
import { useDispatch } from 'react-redux';
import { setTheme } from '../user/userSlice';
import { themeInfos } from '../theme/themeInfos';

// import styled, { css } from 'styled-components';

// const Container = styled.div`
//   background: ${props => props.theme.pgBgColor};
//   color: ${props => props.theme.color};
//   height: 100%;

//   display: grid;
//   grid-template-columns: repeat(auto-fill, minmax(8rem, 1fr));
//   align-content: start;
// `;

// const Button = styled.button`
//   text-align: center;
//   margin: 0.25rem;
//   cursor: pointer;
//   background: ${props => props.theme.background};
//   color: ${props => props.theme.color};
//   border: 2px solid ${props => props.theme.borderColor};
//   border-radius: 3px;
// `;

// const Button = styled.button`
//   text-align: center;
//   margin: 0.25rem;
//   cursor: pointer;
//   background: ${props => props.theme.background};
//   color: ${props => props.theme.color};
//   border: 2px solid ${props => props.theme.borderColor};
//   border-radius: 3px;
// `;

const ThemeButton = styled.button`
  text-align: center;
  margin: 0.25rem;
  cursor: pointer;
  background: ${props => props.theme.background};
  color: ${props => props.theme.color};
  border: 2px solid ${props => props.theme.borderColor};
  border-radius: 3px;
`;

const Container = styled.div`
  text-align: center;
  margin: 0.25rem;
  background: ${props => props.theme.pageBackground};
  color: ${props => props.theme.color};
  border: 2px solid ${props => props.theme.borderColor};
  border-radius: 3px;

  display: inline-block;
`;

const Profile: FunctionComponent = () => {
  const dispatch = useDispatch();

  return (
    <Container>
      {themeInfos.map(({ name, theme }) => (
        <ThemeButton onClick={() => dispatch(setTheme(theme))} key={name}>
          {name}
        </ThemeButton>
      ))}
    </Container>
  );
};

export default Profile;
