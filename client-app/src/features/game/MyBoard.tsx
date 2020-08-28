import React, { useContext, useEffect, useState } from "react";
import { RootStoreContext } from "../../app/stores/rootStore";
import LoadingComponent from "../../app/layout/LoadingComponent";
import { observer } from "mobx-react-lite";

const boardStyle: React.CSSProperties = {
  width: "400px",
  height: "400px",
  display: "flex",
  flexWrap: "wrap",
};

const MyBoard = () => {
  const rootStore = useContext(RootStoreContext);
  const { myCoords } = rootStore.gameStore;

  if (rootStore.gameStore.loadingGames)
    return <LoadingComponent content="Loading your field" />;

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
    if (haveShip && !mark) {
      boxStyle.background = "green";
    }
    if (!haveShip && !mark) {
      boxStyle.background = "white";
    }
    return boxStyle;
  }

  return (
    <div style={boardStyle}>
      {myCoords.map((coord) => (
        <div
          key={"x" + coord.x + " y" + coord.y}
          style={ChooseCellColor(coord.haveShip, coord.mark)}
        >x{coord.x}_y{coord.y}</div>
      ))}
    </div>
  );
};

export default observer(MyBoard);
