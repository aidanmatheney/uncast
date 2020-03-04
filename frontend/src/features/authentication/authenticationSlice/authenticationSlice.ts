import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import { replace as replacePath } from 'connected-react-router';
import { User, SignoutResponse } from "oidc-client";

import { registerUrl } from "../constants";
import userManager from "../userManager";

import { createSignInOutArgs } from "./SignInOutArgs";
import SignInOutData from "./SignInOutData";
import SignInError from "./SignInError";
import SignOutError from "./SignOutError";


const sliceName = 'authentication';

interface AuthenticationState {
  user: User | null; // TODO: Only store a serializable subset of the User object created by oidc-client's UserManager
  signInError: SignInError | null;
  signOutError: SignOutError | null;
}

const initialState: AuthenticationState = {
  user: null,
  signInError: null,
  signOutError: null
};


export const loadUser = createAsyncThunk(`${sliceName}/loadUser`, async () => {
  return await userManager.getUser();
});


export const signIn = createAsyncThunk(`${sliceName}/signIn`, async () => {
  const args = createSignInOutArgs();

  let silentError: any;
  try {
    return await userManager.signinSilent(args);
  } catch (err) {
    silentError = err;
  }

  let popupError: any;
  try {
    return await userManager.signinPopup(args);
  } catch (err) {
    popupError = err;
  }

  let redirectError: any;
  try {
    await userManager.signinRedirect(args);
    return null;
  } catch (err) {
    redirectError = err;
  }

  throw new SignInError(silentError, popupError, redirectError);
});

export const completeSignIn = createAsyncThunk(`${sliceName}/completeSignIn`, async (_, { dispatch }) => {
  let user: User;
  try {
    user = await userManager.signinCallback(window.location.href);
  } catch (err) {
    dispatch(replacePath('/'));
    throw err;
  }

  const state: SignInOutData | null = user?.state ?? null;
  dispatch(replacePath(state?.returnUrl ?? '/'));

  return user;
});


export const signOut = createAsyncThunk(`${sliceName}/signOut`, async () => {
  const args = createSignInOutArgs();

  let popupError: any;
  try {
    await userManager.signoutPopup(args);
    return;
  } catch (err) {
    popupError = err;
  }

  let redirectError: any;
  try {
    await userManager.signoutRedirect(args);
    return;
  } catch (err) {
    redirectError = err;
  }

  throw new SignOutError(popupError, redirectError);
});

export const completeSignOut = createAsyncThunk(`${sliceName}/completeSignOut`, async (_, { dispatch }) => {
  let response: SignoutResponse | void; // No response if the sign-out happened via popup
  try {
    response = await userManager.signoutCallback(window.location.href);
  } catch (err) {
    dispatch(replacePath('/'));
    throw err;
  }

  const state: SignInOutData | null = response ? response.state : null;
  dispatch(replacePath(state?.returnUrl ?? '/'));
});


export const authenticationSlice = createSlice({
  name: sliceName,
  initialState,
  reducers: {
    register: () => {
      window.location.href = registerUrl;
    }
  },
  extraReducers: map => {
    map.addCase(loadUser.fulfilled, (state, { payload: user }) => {
      state.user = user;
    });

    map.addCase(signIn.fulfilled, (state, { payload: user }) => {
      state.user = user;
    });
    map.addCase(signIn.rejected, (state, { error }) => {
      state.signInError = error;
    });

    map.addCase(completeSignIn.fulfilled, (state, { payload: user }) => {
      state.user = user;
    });
    map.addCase(completeSignIn.rejected, (state, { error }) => {
      state.signInError = error;
    });

    map.addCase(signOut.fulfilled, state => {
      state.user = null;
    });
    map.addCase(signOut.rejected, (state, { error }) => {
      state.signOutError = error;
    });

    map.addCase(completeSignOut.fulfilled, state => {
      state.user = null;
    });
    map.addCase(completeSignOut.rejected, (state, { error }) => {
      state.signOutError = error;
    });
  }
});


export const {
  register
} = authenticationSlice.actions;

export default authenticationSlice.reducer;
