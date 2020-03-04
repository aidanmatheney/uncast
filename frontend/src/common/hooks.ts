import { useSelector } from "react-redux";
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
