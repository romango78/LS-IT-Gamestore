import React from 'react';
import { useFormik } from 'formik';

const useGetInTouch = () => {
  const formik = useFormik({
    initialValues: {
      name: '',
      email: '',
      message: ''
    },
    onSubmit: (values) => {
      alert("Name: " + values.name + "; Email: " + values.email + "; Message: " + values.message);

      formik.setValues({ name: '', email: '', message: '' });
    }
  });

  const model = formik.values;
  const handleChange = formik.handleChange;
  const handleSubmit = formik.handleSubmit;

  return {
    model,
    handleChange,
    handleSubmit
  };
};

export default useGetInTouch;