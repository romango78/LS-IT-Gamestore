import React from 'react';

import { Body, Footer, Header } from '.'

import './../assets/styles/app.css';

const App: React.FC = () => {
  return (
    <>
      <div className="app container-fluid p-0">
        <Header />
        <Body />
        <Footer />
      </div>
    </>
  );
}

export default App;
