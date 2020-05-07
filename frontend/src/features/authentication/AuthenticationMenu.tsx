import React, { FunctionComponent } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { User } from 'oidc-client';
import { RootState } from '../../app/createRootReducer';
import { register, signIn, signOut } from './authenticationSlice';
import { Button } from 'antd';
import "antd/dist/antd.css";
import styled from 'styled-components';

const ButtonContainer = styled.div`
  background: ${props => props.theme.pageBackground};
`;

const Container: FunctionComponent = ({ children }) => {
  return (
    <div>{children}</div>
  );
};

const PrimaryButton = styled.button`
  text-align: center;
  margin: 0.25rem;
  cursor: pointer;  
  color: ${props => props.theme.color};
  font-size: 1em;
  width: 100px;
  border: 2px solid ${props => props.theme.borderColor};
  border-radius: 3px;
  background: ${props => props.theme.background};
`; 
const SecondaryButton = styled.button`
  text-align: center;
  margin: 0.25rem;
  cursor: pointer;  
  color: ${props => props.theme.color};
  font-size: 1em;
  width: 100px;
  border: 2px solid ${props => props.theme.borderColor};
  background: transparent;
`; 

const SignedOutView: FunctionComponent = () => {
  const dispatch = useDispatch();

  return (
    <ButtonContainer>
      <Container>
        <div><PrimaryButton onClick={() => dispatch(signIn())}>Sign In</PrimaryButton></div>
        <div><SecondaryButton onClick={() => dispatch(register())}>Register</SecondaryButton></div>
      </Container>
    </ButtonContainer>
  );
};

const SignedInView: FunctionComponent<{ user: User; }> = ({ user }) => {
  const dispatch = useDispatch();

  return (
    <ButtonContainer>
      <Container>
        <div>Hello, {user.profile.name}</div>
        <div><PrimaryButton onClick={() => dispatch(signOut())}>Sign Out</PrimaryButton></div>
      </Container>
    </ButtonContainer>
  );
};

const AuthenticationMenu: FunctionComponent = () => {
  const user = useSelector((state: RootState) => state.authentication.user);

  if (user) {
    return (<SignedInView user={user} />)
  }

  return (<SignedOutView />);
};

export default AuthenticationMenu;
