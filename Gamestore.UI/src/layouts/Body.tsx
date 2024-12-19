import React from 'react';
import { Outlet } from 'react-router-dom'
import { Container, Row, Col } from 'reactstrap'
import { GetInTouch } from '../components'

import './../assets/styles/common.css';

const Body: React.FC = () => {
  return (
    <section id="body">
      <Container fluid className="context-area">
        <Row className="g-0">
          <Outlet />
        </Row>
        <Row xl="3" xs="1" className="g-0">
          <Col className="pe-xl-3">
            <GetInTouch />
          </Col>
          <Col className="ps-xl-3 pe-xl-3">
            <section className="fluid gutter-xl" style={{ border: "solid" }}>
              Twitter Updates
            </section>
          </Col>
          <Col className="ps-xl-3">
            <section className="fluid" style={{ border: "solid" }}>
              From the Blog
            </section>
          </Col>
        </Row>
      </Container>
    </section>
  );
}

export default Body;