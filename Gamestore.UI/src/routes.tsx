import { createBrowserRouter } from "react-router-dom";
import { App, Main, Games, Genres, Publishers, Orders, Cart } from './layouts/';

export const router = createBrowserRouter([
  {
    element: <App />,
    children: [
      { path: "/", element: <Main /> },
      { path: "/games", element: <Games /> },
      { path: "/genres", element: <Genres /> },
      { path: "/publishers", element: <Publishers /> },
      { path: "/orders", element: <Orders /> },
      { path: "/cart", element: <Cart /> },
      { path: "*", element: <Main /> }
    ]
  }
]);