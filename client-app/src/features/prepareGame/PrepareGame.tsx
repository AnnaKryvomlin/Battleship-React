import React, { Fragment, useContext, useEffect } from "react";
import Board from "./Board";
import { RootStoreContext } from "../../app/stores/rootStore";
import { Grid, Button } from "semantic-ui-react";
import { observer } from "mobx-react-lite";

const PrepareGame = () => {
  const rootStore = useContext(RootStoreContext);
  const { submitting, createNewGame, findGame } = rootStore.gameStore;

  const handleCreateGame = () => {
    createNewGame();
  };

  const handleFindGame = () => {
    findGame();
  };

  return (
    <Grid>
      <Grid.Column width={10}>
        <Board />
      </Grid.Column>
      <Grid.Column width={6}>
        <Button
          positive
          loading={submitting}
          content="Create Game"
          onClick={() => handleCreateGame()}
        />
        <Button
          primary
          loading={submitting}
          content="Find Game"
          onClick={() => handleFindGame()}
        />
      </Grid.Column>
    </Grid>
  );
};

export default observer(PrepareGame);
