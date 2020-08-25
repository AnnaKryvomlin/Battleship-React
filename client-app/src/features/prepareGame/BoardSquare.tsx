import React, { useContext, useEffect } from "react";
import { useDrop, DropTarget } from "react-dnd";
import { Overlay } from "./Overlay";
import { RootStoreContext } from "../../app/stores/rootStore";
import { IShip } from "../../app/models/ship";

export interface BoardSquareProps {
  x: number;
  y: number;
  children: JSX.Element | null;
}


const BoardSquare: React.FC<BoardSquareProps> = ({
  x,
  y,
  children,
}: BoardSquareProps) => {

  const rootStore = useContext(RootStoreContext);
  const { changeShipsCoords, shipChange } = rootStore.gameStore;

  function moveShip(ship: IShip) {
    console.log(ship.x +" " + ship.y);
    ship!.x = x;
    ship!.y = y;
    console.log(ship.x +" " + ship.y);
    changeShipsCoords(x, y, ship!)
  }

    
const [{ isOver, canDrop }, drop] = useDrop({
    accept: 'ship',
    canDrop: () => { return true},
    drop: (item: {type: string, ship: IShip}) => moveShip(item.ship),
    collect: (monitor) => ({
      isOver: !!monitor.isOver(),
      canDrop: !!monitor.canDrop(),
    }),
  })

  return (
    <div
    ref={drop}
    style={{
      position: 'relative',
      width: 40,
      height: 40,
      border: "1px solid black"
    }}
  >
    {children}
    {isOver && !canDrop && <Overlay color="red" />}
    {isOver && canDrop && <Overlay color="green" />}
  </div>
  );
};

export default BoardSquare;
