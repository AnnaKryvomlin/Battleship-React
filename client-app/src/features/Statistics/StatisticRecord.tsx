import React, { useContext, Fragment } from "react";
import { Table } from "semantic-ui-react";
import { RootStoreContext } from "../../app/stores/rootStore";
import { observer } from "mobx-react-lite";

const StatisticRecord = () => {
    const rootStore = useContext(RootStoreContext);
    const { statisticRecords } = rootStore.statisticStore;

  return (
    <Fragment>
        {statisticRecords.map((record) => (
    <Table.Row> 
    <Table.Cell>{record.winner} </Table.Cell>
    <Table.Cell>{record.moveCount}</Table.Cell>
    <Table.Cell>{record.winnerShipsCount}</Table.Cell>
    <Table.Cell>{record.winnerShips.map((ship) => (
    <span>{ship.size} - палубный {ship.injuredCells!= 0 && <span>/ранено {ship.injuredCells}</span>};</span> 
    ))}</Table.Cell>
  </Table.Row>
        )) }

    </Fragment>
  );
};

export default observer(StatisticRecord);
