import React, { Fragment, useContext, useEffect } from "react";
import Board from "./Board";
import { RootStoreContext } from "../../app/stores/rootStore";
import { Grid, Button } from "semantic-ui-react";

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
<Board/>
</Grid.Column>
<Grid.Column width={6}>
  <Button positive loading={submitting}  content='Create Game' onClick={() => handleCreateGame()}/>
  <Button primary loading={submitting}  content='Find Game' onClick={() => handleFindGame()}/>
</Grid.Column>
    </Grid>
    // <Grid>
    //   <Grid.Column width={6}>
    //     <div>
    //       <Ship ship={ {x: 1, y: 1, size: 1}} />
    //       <br />
    //       <Ship ship={  { x: 3, y: 1, size: 1 }} />
    //       <br />
    //       <Ship ship={ { x: 5, y: 1, size: 1 }} />
    //       <br />
    //       <Ship ship={   { x: 7, y: 1, size: 1 }} />
    //       <br />
    //       <Ship ship={ { x: 1, y: 3, size: 2 }} />
    //       <br />
    //       <Ship ship={ { x: 5, y: 3, size: 2 }} />
    //       <br />
    //       <Ship ship={ { x: 8, y: 3, size: 2 }} />
    //       <br />
    //       <Ship ship={{ x: 1, y: 5, size: 3 }} />
    //       <br />
    //       <Ship ship={{ x: 5, y: 5, size: 3 }} />
    //       <br />
    //       <Ship ship={{ x: 1, y: 7, size: 4 }} />
    //     </div>
    //   </Grid.Column>
    //   <Grid.Column width={10}>
    //     <Board />
    //   </Grid.Column>
    // </Grid>
  );
};

export default PrepareGame;
