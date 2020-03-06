import SignInOutData from "./SignInOutData";

export default interface SignInOutArgs {
  response_type?: any;
  scope?: any;
  redirect_uri?: any;

  // data was meant to be the place a caller could indicate the data to
  // have round tripped, but people were getting confused, so i added state (since that matches the spec)
  // and so now if data is not passed, but state is then state will be used
  data?: any;
  state?: SignInOutData;
  prompt?: any;
  display?: any;
  max_age?: any;
  ui_locales?: any;
  id_token_hint?: any;
  login_hint?: any;
  acr_values?: any;
  resource?: any;
  request?: any;
  request_uri?: any;
  response_mode?: any;
  extraQueryParams?: any;
  extraTokenParams?: any;
  request_type?: any;
  skipUserInfo?: any;

  useReplaceToNavigate?: boolean;
  silentRequestTimeout?: number;
}

export const createSignInOutArgs = () => {
  const args: SignInOutArgs = {
    useReplaceToNavigate: true,
    silentRequestTimeout: 5000,
    state: {
      returnUrl: `${window.location.pathname}${window.location.search}${window.location.hash}`
    }
  };

  return args;
};
