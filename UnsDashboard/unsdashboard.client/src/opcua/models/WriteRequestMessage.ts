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
import type { WriteRequest } from './WriteRequest';
import {
    WriteRequestFromJSON,
    WriteRequestFromJSONTyped,
    WriteRequestToJSON,
} from './WriteRequest';

/**
 * 
 * @export
 * @interface WriteRequestMessage
 */
export interface WriteRequestMessage {
    /**
     * 
     * @type {Array<string>}
     * @memberof WriteRequestMessage
     */
    NamespaceUris?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof WriteRequestMessage
     */
    ServerUris?: Array<string>;
    /**
     * 
     * @type {Array<string>}
     * @memberof WriteRequestMessage
     */
    LocaleIds?: Array<string>;
    /**
     * 
     * @type {number}
     * @memberof WriteRequestMessage
     */
    ServiceId?: number;
    /**
     * 
     * @type {WriteRequest}
     * @memberof WriteRequestMessage
     */
    Body?: WriteRequest;
}

/**
 * Check if a given object implements the WriteRequestMessage interface.
 */
export function instanceOfWriteRequestMessage(value: object): boolean {
    let isInstance = true;

    return isInstance;
}

export function WriteRequestMessageFromJSON(json: any): WriteRequestMessage {
    return WriteRequestMessageFromJSONTyped(json, false);
}

export function WriteRequestMessageFromJSONTyped(json: any, ignoreDiscriminator: boolean): WriteRequestMessage {
    if ((json === undefined) || (json === null)) {
        return json;
    }
    return {
        
        'NamespaceUris': !exists(json, 'NamespaceUris') ? undefined : json['NamespaceUris'],
        'ServerUris': !exists(json, 'ServerUris') ? undefined : json['ServerUris'],
        'LocaleIds': !exists(json, 'LocaleIds') ? undefined : json['LocaleIds'],
        'ServiceId': !exists(json, 'ServiceId') ? undefined : json['ServiceId'],
        'Body': !exists(json, 'Body') ? undefined : WriteRequestFromJSON(json['Body']),
    };
}

export function WriteRequestMessageToJSON(value?: WriteRequestMessage | null): any {
    if (value === undefined) {
        return undefined;
    }
    if (value === null) {
        return null;
    }
    return {
        
        'NamespaceUris': value.NamespaceUris,
        'ServerUris': value.ServerUris,
        'LocaleIds': value.LocaleIds,
        'ServiceId': value.ServiceId,
        'Body': WriteRequestToJSON(value.Body),
    };
}

