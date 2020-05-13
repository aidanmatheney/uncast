import { useState, useEffect, useRef } from 'react';
import { useSelector } from 'react-redux';
import { useFetch, FetchOptions } from 'react-async';
import { RootState } from '../app/createRootReducer';

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


export function useInterval(callback: () => void, delay: number | null) {
  const savedCallback = useRef<() => void>();

  // Remember the latest callback.
  useEffect(() => {
    savedCallback.current = callback;
  }, [callback]);

  // Set up the interval.
  useEffect(() => {
    function tick() {
      savedCallback.current!();
    }
    if (delay !== null) {
      let id = setInterval(tick, delay);
      return () => clearInterval(id);
    }
  }, [delay]);
}
