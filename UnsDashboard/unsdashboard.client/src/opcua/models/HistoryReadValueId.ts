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
import type { NodeId } from './NodeId';
import {
    NodeIdFromJSON,
    NodeIdFromJSONTyped,
    NodeIdToJSON,
} from './NodeId';
import type { QualifiedName } from './QualifiedName';
import {
    QualifiedNameFromJSON,
    QualifiedNameFromJSONTyped,
    QualifiedNameToJSON,
} from './QualifiedName';

/**
 * 
 * @export
 * @interface HistoryReadValueId
 */
export interface HistoryReadValueId {
    /**
     * 
     * @type {NodeId}
     * @memberof HistoryReadValueId
     */
    NodeId?: NodeId;
    /**
     * 
     * @type {string}
     * @memberof HistoryReadValueId
     */
    IndexRange?: string;
    /**
     * 
     * @type {QualifiedName}
     * @memberof HistoryReadValueId
     */
    DataEncoding?: QualifiedName;
    /**
     * 
     * @type {string}
     * @memberof HistoryReadValueId
     */
    ContinuationPoint?: string;
}

/**
 * Check if a given object implements the HistoryReadValueId interface.
 */
export function instanceOfHistoryReadValueId(value: object): boolean {
    let isInstance = true;

    return isInstance;
}

export function HistoryReadValueIdFromJSON(json: any): HistoryReadValueId {
    return HistoryReadValueIdFromJSONTyped(json, false);
}

export function HistoryReadValueIdFromJSONTyped(json: any, ignoreDiscriminator: boolean): HistoryReadValueId {
    if ((json === undefined) || (json === null)) {
        return json;
    }
    return {
        
        'NodeId': !exists(json, 'NodeId') ? undefined : NodeIdFromJSON(json['NodeId']),
        'IndexRange': !exists(json, 'IndexRange') ? undefined : json['IndexRange'],
        'DataEncoding': !exists(json, 'DataEncoding') ? undefined : QualifiedNameFromJSON(json['DataEncoding']),
        'ContinuationPoint': !exists(json, 'ContinuationPoint') ? undefined : json['ContinuationPoint'],
    };
}

export function HistoryReadValueIdToJSON(value?: HistoryReadValueId | null): any {
    if (value === undefined) {
        return undefined;
    }
    if (value === null) {
        return null;
    }
    return {
        
        'NodeId': NodeIdToJSON(value.NodeId),
        'IndexRange': value.IndexRange,
        'DataEncoding': QualifiedNameToJSON(value.DataEncoding),
        'ContinuationPoint': value.ContinuationPoint,
    };
}

