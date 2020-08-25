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
    enemyLoadFlag,
    myMove,
    takeAShot
  } = rootStore.gameStore;

  const [board, setMyBoard] = useState(Array<Array<JSX.Element>>([]));

  useEffect(() => {
    if (enemyLoadFlag) {
      setBoard();
    }
  }, [myMove, enemyLoadFlag]);

  if (rootStore.gameStore.loadingGames)
    return <LoadingComponent content="Loading enemy field" />;


  function setBoard() {
    console.log("set board");
    const yLine: JSX.Element[][] = [];
    for (let y = 1; y <= 10; y++) {
      const xLine: JSX.Element[] = [];
      for (let x = 1; x <= 10; x++) {
        const hasShip = enemyCoords.find((c) => c.x === x && c.y === y)!.haveShip;
        const mark = enemyCoords.find((c) => c.x === x && c.y === y)!.mark;
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
            background: "white",
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
    console.log(rootStore.gameStore.myMove);
    setMyBoard(yLine);
  }

  const handleTakeAShot = (x: number, y: number) => {
    console.log("shot!");
    takeAShot(x, y);
  };

  function renderSquare(x: number, y: number, color?: React.CSSProperties) {
    return (
      <button
        onClick={() => handleTakeAShot(x, y)}
        className="move"
        key={"x" + x + " y" + y}
        style={color}
        disabled={!myMove}
      ></button>
    );
  }
  return <div style={boardStyle}>{board}</div>;
};

export default observer(EnemyBoard);
