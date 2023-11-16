import React from 'react';
import { Container, Row, Col } from 'reactstrap'
import { News } from '../components';

import './../assets/styles/common.css';

const Main: React.FC = () => {
  return (
    <section id="main" className="gutter">
      <Container fluid>
        <Row lg="2" xs="1" className="g-0">
          <Col className="pe-lg-5 col-lg-8">
            <News />
          </Col>
          <Col className="ps-lg-3 col-lg-4">
            <section style={{ border: "solid" }}>
              Banner
            </section>
          </Col>
        </Row>
      </Container>
    </section>
  );
}

export default Main;