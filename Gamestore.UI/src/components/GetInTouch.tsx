import React from 'react';
import { Form, FormGroup, Label, Input, Button } from 'reactstrap'
import useGetInTouch from './hooks/GetInTouchHooks'

import './../assets/styles/common.css';

const GetInTouch: React.FC = () => {
  const { model, handleChange, handleSubmit } = useGetInTouch();

  return (
    <section id="get-in-touch" className="fluid gutter-xl">
      <p className="fs-title">Get In Touch</p>
      <Form id="get-in-touch-form" tag="form" onSubmit={handleSubmit} >
        <FormGroup className="mb-formgroup">
          <Label for="get-in-touch-name">
            Name
          </Label>
          <Input id="get-in-touch-name" name="name" type="text"
            value={model.name}
            onChange={handleChange} required />
        </FormGroup>
        <FormGroup className="mb-formgroup">
          <Label for="get-in-touch-email">
            Email
          </Label>
          <Input id="get-in-touch-email" name="email" type="email"
            value={model.email}
            onChange={handleChange} required />
        </FormGroup>
        <FormGroup className="mb-formgroup">
          <Label for="get-in-touch-message">
            Message
          </Label>
          <Input id="get-in-touch-message" name="message" type="textarea"
            value={model.message}
            onChange={handleChange} required />
        </FormGroup>
        <div className="clearfix">
          <Button id="get-in-touch-submit-btn" name="submit" type="submit" color="primary" className="float-end">
            Submit
          </Button>
        </div>
      </Form>
    </section>
  );
}

export default GetInTouch;