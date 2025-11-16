import { InjectionToken } from "@angular/core";
import { BrowserCacheLocation, Configuration, LogLevel } from "@azure/msal-browser";
import { IAppConfig } from "./app-config";

export const APP_CONFIG = new InjectionToken<IAppConfig>('app.config');

export const msalConfig: Configuration = {
  auth:{
    clientId:"2d2bae6b-b650-4a24-85b8-51b3248d2b77",
    authority:"https://login.microsoftonline.com/common",
    redirectUri:window.location.origin,
    postLogoutRedirectUri:window.location.origin
  },
  cache:{
    cacheLocation:BrowserCacheLocation.LocalStorage
  },
  system:{
    loggerOptions: {
          logLevel:LogLevel.Verbose,
          loggerCallback:(lvl, msg)=>console.log(msg),
          piiLoggingEnabled:false
    }
  }
}

export const API_RESOURCES: IAppConfig = {
  productGroupListApi: {
    endpoint: "https://localhost:5001/productgroup",
    scopes: ["api://2d2bae6b-b650-4a24-85b8-51b3248d2b77/Data.Read"],
  },
  productGroupDetailApi: {
    endpoint: "https://localhost:5001/productgroup",
    scopes: ["api://2d2bae6b-b650-4a24-85b8-51b3248d2b77/Data.Read",  "api://2d2bae6b-b650-4a24-85b8-51b3248d2b77/Data.Write"],
  },
}
