export interface IEndpointConfig
{
  endpoint: string;
  scopes: string[];
}
export interface IAppConfig {
  productGroupListApi: IEndpointConfig;
  productGroupDetailApi: IEndpointConfig
}
  