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

import { exists, mapValues } from '../runtime';
import type { TranslateBrowsePathsToNodeIdsResponse } from './TranslateBrowsePathsToNodeIdsResponse';
import {
    TranslateBrowsePathsToNodeIdsResponseFromJSON,
    TranslateBrowsePathsToNodeIdsResponseFromJSONTyped,
    TranslateBrowsePathsToNodeIdsResponseToJSON,
} from './TranslateBrowsePathsToNodeIdsResponse';

/**
 * 
 * @export
 * @interface TranslateBrowsePathsToNodeIdsResponseMessage
 */
export interface TranslateBrowsePathsToNodeIdsResponseMessage {
    /**
     * 
     * @type {Array<string>}
     * @memberof TranslateBrowsePathsToNodeIdsResponseMessage
     */
    NamespaceUris?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof TranslateBrowsePathsToNodeIdsResponseMessage
     */
    ServerUris?: Array<string>;
    /**
     * 
     * @type {number}
     * @memberof TranslateBrowsePathsToNodeIdsResponseMessage
     */
    ServiceId?: number;
    /**
     * 
     * @type {TranslateBrowsePathsToNodeIdsResponse}
     * @memberof TranslateBrowsePathsToNodeIdsResponseMessage
     */
    Body?: TranslateBrowsePathsToNodeIdsResponse;
}

/**
 * Check if a given object implements the TranslateBrowsePathsToNodeIdsResponseMessage interface.
 */
export function instanceOfTranslateBrowsePathsToNodeIdsResponseMessage(value: object): boolean {
    let isInstance = true;

    return isInstance;
}

export function TranslateBrowsePathsToNodeIdsResponseMessageFromJSON(json: any): TranslateBrowsePathsToNodeIdsResponseMessage {
    return TranslateBrowsePathsToNodeIdsResponseMessageFromJSONTyped(json, false);
}

export function TranslateBrowsePathsToNodeIdsResponseMessageFromJSONTyped(json: any, ignoreDiscriminator: boolean): TranslateBrowsePathsToNodeIdsResponseMessage {
    if ((json === undefined) || (json === null)) {
        return json;
    }
    return {
        
        'NamespaceUris': !exists(json, 'NamespaceUris') ? undefined : json['NamespaceUris'],
        'ServerUris': !exists(json, 'ServerUris') ? undefined : json['ServerUris'],
        'ServiceId': !exists(json, 'ServiceId') ? undefined : json['ServiceId'],
        'Body': !exists(json, 'Body') ? undefined : TranslateBrowsePathsToNodeIdsResponseFromJSON(json['Body']),
    };
}

export function TranslateBrowsePathsToNodeIdsResponseMessageToJSON(value?: TranslateBrowsePathsToNodeIdsResponseMessage | null): any {
    if (value === undefined) {
        return undefined;
    }
    if (value === null) {
        return null;
    }
    return {
        
        'NamespaceUris': value.NamespaceUris,
        'ServerUris': value.ServerUris,
        'ServiceId': value.ServiceId,
        'Body': TranslateBrowsePathsToNodeIdsResponseToJSON(value.Body),
    };
}

