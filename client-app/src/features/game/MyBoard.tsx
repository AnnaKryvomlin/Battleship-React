import React, { useContext, useEffect, useState } from "react";
import { RootStoreContext } from "../../app/stores/rootStore";
import LoadingComponent from "../../app/layout/LoadingComponent";
import { observer } from "mobx-react-lite";
import BoardSquare from "../prepareGame/BoardSquare";
import { Button } from "semantic-ui-react";

const boardStyle: React.CSSProperties = {
  width: "400px",
  height: "400px",
  display: "flex",
  flexWrap: "wrap",
};

const MyBoard = () => {
  const rootStore = useContext(RootStoreContext);
  const { myCoords, flag } = rootStore.gameStore;

  const [board, setMyBoard] = useState(Array<Array<JSX.Element>>([]));
  
  useEffect(() => {
    if (flag) {
      console.log("кря");
      setBoard();
    }
  }, [myCoords]);

  if (rootStore.gameStore.loadingGames)
    return <LoadingComponent content="Loading your field" />;

//  let board: Array<Array<JSX.Element>> = [];

  function setBoard() {
    const yLine: JSX.Element[][] = [];
    for (let y = 1; y <= 10; y++) {
      const xLine: JSX.Element[] = [];
      for (let x = 1; x <= 10; x++) {
        const hasShip = myCoords.find((c) => c.x == x && c.y == y)!.haveShip;
        const mark = myCoords.find((c) => c.x == x && c.y == y)!.mark;
        let cell: JSX.Element;
        if (hasShip && mark) {
          cell = renderSquare(x, y, {
            background: "red",
            width: 40,
            height: 40,
            border: "1px solid black",
            position: "relative",
          });
        }
        if (!hasShip && mark) {
          cell = renderSquare(x, y, {
            background: "yellow",
            width: 40,
            height: 40,
            border: "1px solid black",
            position: "relative",
          });
        }
        if (hasShip && !mark) {
          cell = renderSquare(x, y, {
            background: "green",
            width: 40,
            height: 40,
            border: "1px solid black",
            position: "relative",
          });
        }
        if (!hasShip && !mark) {
          cell = renderSquare(x, y, {
            background: "white",
            width: 40,
            height: 40,
            border: "1px solid black",
            position: "relative",
          });
        }
        xLine.push(cell!);
      }
      yLine.push(xLine);
    }
    setMyBoard(yLine);
  }

  function renderSquare(x: number, y: number, color?: React.CSSProperties) {
    return (
      <div key={"x" + x + " y" + y} style={color}>

      </div>
    );
  }
  return (
    <div style={boardStyle}>
      {!flag && board} 
    </div>
  );
};

export default observer(MyBoard);
