import { RootStore } from "./rootStore";
import { observable, action, runInAction } from "mobx";
import { IGameForAccount } from "../models/game";
import agent from "../api/agent";
import { ICoordinate } from "../models/coordinate";
import { history } from "../..";
import { IRecord } from "../models/record";
import * as signalR from "@microsoft/signalr";
import {
  HubConnection,
  HubConnectionBuilder,
  LogLevel,
} from "@microsoft/signalr";
import { IShip } from "../models/ship";
import { toast } from "react-toastify";

export default class GameStore {
  rootStore: RootStore;

  constructor(rootStore: RootStore) {
    this.rootStore = rootStore;
  }

  @observable profileGames: IGameForAccount[] = [];
  @observable loadingGames = false;
  @observable myCoords: ICoordinate[] = [];
  @observable enemyCoords: ICoordinate[] = [];
  @observable gameId: number | null = null;
  @observable myLoadFlag: boolean = false;
  @observable enemyLoadFlag: boolean = false;
  @observable records: IRecord[] = [];
  @observable.ref hubConnection: HubConnection | null = null;
  @observable myMove: boolean = false;
  @observable finished = false;
  @observable finishedMessage = "";
  @observable ships: IShip[] = [
    { x: 1, y: 1, size: 1 },
    { x: 3, y: 1, size: 1 },
    { x: 5, y: 1, size: 1 },
    { x: 7, y: 1, size: 1 },
    { x: 1, y: 3, size: 2 },
    { x: 5, y: 3, size: 2 },
    { x: 8, y: 3, size: 2 },
    { x: 1, y: 5, size: 3 },
    { x: 5, y: 5, size: 3 },
    { x: 1, y: 7, size: 4 },
  ];
  @observable shipChange = true;
  @observable submitting = false;

  @action changeShipsCoords = (x: number, y: number, newShip: IShip) => {
    let ship = this.ships.find((s) => s.x === x && s.y === y);
    ship = newShip;
    this.changeShips();
  }

  @action changeShips = () => {
    this.shipChange === true ? this.shipChange =false : this.shipChange =true;
  }

  @action createNewGame = async() => {
    this.submitting = true;
    try {
      await agent.Game.createGame(this.ships)
      runInAction('create activity', () => {
        this.submitting = false;
      })
      history.push(`/profile/${this.rootStore.userStore.user}`)
      alert("Game successfully created! Wait your partner....");
    } catch (error) {
      runInAction('create activity error', () => {
        this.submitting = false;
      })
      toast.error('Problem submitting data');
      console.log(error.response);
    } 
  }

  @action findGame = async() => {
    this.submitting = true;
    try {
      await agent.Game.findGame(this.ships)
      runInAction('create activity', () => {
        this.submitting = false;
      })
      history.push(`/profile/${this.rootStore.userStore.user}`)
      alert("Wait your partner....");
    } catch (error) {
      runInAction('create activity error', () => {
        this.submitting = false;
      })
      toast.error('Problem submitting data');
      console.log(error.response);
    } 
  }

  @action createHubConnection = () => {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl("https://localhost:44336/gamehub", {
        //      accessTokenFactory: () => this.rootStore.commonStore.token!,
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets,
      })
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Information)
      .build();

    this.hubConnection!.start()
      .then(() => this.hubConnection!.invoke("JoinGame", this.gameId))
      .catch((error) => console.log("Error establishing connection: ", error));

    this.hubConnection.on(
      "TakeAShot",
      (result: boolean, x: number, y: number, currentPlayerId: number) => {
        runInAction(() => {
          this.enemyCoords.find((c) => c.x === x && c.y === y)!.mark = true;
          this.enemyLoadFlag = true;
          this.myLoadFlag = true;
          this.changeMovePlayer();
        });
      }
    );

    this.hubConnection.on(
      "ShowAShot",
      (x: number, y: number, currentPlayerId: number) => {
        runInAction(() => {
          this.myCoords.find((c) => c.x === x && c.y === y)!.mark = true;
          this.myLoadFlag = true;
          this.enemyLoadFlag = true;
          this.changeMovePlayer();
        });
      }
    );
    this.hubConnection.on("NewRecord", (record: string) => {
      runInAction(() => {
        const rec: IRecord = { playerMove: record };
        this.records.push(rec);
      });
    });

    this.hubConnection.on("Finished", (message: string) => {
      runInAction(() => {
        this.myMove = false;
        this.finishedMessage = message;
        this.finished = true;
      });
    });

    this.hubConnection.on("StopGame", (message: string) => {
      runInAction(() => {
        this.myMove = false;
        this.finishedMessage = message;
        this.finished = true;
      });
    });
  };

  @action changeMovePlayer = () => {
    this.myMove == true ? (this.myMove = false) : (this.myMove = true);
    console.log("move:" + this.myMove);
  };

  @action stopHubConnection = () => {
    this.hubConnection!.stop();
    this.enemyLoadFlag = false;
  };

  @action takeAShot = async (x: number, y: number) => {
    try {
      await this.hubConnection!.invoke(
        "TakeAShot",
        this.rootStore.userStore.userId,
        this.gameId,
        x,
        y
      );
    } catch (error) {
      console.log(error);
    }
  };

  @action stopGame = async () => {
    try {
      await this.hubConnection!.invoke(
        "TrowUpTheTowel",
        this.rootStore.userStore.userId,
        this.gameId
      );
    } catch (error) {
      console.log(error);
    }
  };

  @action setGameId = (game: IGameForAccount) => {
    this.gameId = game.id;
    if (game.currentMovePlayerId === this.rootStore.userStore.userId)
      this.myMove = true;
    else this.myMove = false;
    history.push(`/game/${this.gameId}`);
  };

  @action getGameRecords = async () => {
    try {
      const records = await agent.Game.recordsForGame(this.gameId!);
      runInAction(() => {
        this.records = records;
      });
    } catch (error) {
      console.log(error);
    }
  };

  @action loadEnemyCoords = async () => {
    this.loadingGames = true;
    try {
      const coords = await agent.Game.enemyCoordsForGame(this.gameId!);
      runInAction(() => {
        this.enemyCoords = coords;
        this.enemyLoadFlag = true;
        this.loadingGames = false;
      });
    } catch (error) {
      this.loadingGames = false;
      console.log(error);
    } finally {
      this.enemyLoadFlag = false;
    }
  };

  @action loadMyCoords = async () => {
    this.loadingGames = true;
    try {
      const coords = await agent.Game.myCoordsForGame(this.gameId!);
      runInAction(() => {
        this.myCoords = coords;
        this.myLoadFlag = true;
        this.loadingGames = false;
      });
    } catch (error) {
      this.loadingGames = false;
      console.log(error);
    } finally {
      this.myLoadFlag = false;
    }
  };

  @action loadProfileGames = async () => {
    this.loadingGames = true;
    try {
      const games = await agent.Game.listProfileGames();
      runInAction(() => {
        this.profileGames = games;
        this.loadingGames = false;
      });
    } catch (error) {
      this.loadingGames = false;
      console.log(error);
    }
  };
}
