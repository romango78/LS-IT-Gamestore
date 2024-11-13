import React from 'react';
import { AxiosRequestConfig } from 'axios';
import { SignatureV4 } from '@smithy/signature-v4';
import { HttpRequest, IHttpRequest } from '@smithy/protocol-http';
import { AwsCredentialIdentity, HeaderBag } from '@smithy/types';
import { Sha256 } from "@aws-crypto/sha256-js";

const service = 'execute-api';
const region = process.env.REACT_APP_AWS_REGION;

export interface UseAwsAuthProps {
  apiRequest: AxiosRequestConfig,
  setData: React.Dispatch<React.SetStateAction<any>>
}

const credentials: AwsCredentialIdentity = {
  accessKeyId: process.env.REACT_APP_AWS_ACCESS_KEY!,
  secretAccessKey: process.env.REACT_APP_AWS_SECRET_KEY!,
  sessionToken: ''
}

const toAxiosRequestConfig = (smithyRequest: IHttpRequest) => {
  const axiosConfig: AxiosRequestConfig = {
    url: `${smithyRequest.protocol}//${smithyRequest.hostname}${smithyRequest.path}`,
    method: smithyRequest.method.toUpperCase(),
    headers: smithyRequest.headers
  };

  if (smithyRequest.body) {
    axiosConfig.data = smithyRequest.body;
  }
  // Include query parameters if present
  //if (smithyRequest.query) {
  //  axiosConfig.params = Object.fromEntries(smithyRequest.query);
  //}

  return axiosConfig;
}

const toSmithyRequest = (requestConfig: AxiosRequestConfig) => {
  const { url, method, headers, data } = requestConfig;

  // Convert Axios headers to HeaderBag
  const smithyHeaders: HeaderBag = Object.entries(headers!).reduce((acc: HeaderBag, [key, value]) => {
    acc[key] = String(value);
    return acc;
  }, {});

  return new HttpRequest({
    hostname: new URL(url!).hostname,
    path: new URL(url!).pathname,
    protocol: new URL(url!).protocol,
    method: method,
    headers: smithyHeaders,    
    body: data
  });
}

const validateProps = (props: UseAwsAuthProps) => {
  const {
    apiRequest
  } = props;

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
    if (!validateProps(props))
      return;

    const canonicalRequest = toSmithyRequest(apiRequest);
   
    const configureAwsAuth = async () => {
      // Create the signer
      const signer = new SignatureV4({
        credentials: credentials,
        region: region!,
        service: service,
        sha256: Sha256
      });

      // Sign the request
      const signedRequest = await signer.sign(canonicalRequest);

      return toAxiosRequestConfig(signedRequest);
    }

    configureAwsAuth()
      .then(request => {
        setData(request)
      })
      .catch(reason => {
        console.error(reason);
      });
    
  }, [apiRequest, setData]);

  React.useEffect(() => {
    callback();
  }, [callback]);
}

export default useAwsAuth;