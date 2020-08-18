import React from "react";
import Ship from "./Ship";
import { Grid } from "semantic-ui-react";
import Board from "./Board";

const PrepareGame = () => {
  return (
    <Grid>
      <Grid.Column width={6}>
        <div>
          <Ship size={4} />
          <br />
          <Ship size={3} />
          <br />
          <Ship size={3} />
          <br />
          <Ship size={2} />
          <br />
          <Ship size={2} />
          <br />
          <Ship size={2} />
          <br />
          <Ship size={1} />
          <br />
          <Ship size={1} />
          <br />
          <Ship size={1} />
          <br />
          <Ship size={1} />
        </div>
      </Grid.Column>
      <Grid.Column width={10}>
        <Board />
      </Grid.Column>
    </Grid>
  );
};

export default PrepareGame;
