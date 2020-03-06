import { useState, useEffect } from "react";
import { useSelector } from "react-redux";
import { useFetch, FetchOptions } from "react-async";
import { RootState } from "../app/createRootReducer";

export const useUser = () => useSelector((state: RootState) => state.authentication.user);
export const useAccessToken = () => useSelector((state: RootState) => state.authentication.user?.access_token);

export const useAuthenticatedRequestOptions = () => {
  const accessToken = useAccessToken();

  const headers: Record<string, string> = { };
  if (accessToken != null) {
    headers['Authorization'] = `Bearer ${accessToken}`;
  }

  const options: Partial<RequestInit> = { headers };
  return options;
};

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
