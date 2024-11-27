import React from 'react';
import axios, { AxiosError, AxiosRequestConfig } from 'axios';

export interface UseApiCallProps {
  apiRequest: AxiosRequestConfig | null | undefined,
  setIsLoading?: React.Dispatch<React.SetStateAction<boolean>>,
  setData: React.Dispatch<React.SetStateAction<any>>
};

const validateProps = (props: UseApiCallProps) => {
  const {
    apiRequest
  } = props;

  if (!apiRequest)
    return false;

  if (apiRequest.url === undefined)
    return false;

  if (apiRequest.method === undefined)
    return false;

  return true;
};

const useApiCall = (props: UseApiCallProps) => {
  const {
    apiRequest,
    setIsLoading = undefined,
    setData
  } = props;

  const callback = React.useCallback(() => {

    const setIsLoadingInternal = (val: boolean) => {
      if (setIsLoading === undefined || setIsLoading === null) {
        return;
      }

      setIsLoading(val);
    }

    // Validation props
    if (!validateProps(props)) {
      console.info("The 'useApiCall' was not invoked due the validation failure.");
      return;
    }

    const callApi = async () => {
      setIsLoadingInternal(true);

      await axios(apiRequest!)
        .then(response => {
          console.info(`Invoked 'useApiCall' hook for ${apiRequest?.baseURL ?? ''}${apiRequest?.url ?? ''}`);
          setData(response.data);
        })
        .catch(reason => {
          let error: any;
          if (reason?.isAxiosError) {
            const axiosError = reason as AxiosError;
            error = apiRequest!.method + ' ' + apiRequest!.url + ' ' + axiosError?.message;
          } else {
            error = reason;
          }
          console.error(error);
        });
      setIsLoadingInternal(false);
    };

    callApi();
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [apiRequest]);

  React.useEffect(() => {
    callback();
  }, [callback]);  
}

export default useApiCall;