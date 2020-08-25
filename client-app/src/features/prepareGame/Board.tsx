import React from "react";
import { IShip } from "../../app/models/ship";
import BoardSquare from "./BoardSquare";

const boardStyle: React.CSSProperties = {
    width: '400px',
    height: '400px',
    display: 'flex',
    flexWrap: 'wrap',
  }

const Board = () => {
  let board: Array<Array<JSX.Element>> = [];
  setBoard();
  function setBoard() {
    const yLine: JSX.Element[][] = [];
    for (let y = 1; y <= 10; y++) {
      const xLine: JSX.Element[] = [];
      for (let x = 1; x <= 10; x++) {
        const cell = renderSquare(x, y);
        xLine.push(cell);
      }
      yLine.push(xLine);
    }
    board = yLine;
    console.log(board);
  }

  function renderSquare(x: number, y: number) {
    return (
      <div key={"x" + x + " y" + y}>
        <BoardSquare x={x} y={y}></BoardSquare>
      </div>
    );
  }

return <div style={boardStyle}>{board}</div>;
};

export default Board;
