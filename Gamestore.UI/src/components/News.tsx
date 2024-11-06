import React from 'react';
import { Button } from 'reactstrap';
import { AxiosRequestConfig } from 'axios';
import useApiCall from './hooks/useApiCall';

import './../assets/styles/common.css';


const News: React.FC = () => {
  //const [appId, setAppId] = React.useState(240);
  //const [appList, setAppList] = React.useState([]);

  //function getAppList(): Promise<any> {
  //  //const headers = new Headers();
  //  //headers.append("Access-Control-Request-Method", "GET");
  //  //headers.append("Access-Control-Request-Headers", "Content-Type, Authorization");
  //  //headers.append("Content-Type", "application/json");
  //  // http://api.steampowered.com/ISteamApps/GetAppList/v2
  //  const request = new Request("https://api.steampowered.com/ISteamNews/GetNewsForApp/v0002/?appid=440&count=1&maxlength=1000&format=json", {
  //    method: "GET",
  //    mode: "no-cors",
  //    headers: {
  //      "Access-Control-Allow-Origin": "*",
  //      "Access-Control-Allow-Headers": "Origin, X-Requested-With, Content-Type, Accept",
  //      "Content-Type": "application/json"
  //    }
  //  });

  //  return fetch(request);
  //  //return axios.get("http://api.steampowered.com/ISteamApps/GetAppList/v2",
  //  //  {
  //  //    headers: {
  //  //      'Access-Control-Allow-Origin': '*',
  //  //      'Access-Control-Allow-Methods': 'GET,OPTIONS',
  //  //      'Content-Type': 'application/json'
  //  //    }
  //  //  })
  //  //  .then(response => response.data.json())
  //}

  //useApiCall({ apiCall: getAppList, setData: setAppList });

  //React.useEffect(() => {
  //  const intervalAppIdChaging = setInterval(() => {
  //    setAppId(v => v + 1);
  //  }, 60000);

  //  return () => clearInterval(intervalAppIdChaging);
  //}, []);

  const [gameId, setGameId] = React.useState('440');
  const [news, setNews] = React.useState({ 'game-id': '', 'source': '', 'title': '', 'contents': '', 'read-more': '' });
  const [newsRequest, setNewsRequest] = React.useState(getNewsRequest(gameId));

  function getNewsRequest(gameId: string): AxiosRequestConfig {
    const config: AxiosRequestConfig = {
      url: "https://ad49xs3450.execute-api.us-east-1.amazonaws.com/news",
      method: "GET",
      params: {
        gameId: gameId
      },
      headers: {
        'Content-Type': 'application/json'
      }
    };

    return config;
  }

  useApiCall({ apiRequest: newsRequest, setData: setNews, deps: [newsRequest] });

  return (
    <section className="gutter-lg">
      <p className="fs-title text-start">{news.title}</p>
      <p className="text-start mb-4" dangerouslySetInnerHTML={{ __html: news.contents }}/>
      <Button id="learn-more" type="button"
        name="learn-more" color="primary"
        className="float-start"
        href={news['read-more']} target="_blank">
        Read More
      </Button>
    </section>
  );
}

export default News;