import React from 'react';
import { Container, Row, Col } from 'reactstrap'

import './../assets/styles/main.css';

import GetInTouch from '../components/GetInTouch'

function Main() {
  return (
    <section id="main">
      <Container fluid className="context-area">
        <Row className="g-0">
          <section className="gutter" style={{ border: "solid" }} >
            Main Content
          </section>
        </Row>
        <Row xl="3" xs="1" className="g-0">
          <Col className="pe-xl-3">
            <GetInTouch/>
          </Col>
          <Col className="ps-xl-3 pe-xl-3">
            <section className="fluid gutter" style={{ border: "solid" }}>
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

export default Main;