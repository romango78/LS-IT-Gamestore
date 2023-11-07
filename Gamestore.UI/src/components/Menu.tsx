import { Nav, NavItem, NavLink } from 'reactstrap'

export interface IMenuItem {
  id: string,
  title?: string,
  img?: any,
  link: string
};

interface IMenuItemsParams {
  items?: IMenuItem[],
  renderItem: any
}

interface IMenuParams {
  label?: string,
  items?: IMenuItem[],
  renderItem?: any,
  renderEmpty?: any,
  [prop: string]: any
}

function MenuItems({ items, renderItem }: IMenuItemsParams) {
  return (
    <>
      {items?.map((item) => (
        <NavItem key={item.id} className="me-3 mb-3">
          {renderItem(item)}
        </NavItem>
      ))}
    </>
  );
};

export const Menu = ({ label, items, renderItem, renderEmpty, ...props }: IMenuParams) => {
  const isNoItems = !items?.length;
  const noLabel = label == null;

  return isNoItems
    ? renderEmpty
    : (
      <>
        {noLabel ? `` : (<h5 className="mb-3">{label}</h5>)}
        <Nav {...props} >
          <MenuItems items={items}
            renderItem={renderItem} />
        </Nav>
      </>
    );
};

Menu.defaultProps = {
  label: null,
  items: null,
  renderItem: ((item: IMenuItem) => (<NavLink href={item.link}>{item.title}</NavLink>)),
  renderEmpty: (<p></p>)
};