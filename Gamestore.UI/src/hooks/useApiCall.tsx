import React from 'react';
import axios, { AxiosError, AxiosRequestConfig } from 'axios';

export interface UseApiCallProps {
  apiRequest: AxiosRequestConfig | null | undefined,
  setIsLoading?: React.Dispatch<React.SetStateAction<boolean>>,
  setData: React.Dispatch<React.SetStateAction<any>>,
  limit?: number
};

const validateProps = (apiRequest: AxiosRequestConfig | null | undefined) => {
  if (!apiRequest)
    return false;

  if (apiRequest.url === undefined)
    return false;

  if (apiRequest.method === undefined)
    return false;

  return true;
};

const getRequestInfo = (apiRequest: AxiosRequestConfig | null | undefined) => {
  return `${apiRequest?.baseURL ?? ''}${apiRequest?.url ?? ''}`;
}

const useApiCall = (props: UseApiCallProps) => {
  const {
    apiRequest,
    setIsLoading = undefined,
    setData,
    limit = 0
  } = props; 

  const lastCallRef = React.useRef(0);

  const throttledCallback = React.useCallback(async (): Promise<any> => {
    const setIsLoadingInternal = (val: boolean) => {
      if (setIsLoading === undefined || setIsLoading === null) {
        return;
      }

      setIsLoading(val);
    }

    const callApi = async (apiRequest: AxiosRequestConfig | null | undefined) => {
      console.info(`The 'useApiCall' hook invoked for ${getRequestInfo(apiRequest)}.`);

      // Make API Call
      try {
        const response = await axios(apiRequest!);
        console.info(`The 'useApiCall' hook completed for ${getRequestInfo(apiRequest)}`);
        return response.data;
      } catch (reason) {
        let error: any;
        if ((reason as any)?.isAxiosError) {
          const axiosError = reason as AxiosError;
          error = `${apiRequest!.method} ${getRequestInfo(apiRequest)} ${axiosError?.message}.`;
        } else {
          error = reason;
        }
        throw error;
      }
    }

    const now = Date.now();
    if (now - lastCallRef.current >= limit) {
      lastCallRef.current = now;

      // Set Is Loading
      setIsLoadingInternal(true);

      await callApi(apiRequest)
        .then(data => {
            setData(data);
            setIsLoadingInternal(false);
        })
        .catch(error => {
          console.error(error);
          setIsLoadingInternal(false);
        });
    }
  }, [apiRequest, limit, setData, setIsLoading]);

  React.useEffect(() => {
    // Validation props
    if (!validateProps(apiRequest)) {
      console.warn(`The 'useApiCall' hook failed for ${getRequestInfo(apiRequest)} due the validation errors.`);
      return;
    } 

    throttledCallback();
  }, [apiRequest, throttledCallback]);  
}

export default useApiCall;