import React, { useContext, useState, useEffect } from "react";
import { RootStoreContext } from "../../app/stores/rootStore";
import LoadingComponent from "../../app/layout/LoadingComponent";
import { observer } from "mobx-react-lite";

const boardStyle: React.CSSProperties = {
  width: "400px",
  height: "400px",
  display: "flex",
  flexWrap: "wrap",
};

const EnemyBoard = () => {
  const rootStore = useContext(RootStoreContext);
  const {
    enemyCoords,
    myMove,
    takeAShot
  } = rootStore.gameStore;

  if (rootStore.gameStore.loadingGames)
    return <LoadingComponent content="Loading enemy field" />;

  function ChooseCellColor(haveShip: boolean, mark: boolean) {
    let boxStyle: React.CSSProperties = {
      width: 40,
      height: 40,
      border: "1px solid black",
      position: "relative",
      background: "",
    };
    if (haveShip && mark) {
      boxStyle.background = "red";
    }
    if (!haveShip && mark) {
      boxStyle.background = "yellow";
    }
    else {
      boxStyle.background = "white";
    }
    return boxStyle;
  }

  return <div style={boardStyle}>
  {enemyCoords.map((coord) => (
    <button
    onClick={() => takeAShot(coord.x, coord.y)}
    className="move"
    key={"x" + coord.x + " y" + coord.y}
    style={ChooseCellColor(coord.haveShip, coord.mark)}
    disabled={!myMove}
  ></button>
  ))}
</div>;
};

export default observer(EnemyBoard);
