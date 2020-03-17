import React, { FunctionComponent } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { User } from 'oidc-client';
import { RootState } from '../../app/createRootReducer';
import { register, signIn, signOut } from './authenticationSlice';
import { Button } from 'antd';
import "antd/dist/antd.css";

const Container: FunctionComponent = ({ children }) => {
  return (
    <div>{children}</div>
  );
};

const SignedOutView: FunctionComponent = () => {
  const dispatch = useDispatch();

  return (
    <Container>
      <div><Button type="primary" onClick={() => dispatch(signIn())}>Sign In</Button></div>
      <div><Button type="default" onClick={() => dispatch(register())}>Register</Button></div>
    </Container>
  );
};

const SignedInView: FunctionComponent<{ user: User; }> = ({ user }) => {
  const dispatch = useDispatch();

  return (
    <Container>
      <div>Hello, {user.profile.name}</div>
      <div><Button type="primary" onClick={() => dispatch(signOut())}>Sign Out</Button></div>
    </Container>
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
