import React, { FunctionComponent } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { User } from 'oidc-client';
import { RootState } from '../../app/createRootReducer';
import { register, signIn, signOut } from './authenticationSlice';

const Container: FunctionComponent = ({ children }) => {
  return (
    <div>{children}</div>
  );
};

const SignedOutView: FunctionComponent = () => {
  const dispatch = useDispatch();

  return (
    <Container>
      <div><button type="button" onClick={() => dispatch(signIn())}>Sign In</button></div>
      <div><button type="button" onClick={() => dispatch(register())}>Register</button></div>
    </Container>
  );
};

const SignedInView: FunctionComponent<{ user: User; }> = ({ user }) => {
  const dispatch = useDispatch();

  return (
    <Container>
      <div>Hello, {user.profile.name}</div>
      <div><button type="button" onClick={() => dispatch(signOut())}>Sign Out</button></div>
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
