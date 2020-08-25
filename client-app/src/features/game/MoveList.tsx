import React, { useContext } from "react";
import { List } from "semantic-ui-react";
import { RootStoreContext } from "../../app/stores/rootStore";
import { observer } from "mobx-react-lite";

const MoveList = () => {
  const rootStore = useContext(RootStoreContext);
  const { records } = rootStore.gameStore;

  return (
    <List>
      {records.map((record) => (
        <List.Item key={record.playerMove}>{record.playerMove}</List.Item>
      ))}
    </List>
  );
};

export default observer(MoveList);
