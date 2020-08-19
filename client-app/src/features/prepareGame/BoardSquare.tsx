import React from "react";
import { useDrop } from "react-dnd";
import { Overlay } from "./Overlay";

export interface BoardSquareProps {
  x: number;
  y: number;
 // children: any;
}


const BoardSquare: React.FC<BoardSquareProps> = ({
  x,
  y,
  //children,
}: BoardSquareProps) => {
    
const [{ isOver, canDrop }, drop] = useDrop({
    accept: 'ship',
    canDrop: () => { return true},
    // drop: () => moveKnight(x, y),
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
    {isOver && !canDrop && <Overlay color="red" />}
    {isOver && canDrop && <Overlay color="green" />}
  </div>
  );
};

export default BoardSquare;
