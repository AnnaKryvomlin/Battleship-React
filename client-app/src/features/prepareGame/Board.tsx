import React, { useState, useEffect, useContext } from "react";
import BoardSquare from "./BoardSquare";
import Ship from "./Ship";
import { RootStoreContext } from "../../app/stores/rootStore";
import { observer } from "mobx-react-lite";

const boardStyle: React.CSSProperties = {
  width: "400px",
  height: "400px",
  display: "flex",
  flexWrap: "wrap",
};

const Board = () => {
  const rootStore = useContext(RootStoreContext);
  const { ships, shipChange } = rootStore.gameStore;

  const [board, setMyBoard] = useState(Array<Array<JSX.Element>>([]));

  useEffect(() => {
    setBoard();
  }, [shipChange]);

  function setBoard() {
    const yLine: JSX.Element[][] = [];
    for (let x = 1; x <= 10; x++) {
      const xLine: JSX.Element[] = [];
      for (let y = 1; y <= 10; y++) {
        const cell = renderSquare(x, y);
        xLine.push(cell);
      }
      yLine.push(xLine);
    }
    setMyBoard(yLine);
    return(yLine);
  }

  function setShip(x: number, y: number) {
    const shipForPosition = ships!.find((s) => s.x === x && s.y === y);
    return shipForPosition !== undefined ? (
      <Ship ship={shipForPosition} />
    ) : null;
  }

  function renderSquare(x: number, y: number) {
    const hasShip =
      !!ships.find((s) => s.y - 1 <= y && s.y + s.size + 1 > y && s.x == x) ||
      !!ships.find((s) => s.y <= y && s.y + s.size > y && s.x == x - 1) ||
      !!ships.find((s) => s.y <= y && s.y + s.size > y && s.x == x + 1);
    return (
      <div key={"x" + x + " y" + y}>
        <BoardSquare x={x} y={y} hasShip={hasShip}>
          {setShip(x, y)}
        </BoardSquare>
      </div>
    );
  }

  return <div style={boardStyle}>{board} </div>;
};

export default observer(Board);
