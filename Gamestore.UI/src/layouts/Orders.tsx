import React from 'react';
import { Container, Row, Col } from 'reactstrap'

const Orders: React.FC = () => {
  return (
    <section id="orders" className="gutter">
      <Container fluid>
        <Row className="g-0">
          <Col className="me-3" style={{ border: "solid" }}>
            Orders List
          </Col>
          <Col className="ms-3" style={{ border: "solid" }}>
            Filter
          </Col>
        </Row>
      </Container>
    </section>
  );
}

export default Orders;