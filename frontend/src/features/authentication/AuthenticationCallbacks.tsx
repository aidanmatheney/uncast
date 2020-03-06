import React, { FunctionComponent, useEffect } from 'react';
import { useDispatch } from 'react-redux';
import { Route } from 'react-router-dom';
import { signInCallbackPath, signOutCallbackPath } from './constants';
import { completeSignIn, completeSignOut } from './authenticationSlice'

const SignInCallback: FunctionComponent = () => {
  const dispatch = useDispatch();
  useEffect(() => {
    dispatch(completeSignIn());
  }, [dispatch]);

  return (<></>);
};

const SignOutCallback: FunctionComponent = () => {
  const dispatch = useDispatch();
  useEffect(() => {
    dispatch(completeSignOut());
  }, [dispatch]);

  return (<></>);
};

// The callbacks render nothing to the DOM, so we can place them next to the child components
const AuthenticationCallbacks: FunctionComponent = ({ children }) => (<>
  <Route path={signInCallbackPath} component={SignInCallback} />
  <Route path={signOutCallbackPath} component={SignOutCallback} />

  {children}
</>);

export default AuthenticationCallbacks;
