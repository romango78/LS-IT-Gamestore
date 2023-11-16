import React from 'react';
import { Container, Row, Col } from 'reactstrap'

const Games: React.FC = () => {
  return (
    <section id="games" className="gutter">
      <Container fluid>
        <Row className="g-0">
          <Col className="me-3" style={{ border: "solid" }}>
            Game List
          </Col>
          <Col className="ms-3" style={{ border: "solid" }}>
            Filter
          </Col>
        </Row>
      </Container>
    </section>
  );
}

export default Games;