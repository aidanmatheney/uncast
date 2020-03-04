import { useGet } from 'restful-react';
import { UseGetProps } from 'restful-react/lib/useGet';
import { useAuthenticatedRequestOptions } from '../hooks';

export const useAuthenticatedGet = <
  TData = any,
  TError = any,
  TQueryParams = { [key: string]: any; }
>(
  path: string,
  props?: Omit<UseGetProps<TData, TQueryParams>, "path">
) => {
  const requestOptions = useAuthenticatedRequestOptions();

  return useGet(path, {
    ...props,
    requestOptions
  });
}
