import { UserManager, WebStorageStateStore } from 'oidc-client';
import { appUrl, clientId, apiUrl, apiScope } from '../../common/constants';
import { signInCallbackPath, signOutCallbackPath } from './constants';

const userManager = new UserManager({
  authority: apiUrl,
  client_id: clientId,
  response_type: 'code',
  scope: `openid profile ${apiScope}`,
  redirect_uri: `${appUrl}${signInCallbackPath}`,
  post_logout_redirect_uri: `${appUrl}${signOutCallbackPath}`,

  automaticSilentRenew: true,
  includeIdTokenInSilentRenew: true,
  userStore: new WebStorageStateStore({
    prefix: 'uncast',
    store: localStorage
  })
});

export default userManager;
