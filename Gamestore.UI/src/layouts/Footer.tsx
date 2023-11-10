import React from 'react'
import { Row, Col, NavLink } from 'reactstrap'
import { MenuItem, Menu } from './../components'

import './../assets/styles/footer.css'
import { siteLinksMenuItems, companyInfoMenuItems, socialMediaMenuItems } from '../assets/Links'

const Footer: React.FC = () => {
  return (
    <footer className="app-footer">
      <div className="container-fluid context-area text-start">
        <Row>
          <Col>
            <Menu vertical label="Site Links"
              items={siteLinksMenuItems}
              renderItem={(item: MenuItem) => (
                <NavLink href={item.link} className="p-0 text-reset">{item.title}</NavLink>
                )} />
          </Col>
          <Col>
            <Menu vertical label="Company Information"
              items={companyInfoMenuItems}
              renderItem={(item: MenuItem) => (
                <NavLink href={item.link} className="p-0 text-reset">{item.title}</NavLink>
                )} />
          </Col>
          <Col>
            <Menu vertical className="flex-md-row" label="Social Media"
              items={socialMediaMenuItems}
              renderItem={(item: MenuItem) => (
                <NavLink href={item.link} className="p-0">{<item.img />}</NavLink>
                )} />
          </Col>
        </Row>
        <div className="row copyright">
          <div className="col">
            &copy; 2007 - {new Date().getFullYear()} All Rights Reserved
          </div>
        </div>
      </div>
    </footer>   
  );
}

export default Footer;