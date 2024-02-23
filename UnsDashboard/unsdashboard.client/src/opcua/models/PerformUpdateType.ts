/* tslint:disable */
/* eslint-disable */
/**
 * OPC UA REST API
 * This API provides simple REST based access to an OPC UA server.
 *
 * The version of the OpenAPI document: 0.0.1
 * Contact: office@opcfoundation.org
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */


/**
 * 
 * @export
 */
export const PerformUpdateType = {
    Insert: 1,
    Replace: 2,
    Update: 3,
    Remove: 4
} as const;
export type PerformUpdateType = typeof PerformUpdateType[keyof typeof PerformUpdateType];


export function PerformUpdateTypeFromJSON(json: any): PerformUpdateType {
    return PerformUpdateTypeFromJSONTyped(json, false);
}

export function PerformUpdateTypeFromJSONTyped(json: any, ignoreDiscriminator: boolean): PerformUpdateType {
    return json as PerformUpdateType;
}

export function PerformUpdateTypeToJSON(value?: PerformUpdateType | null): any {
    return value as any;
}
