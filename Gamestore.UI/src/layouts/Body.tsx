import React from 'react';
import { Routes, Route } from 'react-router-dom'
import { Container, Row, Col } from 'reactstrap'
import { GetInTouch } from '../components'
import { Main, Games, Genres, Publishers, Orders, Cart } from './'

import './../assets/styles/common.css';

const Body: React.FC = () => {
  return (
    <section id="body">
      <Container fluid className="context-area">
        <Row className="g-0">          
            <Routes>
              <Route path="/" element={<Main />} />
              <Route path="/games" element={<Games />} />
              <Route path="/genres" element={<Genres />} />
              <Route path="/publishers" element={<Publishers />} />
              <Route path="/orders" element={<Orders />} />
              <Route path="/cart" element={<Cart />} />
            </Routes>          
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