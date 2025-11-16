import { InjectionToken } from "@angular/core";
import { AuthConfig } from "angular-oauth2-oidc";
import { IAppConfig } from "./app-config";

export const APP_CONFIG = new InjectionToken<IAppConfig>('app.config');

export const clientid = "2d2bae6b-b650-4a24-85b8-51b3248d2b77";
export const tenantId = "420d8d99-43b5-46a6-a6d5-2cfc459bc193";

export const API_RESOURCES: IAppConfig = {
  productGroupListApi: {
    endpoint: "https://localhost:5001/productgroup",
    scopes: [`api://${clientid}/Data.Read`],
  },
  productGroupDetailApi: {
    endpoint: "https://localhost:5001/productgroup",
    scopes: [`api://${clientid}/Data.Read`],
  },
}
export const customScopes = [`api://${clientid}/Data.Read`];

export const authConfig : AuthConfig = {
  issuer:`https://login.microsoftonline.com/${tenantId}/v2.0`,
  strictDiscoveryDocumentValidation:false,
  useSilentRefresh: true,
  silentRefreshRedirectUri: `${window.location.origin}/silent-refresh.html`,
  redirectUri: `${window.location.origin}/index.html`,
  clientId:clientid,
  responseType:"code",
  scope:  "openid profile email offline_access " + customScopes[0],
  //disablePKCE:false,
  //clearHashAfterLogin:true,
  showDebugInformation:true  
};
