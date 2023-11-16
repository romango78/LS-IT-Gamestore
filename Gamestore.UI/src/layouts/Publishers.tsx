import React from 'react';
import { Container, Row, Col } from 'reactstrap'

const Publishers: React.FC = () => {
  return (
    <section id="publishers" className="gutter">
      <Container fluid>
        <Row className="g-0">
          <Col className="me-3" style={{ border: "solid" }}>
            Publisher List
          </Col>
          <Col className="ms-3" style={{ border: "solid" }}>
            Filter
          </Col>
        </Row>
      </Container>
    </section>
  );
}

export default Publishers;