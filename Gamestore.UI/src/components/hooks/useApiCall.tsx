import React from 'react';
import axios, { AxiosError, AxiosRequestConfig } from 'axios';

export interface UseApiCallProps {
  apiRequest: AxiosRequestConfig,
  setIsLoading?: React.Dispatch<React.SetStateAction<boolean>>,
  setData: React.Dispatch<React.SetStateAction<any>>,
  deps?: React.DependencyList
};

const useApiCall = (props: UseApiCallProps) => {
  const {
    apiRequest,
    setIsLoading = undefined,
    setData,
    deps = []
  } = props;

  const request = apiRequest;

  const setIsLoadingInternal = (val: boolean) => {
    if (setIsLoading === undefined || setIsLoading === null) {
      return;
    }

    setIsLoading(val);
  }

  const callApi = async () => {
    setIsLoadingInternal(true);

    await axios(request)
      .then(response => {
        setData(response.data);
      })
      .catch(reason => {
        let error: any;
        if (reason?.isAxiosError) {
          const axiosError = reason as AxiosError;
          error = request.method + ' ' + request.url + ' ' + axiosError?.message;
        } else {
          error = reason;
        }
        console.error(error);
      });

    setIsLoadingInternal(false);
  };

  const callback = React.useCallback(() => callApi(), deps);

  React.useEffect(() => {
    callback();
  }, [callback]);  
}

export default useApiCall;