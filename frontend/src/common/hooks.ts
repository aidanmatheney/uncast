import { useState, useEffect, useRef } from 'react';
import { useFetch, FetchOptions } from 'react-async';

export const useResponseText = (response: Response | null | undefined) => {
  const [text, setText] = useState<string | undefined>(undefined);
  useEffect(() => {
    setText(undefined);
    if (response != null) {
      response.text().then(setText);
    }
  }, [response]);

  return text;
};

export const useTextFetch = (
  resource: RequestInfo,
  init: RequestInit,
  options?: FetchOptions<Response>
) => {
  if (options?.json === true) {
    throw new Error('options.json must be false or undefined');
  }

  const state = useFetch(resource, init, { ...options, json: false });
  const text = useResponseText(state.data);

  return {
    ...state,
    isLoading: state.isLoading || text == null,
    text
  };
};

export const useInterval = (callback: () => void, delay: number | null) => {
  const savedCallback = useRef<typeof callback>();

  // Remember the latest callback.
  useEffect(() => savedCallback.current = callback, [callback]);

  // Set up the interval.
  useEffect(() => {
    if (delay !== null) {
      const id = setInterval(() => savedCallback.current!(), delay);
      return () => clearInterval(id);
    }
  }, [delay]);
};
