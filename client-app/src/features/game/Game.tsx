import React, { useContext, useEffect } from "react";
import { Grid } from "semantic-ui-react";
import MyBoard from "./MyBoard";
import { RootStoreContext } from "../../app/stores/rootStore";
import { observer } from "mobx-react-lite";

const Game = () => {
    const rootStore = useContext(RootStoreContext);
    const { loadMyCoords } = rootStore.gameStore;
    
    useEffect(() => {
        loadMyCoords();
      }, [loadMyCoords]);

  return (
    <Grid>
      <Grid.Column width={8}>
          <MyBoard/>
      </Grid.Column>
      <Grid.Column width={8}></Grid.Column>
    </Grid>
  );
};

export default observer(Game);
