import React from 'react';
import { Container, Row, Col } from 'reactstrap'

const Genres: React.FC = () => {
  return (
    <section id="games" className="gutter">
      <Container fluid>
        <Row className="g-0">
          <Col className="me-3" style={{ border: "solid" }}>
            Genre List
          </Col>
          <Col className="ms-3" style={{ border: "solid" }}>
            Filter
          </Col>
        </Row>
      </Container>
    </section>
  );
}

export default Genres;