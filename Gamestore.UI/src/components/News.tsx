import React from 'react';
import { Button } from 'reactstrap';
import { AxiosRequestConfig } from 'axios';
import { AvaliableGame, GameNews } from '../models';
import addSigV4Auth from '../tools/awsAuth';
import useApiCall from '../hooks/useApiCall';

import { aws_backend_url } from '../assets/backend';
import './../assets/styles/common.css';

const NoNews: GameNews = { 'game-id': '', 'source': '', 'title': 'News', 'contents': 'Comming soon...', 'read-more': '' };

const News: React.FC = () => {

  function getAvailableGamesRequest(): AxiosRequestConfig | null {
    const config: AxiosRequestConfig = {
      baseURL: aws_backend_url,
      url: "/availablegames",
      method: "GET",
      headers: {
        'Content-Type': 'application/json'
      }
    };

    return addSigV4Auth(config);
  }

  function getNewsRequest(gameId: string | null): AxiosRequestConfig | null {
    if (gameId === null) {
      return null;
    }

    const config: AxiosRequestConfig = {
      baseURL: aws_backend_url,
      url: "/news",
      method: "GET",
      params: {
        gameId: gameId
      },
      headers: {
        'Content-Type': 'application/json'
      }
    };

    return addSigV4Auth(config);
  }

  const [availableGamesRequest] = React.useState(getAvailableGamesRequest());
  const [availableGames, setAvailableGames] = React.useState([] as AvaliableGame[]);
  const [newsRequest, setNewsRequest] = React.useState(null as AxiosRequestConfig | null);
  const [news, setNews] = React.useState(NoNews);

  useApiCall({ apiRequest: availableGamesRequest, setData: setAvailableGames, limit: 10000 }); 
  useApiCall({ apiRequest: newsRequest, setData: setNews, limit: 10000 });

  React.useEffect(() => {
    function getNews() {
      const count = availableGames.length;
      if (count === 0)
        return;
      const index = Math.floor(Math.random() * count);
      const gameId = availableGames[index]['game-id'];

      console.info(`Selected game '${gameId}':'${index}' for downloading News`);

      const request = getNewsRequest(gameId);
      if (request !== null)
        setNewsRequest(request);
    };

    if (availableGames === null || availableGames.length === 0)
      return;

    getNews();

    const intervalChaging = setInterval(() => getNews(), 60000);

    return () => clearInterval(intervalChaging);
  }, [availableGames]); 

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