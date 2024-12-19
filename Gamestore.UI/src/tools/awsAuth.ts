import { AxiosRequestConfig } from 'axios';
import moment from 'moment';
import crypto from 'crypto-js';

const service = 'execute-api';
const region = process.env.REACT_APP_AWS_REGION;

const awsCredentials = {
  accessKeyId: process.env.REACT_APP_AWS_ACCESS_KEY!,
  secretAccessKey: process.env.REACT_APP_AWS_SECRET_KEY!,
}

const createHash = (input: any) => {
  return crypto.enc.Hex.stringify(crypto.SHA256(input));
}

const getSignedHeaders = (credentials: any, region: any, service: any, axiosConfig: AxiosRequestConfig) => {
  const { baseURL, url, method, headers, params, data } = axiosConfig;

  let host;
  let path;
  if (!baseURL) {
    host = new URL(url!).hostname;
    path = new URL(url!).pathname;
  } else {
    host = new URL(baseURL! + url!).hostname;
    path = new URL(baseURL! + url!).pathname;
  }

  // 0. Validate Request
  if (!method || !host) {
    return headers;
  }

  // 1. Create a canonical request for Signature Version 4
  // Arrange the contents of your request (host, action, headers, etc.) 
  // into a standard(canonical) format.The canonical request is one of 
  // the inputs used to create a string to sign.
  const t = moment().utc()
  const { accessKeyId, secretAccessKey } = credentials

  const amzDate = `${t.format("YYYYMMDDTHHmmss")}Z`;
  const httpRequestMethod = method.toUpperCase();
  const canonicalURI = encodeURI(path);
  const canonicalQueryString = params ?
    Object.keys(params)
      .map(key => `${encodeURI(key)}=${encodeURI(params[key])}`)
      .join('&')
    : '';
  const canonicalHeaders = `host:${host}\nx-amz-date:${amzDate}\n`;
  const signedHeaders = 'host;x-amz-date';
  const payload = data ? JSON.stringify(data) : '';
  const hashedPayload = createHash(payload);

  const canonicalRequest =
    `${httpRequestMethod}\n${canonicalURI}\n${canonicalQueryString}\n${canonicalHeaders}\n${signedHeaders}\n${hashedPayload}`

  const hashedCanonicalRequest = createHash(canonicalRequest);

  // 2. Create a string to sign for Signature Version 4
  // Create a string to sign with the canonical request and extra information such as the algorithm, 
  // request date, credential scope, and the digest(hash) of the canonical request.
  const algorithm = 'AWS4-HMAC-SHA256';
  const requestDateTime = amzDate;
  const dateStamp = t.format('YYYYMMDD'); // Date w/o time, used in credential scope
  const credentialScope = `${dateStamp}/${region}/${service}/aws4_request`;

  const stringToSign = `${algorithm}\n${requestDateTime}\n${credentialScope}\n${hashedCanonicalRequest}`;

  // 3. Calculate the signature for AWS Signature Version 4
  // Derive a signing key by performing a succession of keyed hash operations 
  // (HMAC operations) on the request date, Region, and service, with your AWS 
  // secret access key as the key for the initial hashing operation.
  // After you derive the signing key, you then calculate the signature by performing 
  // a keyed hash operation on the string to sign.Use the derived signing key as the hash key for this operation.

  var kDate = crypto.HmacSHA256(dateStamp, "AWS4" + secretAccessKey);
  var kRegion = crypto.HmacSHA256(region, kDate);
  var kService = crypto.HmacSHA256(service, kRegion);
  var kSigning = crypto.HmacSHA256("aws4_request", kService);

  const signature = crypto.enc.Hex.stringify(crypto.HmacSHA256(stringToSign, kSigning));

  // 4. Add the signature to the HTTP request
  // After you calculate the signature, add it to an HTTP header or to the query string of the request.

  const authorizationHeader = `${algorithm} Credential=${accessKeyId}/${credentialScope}, SignedHeaders=${signedHeaders}, Signature=${signature}`;

  const updatedHeaders = {
    'X-Amz-Date': amzDate,
    'Authorization': authorizationHeader,
    //'Host': host, // Warning: Refused to set unsafe header "Host"
    ...headers
  };

  return updatedHeaders;
}

export default function addSigV4Auth(requestConfig: AxiosRequestConfig | null) {
  if (requestConfig === null)
    return requestConfig;

  const updatedHeaders = getSignedHeaders(awsCredentials, region, service, requestConfig);

  requestConfig.headers = updatedHeaders;

  return requestConfig;
}