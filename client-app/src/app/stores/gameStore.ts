import { RootStore } from "./rootStore";
import { observable, action, runInAction } from "mobx";
import { IGameForAccount } from "../models/game";
import agent from "../api/agent";
import { ICoordinate } from "../models/coordinate";
import { history } from "../..";

export default class GameStore {
  rootStore: RootStore;

  constructor(rootStore: RootStore) {
    this.rootStore = rootStore;
  }

  @observable profileGames: IGameForAccount[] = [];
  @observable loadingGames = false;
  @observable myCoords: ICoordinate[] = [];
  @observable gameId: number | null = null;
  @observable flag: boolean = false;
  @observable myBoard: Array<Array<JSX.Element>> = [];

@action setMyBoard = (board: Array<Array<JSX.Element>>) => {
this.myBoard = board;
}

  @action setGameId = (id: number) => {
    this.gameId = id;
    console.log(this.gameId);
    history.push(`/game/${this.gameId}`)
  };

  @action loadMyCoords = async () => {
    this.loadingGames = true;
    try {
      console.log(this.gameId);
      const coords = await agent.Game.myCoordsForGame(this.gameId!);
      runInAction(() => {
        this.myCoords = coords;
        this.flag = true;
        this.loadingGames = false;
      });
    } catch (error) {
      this.loadingGames = false;
      console.log(error);
    }
    finally {
      this.flag = false;
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
