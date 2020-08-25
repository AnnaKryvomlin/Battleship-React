import React from 'react'
import { useDrag } from 'react-dnd'
import { IShip } from '../../app/models/ship'


const shipStyle: React.CSSProperties = {
    cursor: 'move',
    height: 40,
    background: 'red',
    border: '2px solid black',
    position: "absolute"
  }


const Ship : React.FC<{ship: IShip}> = ({ship}) => {
    const [{ isDragging }, drag] = useDrag({
      item: { type: 'ship', ship },
      collect: (monitor) => ({
        isDragging: !!monitor.isDragging(),
      }),
    })
    return (
        <div
        ref={drag}
        style={{
          ...shipStyle,
          opacity: isDragging ? 0.5 : 1,
          width: 40*ship.size
        }}
      >
      </div>
    )
}

export default Ship
