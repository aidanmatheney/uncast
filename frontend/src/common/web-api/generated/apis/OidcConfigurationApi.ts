/* tslint:disable */
/* eslint-disable */
/**
 * Uncast Web API
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * The version of the OpenAPI document: v1
 * 
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */


import * as runtime from '../runtime';

export interface GetOidcConfigurationClientRequestParametersRequest {
    clientId: string;
}

/**
 * no description
 */
export class OidcConfigurationApi extends runtime.BaseAPI {

    /**
     */
    async getOidcConfigurationClientRequestParametersRaw(requestParameters: GetOidcConfigurationClientRequestParametersRequest): Promise<runtime.ApiResponse<{ [key: string]: string; }>> {
        if (requestParameters.clientId === null || requestParameters.clientId === undefined) {
            throw new runtime.RequiredError('clientId','Required parameter requestParameters.clientId was null or undefined when calling getOidcConfigurationClientRequestParameters.');
        }

        const queryParameters: runtime.HTTPQuery = {};

        const headerParameters: runtime.HTTPHeaders = {};

        const response = await this.request({
            path: `/_configuration/{clientId}`.replace(`{${"clientId"}}`, encodeURIComponent(String(requestParameters.clientId))),
            method: 'GET',
            headers: headerParameters,
            query: queryParameters,
        });

        return new runtime.JSONApiResponse<any>(response);
    }

    /**
     */
    async getOidcConfigurationClientRequestParameters(requestParameters: GetOidcConfigurationClientRequestParametersRequest): Promise<{ [key: string]: string; }> {
        const response = await this.getOidcConfigurationClientRequestParametersRaw(requestParameters);
        return await response.value();
    }

}
