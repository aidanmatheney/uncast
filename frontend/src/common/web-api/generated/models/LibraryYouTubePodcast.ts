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
 * @interface LibraryYouTubePodcast
 */
export interface LibraryYouTubePodcast {
    /**
     * 
     * @type {string}
     * @memberof LibraryYouTubePodcast
     */
    channelId?: string | null;
    /**
     * 
     * @type {string}
     * @memberof LibraryYouTubePodcast
     */
    id?: string;
    /**
     * 
     * @type {string}
     * @memberof LibraryYouTubePodcast
     */
    name?: string | null;
    /**
     * 
     * @type {string}
     * @memberof LibraryYouTubePodcast
     */
    author?: string | null;
    /**
     * 
     * @type {string}
     * @memberof LibraryYouTubePodcast
     */
    description?: string | null;
    /**
     * 
     * @type {string}
     * @memberof LibraryYouTubePodcast
     */
    thumbnailFileId?: string | null;
}

export function LibraryYouTubePodcastFromJSON(json: any): LibraryYouTubePodcast {
    return LibraryYouTubePodcastFromJSONTyped(json, false);
}

export function LibraryYouTubePodcastFromJSONTyped(json: any, ignoreDiscriminator: boolean): LibraryYouTubePodcast {
    if ((json === undefined) || (json === null)) {
        return json;
    }
    return {
        
        'channelId': !exists(json, 'channelId') ? undefined : json['channelId'],
        'id': !exists(json, 'id') ? undefined : json['id'],
        'name': !exists(json, 'name') ? undefined : json['name'],
        'author': !exists(json, 'author') ? undefined : json['author'],
        'description': !exists(json, 'description') ? undefined : json['description'],
        'thumbnailFileId': !exists(json, 'thumbnailFileId') ? undefined : json['thumbnailFileId'],
    };
}

export function LibraryYouTubePodcastToJSON(value?: LibraryYouTubePodcast | null): any {
    if (value === undefined) {
        return undefined;
    }
    if (value === null) {
        return null;
    }
    return {
        
        'channelId': value.channelId,
        'id': value.id,
        'name': value.name,
        'author': value.author,
        'description': value.description,
        'thumbnailFileId': value.thumbnailFileId,
    };
}

