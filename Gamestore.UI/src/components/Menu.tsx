import * as React from "react";
import { Nav, NavItem, NavLink, NavProps } from 'reactstrap'

export interface MenuItem {
  id: string,
  title?: string,
  img?: any,
  link: string
}

export interface MenuProps extends NavProps{
  items?: MenuItem[] | null,
  label?: string | null,
  renderItem?: React.FC<MenuItem>,
  renderEmpty?: React.FC
}

const Menu: React.FC<MenuProps> = (props) => {
  const {
    items = null,
    label = null,
    renderItem = ((item) => (<NavLink href={item.link}>{item.title}</NavLink>)),
    renderEmpty = (() => (<p></p>)),
    ...navProps
  } = props;

  const isNoItems = !items?.length;
  const noLabel = label == null;

  return isNoItems
    ? (
      <>
        {renderEmpty}
      </>
    )
    : (
      <>
        {noLabel ? `` : (<h5 className="mb-3">{label}</h5>)}
        <Nav {...navProps} >
          {items?.map((item) => (
            <NavItem key={item.id} className="me-3 mb-3">
              {renderItem(item)}
            </NavItem>
          ))}
        </Nav>
      </>
    );
}

export default Menu;