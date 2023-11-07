import './../assets/styles/app.css';

import Footer from './../layouts/Footer'
import Header from './../layouts/Header';
import Main from './../layouts/Main';

function App() {
  return (
    <div className="app container-fluid p-0">
      <Header />
      <Main />
      <Footer />
    </div>
  );
}

export default App;
