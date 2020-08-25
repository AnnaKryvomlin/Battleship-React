import React, { useContext, useEffect, Fragment } from "react";
import { Grid, Button } from "semantic-ui-react";
import MyBoard from "./MyBoard";
import { RootStoreContext } from "../../app/stores/rootStore";
import { observer } from "mobx-react-lite";
import EnemyBoard from "./EnemyBoard";
import MoveList from "./MoveList";
import { history } from "../..";

const Game = () => {
  const rootStore = useContext(RootStoreContext);
  const {
    loadMyCoords,
    loadEnemyCoords,
    getGameRecords,
    createHubConnection,
    stopHubConnection,
    finished,
    finishedMessage,
    stopGame
  } = rootStore.gameStore;

    const { user } = rootStore.userStore;

  useEffect(() => {
    loadMyCoords();
    loadEnemyCoords();
    getGameRecords();
    createHubConnection();
    return () => {
      stopHubConnection();
    };
  }, [
    loadMyCoords,
    loadEnemyCoords,
    getGameRecords,
    finished
  ]);

  if(finished) {
    alert(finishedMessage);
    history.push(`/profile/${user!.userName}`);
  }

  const handleStopGame = () =>
  {
    stopGame();
  };

  return (
    <Fragment>
      <Grid>
        <Grid.Column width={8}>
          <MyBoard />
        </Grid.Column>
        <Grid.Column width={8}>
          <EnemyBoard />
        </Grid.Column>
      </Grid>
      <Button basic color='grey' content='Stop It' onClick={() => handleStopGame()}/>
      <MoveList />
    </Fragment>
  );
};

export default observer(Game);
