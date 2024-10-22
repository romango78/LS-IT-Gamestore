import React from 'react';

export interface UseApiCallProps {
  apiCall: () => Promise<any>,
  setData: React.Dispatch<React.SetStateAction<any>>,
  once?: boolean
};

const useApiCall = (props: UseApiCallProps) => {
  const {
    apiCall,
    setData,
    once = false
  } = props;
  
  React.useEffect(() => {
    apiCall()
      .then((response) => {
        return response.json()
      })
      .then(data => {
        setData(data)
      })
      .catch(error => {
        console.error(error);
      });
  }, []);
  
}

export default useApiCall;