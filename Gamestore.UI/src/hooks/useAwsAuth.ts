import React from 'react';
import { AxiosRequestConfig } from 'axios';
import addSigV4Auth from '../tools/awsAuth';

export interface UseAwsAuthProps {
  apiRequest: AxiosRequestConfig | null | undefined,
  setData: React.Dispatch<React.SetStateAction<any>>
}

const validateProps = (props: UseAwsAuthProps) => {
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

const useAwsAuth = (props: UseAwsAuthProps) => {
  const {
    apiRequest,
    setData
  } = props;

  const callback = React.useCallback(() => {
    // Validation props
    if (!validateProps(props)) {
      console.info("The 'useAwsAuth' was not invoked due the validation failure.");
      return;
    }

    console.info(`Invoked 'useAwsAuth' hook for ${apiRequest?.baseURL ?? ''}${apiRequest?.url ?? ''}`);
    setData(addSigV4Auth(apiRequest!));    

  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [apiRequest]);

  React.useEffect(() => {
    callback();
  }, [callback]);
}

export default useAwsAuth;