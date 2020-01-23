/* Generated by restful-react */

import React from "react";
import { Get, GetProps, useGet, UseGetProps } from "restful-react";

export type Omit<T, K extends keyof T> = Pick<T, Exclude<keyof T, K>>;

export interface LibraryRssPodcast {
  url?: string | null;
  id?: number;
}

export type GetLibraryRssPodcastsProps = Omit<GetProps<LibraryRssPodcast[], unknown, void>, "path">;

/**
 * Get all library RSS podcasts.
 */
export const GetLibraryRssPodcasts = (props: GetLibraryRssPodcastsProps) => (
  <Get<LibraryRssPodcast[], unknown, void>
    path={`/api/LibraryRssPodcast`}
    {...props}
  />
);

export type UseGetLibraryRssPodcastsProps = Omit<UseGetProps<LibraryRssPodcast[], void>, "path">;

/**
 * Get all library RSS podcasts.
 */
export const useGetLibraryRssPodcasts = (props: UseGetLibraryRssPodcastsProps) => useGet<LibraryRssPodcast[], unknown, void>(`/api/LibraryRssPodcast`, props);


export type GetLibraryRssPodcastByIdProps = Omit<GetProps<LibraryRssPodcast, unknown, void>, "path"> & {id: number};

/**
 * Get a library RSS podcast by ID.
 */
export const GetLibraryRssPodcastById = ({id, ...props}: GetLibraryRssPodcastByIdProps) => (
  <Get<LibraryRssPodcast, unknown, void>
    path={`/api/LibraryRssPodcast/${id}`}
    {...props}
  />
);

export type UseGetLibraryRssPodcastByIdProps = Omit<UseGetProps<LibraryRssPodcast, void>, "path"> & {id: number};

/**
 * Get a library RSS podcast by ID.
 */
export const useGetLibraryRssPodcastById = ({id, ...props}: UseGetLibraryRssPodcastByIdProps) => useGet<LibraryRssPodcast, unknown, void>(`/api/LibraryRssPodcast/${id}`, props);

