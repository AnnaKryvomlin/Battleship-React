import React, { useContext } from "react";
import { Card, Button } from "semantic-ui-react";
import { RootStoreContext } from "../../app/stores/rootStore";
import { observer } from "mobx-react-lite";
import { Link } from "react-router-dom";

const ListGames = () => {
  const rootStore = useContext(RootStoreContext);
  const { profileGames, setGameId } = rootStore.gameStore;
  const { getCurrentUserId } = rootStore.userStore;

  function CheckMove(id: number) {
    if (id === getCurrentUserId()) {
      return <Card.Content extra>Your Move</Card.Content>;
    }
  }
   
  const handleGoGame = (id: number) =>
  {
    setGameId(id);
  };

  return (
    <Card.Group>
      {profileGames.map((game) => (
        <Card fluid color="red" key={game.id}>
          <Card.Content>Game# {game.id} <Button onClick={() => handleGoGame(game.id)} content="GO" color="green" size='mini'/> </Card.Content>
          {CheckMove(game.currentMovePlayerId)}
        </Card>
      ))}
    </Card.Group>
  );
};

export default observer(ListGames);
