import { AuthConfig } from 'angular-oauth2-oidc';
import { environment } from 'src/environments/environment';

export const authCodeFlowConfig: AuthConfig = {
  loginUrl: `https://identity.${environment.domain}/connect/authorize`,
  redirectUri: `http://${environment.domain}/home`,
  tokenEndpoint: `https://identity.${environment.domain}/connect/token`,
  issuer: `https://identity.${environment.domain}/`,
  clientId: 'hubertyne-spa',
  responseType: 'code',
  scope: 'openid api',
};
