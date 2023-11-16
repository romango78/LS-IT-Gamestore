import React from 'react';
import { BrowserRouter } from 'react-router-dom'
import { Body, Footer, Header } from './../layouts'

import './../assets/styles/app.css';

const App: React.FC = () => {
  return (
    <BrowserRouter>
      <div className="app container-fluid p-0">
        <Header />
        <Body />
        <Footer />
      </div>
    </BrowserRouter>
  );
}

export default App;
