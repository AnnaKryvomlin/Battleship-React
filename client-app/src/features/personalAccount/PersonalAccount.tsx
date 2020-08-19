import React, { useContext, useEffect } from "react";
import { RootStoreContext } from "../../app/stores/rootStore";
import LoadingComponent from "../../app/layout/LoadingComponent";
import { Card, Grid } from "semantic-ui-react";
import ListGames from "./ListGames";
import { observer } from "mobx-react-lite";

const PersonalAccount = () => {
  const rootStore = useContext(RootStoreContext);
  const { user } = rootStore.userStore;

  useEffect(() => {
    rootStore.gameStore.loadProfileGames();
  }, [rootStore.gameStore]);

  if (rootStore.gameStore.loadingProfile)
    return <LoadingComponent content="Loading your games" />;

  return (
    <Grid>
      <Grid.Column width={4}>
        <h2>{user?.userName}</h2>
      </Grid.Column>
      <Grid.Column width={12}>
        <ListGames />
      </Grid.Column>
    </Grid>
  );
};

export default observer (PersonalAccount);
