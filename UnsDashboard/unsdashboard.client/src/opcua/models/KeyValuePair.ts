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
import type { QualifiedName } from './QualifiedName';
import {
    QualifiedNameFromJSON,
    QualifiedNameFromJSONTyped,
    QualifiedNameToJSON,
} from './QualifiedName';

/**
 * 
 * @export
 * @interface KeyValuePair
 */
export interface KeyValuePair {
    /**
     * 
     * @type {QualifiedName}
     * @memberof KeyValuePair
     */
    Key?: QualifiedName;
    /**
     * 
     * @type {object}
     * @memberof KeyValuePair
     */
    Value?: object;
}

/**
 * Check if a given object implements the KeyValuePair interface.
 */
export function instanceOfKeyValuePair(value: object): boolean {
    let isInstance = true;

    return isInstance;
}

export function KeyValuePairFromJSON(json: any): KeyValuePair {
    return KeyValuePairFromJSONTyped(json, false);
}

export function KeyValuePairFromJSONTyped(json: any, ignoreDiscriminator: boolean): KeyValuePair {
    if ((json === undefined) || (json === null)) {
        return json;
    }
    return {
        
        'Key': !exists(json, 'Key') ? undefined : QualifiedNameFromJSON(json['Key']),
        'Value': !exists(json, 'Value') ? undefined : json['Value'],
    };
}

export function KeyValuePairToJSON(value?: KeyValuePair | null): any {
    if (value === undefined) {
        return undefined;
    }
    if (value === null) {
        return null;
    }
    return {
        
        'Key': QualifiedNameToJSON(value.Key),
        'Value': value.Value,
    };
}

