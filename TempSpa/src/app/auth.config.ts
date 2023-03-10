import { AuthConfig } from 'angular-oauth2-oidc';

export const authCodeFlowConfig: AuthConfig = {
  loginUrl: 'https://identity.localhost/connect/authorize',
  redirectUri: 'https://dashboard.localhost/home',
  tokenEndpoint: 'https://identity.localhost/connect/token',
  issuer: 'https://identity.localhost/',
  clientId: 'hubertyne-spa',
  responseType: 'code',
  scope: 'openid api',
};