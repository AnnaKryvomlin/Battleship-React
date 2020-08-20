import React, { useContext } from "react";
import { Card } from "semantic-ui-react";
import { RootStoreContext } from "../../app/stores/rootStore";
import { observer } from "mobx-react-lite";

const ListGames = () => {
  const rootStore = useContext(RootStoreContext);
  const { profileGames } = rootStore.gameStore;
  const { user, getCurrentUserId } = rootStore.userStore;

  function CheckMove(id: number) {
   if (id === getCurrentUserId()) {
   return <Card.Content extra>Your Move</Card.Content>
}
  }


  return (
    <Card.Group>
      {profileGames.map((game) => (
        <Card fluid color="red" key={game.id}>
          <Card.Content>Game# {game.id} </Card.Content>
          {CheckMove(game.currentMovePlayerId)}
        </Card>
      ))}
    </Card.Group>
  );
};

export default observer(ListGames);
