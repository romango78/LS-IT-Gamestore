import React from 'react';
import { Container, Row, Col } from 'reactstrap'

const Cart: React.FC = () => {
  return (
    <section id="orders" className="gutter">
      <Container fluid>
        <Row className="g-0">
          <Col className="me-3" style={{ border: "solid" }}>
            Cart
          </Col>
          <Col className="ms-3" style={{ border: "solid" }}>
            Total
          </Col>
        </Row>
      </Container>
    </section>
  );
}

export default Cart;