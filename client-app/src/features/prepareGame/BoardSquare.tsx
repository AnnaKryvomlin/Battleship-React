import React, { useContext, useEffect } from "react";
import { useDrop, DropTarget } from "react-dnd";
import { Overlay } from "./Overlay";
import { RootStoreContext } from "../../app/stores/rootStore";
import { IShip } from "../../app/models/ship";
import { observer } from "mobx-react-lite";

export interface BoardSquareProps {
  x: number;
  y: number;
  children: JSX.Element | null;
  hasShip: boolean;
}

const BoardSquare: React.FC<BoardSquareProps> = ({
  x,
  y,
  children,
  hasShip,
}: BoardSquareProps) => {
  const rootStore = useContext(RootStoreContext);
  const { changeShipsCoords, shipChange, ships } = rootStore.gameStore;

  function moveShip(ship: IShip) {
    ship!.x = x;
    ship!.y = y;
    changeShipsCoords(x, y, ship!);
  }

  function canMoveShip(ship: IShip) {
    if (ship.size > 1 && y + ship.size - 1 > 10) return false;
    if (hasShip) return false;
    return true;
  }

  const [{ isOver, canDrop }, drop] = useDrop({
    accept: "ship",
    canDrop: (item: { type: string; ship: IShip }) => canMoveShip(item.ship),
    drop: (item: { type: string; ship: IShip }) => moveShip(item.ship),
    collect: (monitor) => ({
      isOver: !!monitor.isOver(),
      canDrop: !!monitor.canDrop(),
    }),
  });

  return (
    <div
      ref={drop}
      style={{
        position: "relative",
        width: 40,
        height: 40,
        border: "1px solid black",
      }}
    >
      {children}
      {isOver && !canDrop && <Overlay color="red" />}
      {isOver && canDrop && <Overlay color="green" />}
    </div>
  );
};

export default observer(BoardSquare);
