export interface ICoordinate {
  x: number;
  y: number;
  mark: boolean;
  haveShip: boolean;
}

// export class MyCoordinate implements ICoordinate {
//   @observable
//   public x: number;
//   @observable
//   public y: number;
//   @observable
//   public mark: boolean;
//   @observable
//   public haveShip: boolean;

//   constructor(x: number, y: number, mark: boolean, haveShip: boolean) {
//     this.x = x;
//     this.y = y;
//     this.mark = mark;
//     this.haveShip = haveShip;
//   }

//   @computed get color() {
//     if (this.haveShip && this.mark) {
//       return "red";
//     }
//     if (!this.haveShip && this.mark) {
//       return "yellow";
//     }
//     if (this.haveShip && !this.mark) {
//       return "green";
//     }
//     if (!this.haveShip && !this.mark) {
//       return "white";
//     }
//   }
// }

//export default MyCoordinate;
