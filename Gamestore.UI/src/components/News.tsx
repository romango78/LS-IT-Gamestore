import React from 'react';
import { Button } from 'reactstrap';
import axios from 'axios';
import useApiCall from './hooks/useApiCall';

import './../assets/styles/common.css';


const News: React.FC = () => {
  const [appId, setAppId] = React.useState(240);
  const [appList, setAppList] = React.useState([]);

  function getAppList(): Promise<any> {
    //const headers = new Headers();
    //headers.append("Access-Control-Request-Method", "GET");
    //headers.append("Access-Control-Request-Headers", "Content-Type, Authorization");
    //headers.append("Content-Type", "application/json");
    // http://api.steampowered.com/ISteamApps/GetAppList/v2
    const request = new Request("https://api.steampowered.com/ISteamNews/GetNewsForApp/v0002/?appid=440&count=1&maxlength=1000&format=json", {
      method: "GET",
      mode: "no-cors",
      headers: {
        "Access-Control-Allow-Origin": "*",
        "Access-Control-Allow-Headers": "Origin, X-Requested-With, Content-Type, Accept",
        "Content-Type": "application/json"
      }
    });

    return fetch(request);
    //return axios.get("http://api.steampowered.com/ISteamApps/GetAppList/v2",
    //  {        
    //    headers: {
    //      'Access-Control-Allow-Origin': '*',
    //      'Access-Control-Allow-Methods': 'GET,OPTIONS',
    //      'Content-Type': 'application/json'
    //    }
    //  })
    //  .then(response => response.data.json())
  }

  useApiCall({ apiCall: getAppList, setData: setAppList });

  React.useEffect(() => {
    const intervalAppIdChaging = setInterval(() => {
      setAppId(v => v + 1);
    }, 60000);

    return () => clearInterval(intervalAppIdChaging);
  }, []);



  return (
    <section className="gutter-lg">
      <p className="fs-title text-start">Title</p>
      <p className="text-start mb-4">
        {appId} Donec urna sapien, porta nec condimentum id, elementum in quam. Curabitur a aliquet turpis, sagittis eleifend sem. Nam quis est eget neque euismod fringilla et sit amet mauris. Proin commodo sodales arcu, a volutpat augue facilisis et. Nam pellentesque finibus felis eget feugiat. Nulla convallis a nisl non vehicula. Aliquam erat volutpat. Vivamus id massa a lacus tincidunt tempor at eu ex...
      </p>
      <p>
        {appList}
      </p>
      <Button id="learn-more" type="button"
        name="learn-more" color="primary"
        className="float-start">
        Learn More
      </Button>
    </section>
  );
}

export default News;