import { AuthConfig } from 'angular-oauth2-oidc';
import { environment } from 'src/environments/environment';

export const authCodeFlowConfig: AuthConfig = {
  loginUrl: `https://${environment.identityHost}/connect/authorize`,
  redirectUri: `https://${environment.dashboardHost}/home`,
  tokenEndpoint: `https://${environment.identityHost}/connect/token`,
  issuer: `https://${environment.identityHost}/`,
  clientId: 'hubertyne-spa',
  responseType: 'code',
  scope: 'openid api',
};
