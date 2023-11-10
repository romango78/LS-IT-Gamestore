import React from 'react';
import Footer from './../layouts/Footer'
import Header from './../layouts/Header';
import Main from './../layouts/Main';

import './../assets/styles/app.css';

const App: React.FC = () => {
  return (
    <div className="app container-fluid p-0">
      <Header />
      <Main />
      <Footer />
    </div>
  );
}

export default App;
