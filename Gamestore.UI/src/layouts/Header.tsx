import React from 'react'
import { useLocation } from 'react-router-dom'
import { Col, Row, NavLink, Container, Navbar, NavbarBrand, NavbarToggler, Collapse } from 'reactstrap'
import { Menu, MenuItem } from './../components'

import { mainMenuItems } from './../assets/Links'
import './../assets/styles/header.css'
import background from './../assets/images/header-background.png'

const Header: React.FC = () => {
  const [collapsed, setCollapsed] = React.useState(true);
  const [titleClass, setTitleClass] = React.useState("g-0 h-100 align-items-end");
  const currentLocation = useLocation();

  const toggleNavbar = () => {
    setCollapsed(!collapsed);
    setTitleClass(!collapsed ? "h-100 align-items-end" : "");
  }

  return (
    <header className="app-header" style={{
      background: `linear-gradient(rgba(0,0,0,0.4), rgba(0,0,0,0.4)),url(${background})`,
      backgroundRepeat: `no-repeat`,
      backgroundSize: `cover`
    }}>
      <Container fluid className="context-area">
        <Navbar dark container={false} expand="lg">
          <NavbarBrand href="/" className="logo text-reset">logo</NavbarBrand>
          <NavbarToggler onClick={toggleNavbar} />
          <Collapse navbar isOpen={!collapsed} className="justify-content-end">
            <Menu navbar
              items={mainMenuItems}
              renderItem={(item: MenuItem) => (
                <NavLink href={item.link}
                  className="text-end text-uppercase"
                  active={currentLocation.pathname === item.link} >
                  {item.title}
                </NavLink>
                )} />          
          </Collapse>
        </Navbar>
        <Container className="g-0 h-100">
          <Row className={titleClass} >
          <Col>
            <div className="title text-start">
              <span>What Can Make Your<br />Life Easier?</span>
            </div>
            <div className="subtitle mt-2 text-start">
              <span>Empowered Solutions with Simple Process</span>
            </div>
          </Col>
          </Row>
        </Container>
      </Container>
    </header>
  );
}

export default Header;