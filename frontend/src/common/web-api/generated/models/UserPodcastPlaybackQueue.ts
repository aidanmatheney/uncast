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

import { exists, mapValues } from '../runtime';
/**
 * 
 * @export
 * @interface UserPodcastPlaybackQueue
 */
export interface UserPodcastPlaybackQueue {
    /**
     * 
     * @type {string}
     * @memberof UserPodcastPlaybackQueue
     */
    userId?: string | null;
    /**
     * 
     * @type {Array<string>}
     * @memberof UserPodcastPlaybackQueue
     */
    episodeIds?: Array<string> | null;
}

export function UserPodcastPlaybackQueueFromJSON(json: any): UserPodcastPlaybackQueue {
    return UserPodcastPlaybackQueueFromJSONTyped(json, false);
}

export function UserPodcastPlaybackQueueFromJSONTyped(json: any, ignoreDiscriminator: boolean): UserPodcastPlaybackQueue {
    if ((json === undefined) || (json === null)) {
        return json;
    }
    return {
        
        'userId': !exists(json, 'userId') ? undefined : json['userId'],
        'episodeIds': !exists(json, 'episodeIds') ? undefined : json['episodeIds'],
    };
}

export function UserPodcastPlaybackQueueToJSON(value?: UserPodcastPlaybackQueue | null): any {
    if (value === undefined) {
        return undefined;
    }
    if (value === null) {
        return null;
    }
    return {
        
        'userId': value.userId,
        'episodeIds': value.episodeIds,
    };
}

