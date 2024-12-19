const aws_region = process.env.REACT_APP_AWS_REGION;
const aws_httpapi_ref = process.env.REACT_APP_AWS_HTTPAPI_REF;

export const aws_backend_url = `https://${aws_httpapi_ref}.execute-api.${aws_region}.amazonaws.com`;