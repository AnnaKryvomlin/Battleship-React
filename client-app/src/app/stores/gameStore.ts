import { RootStore } from "./rootStore";
import { observable, action, runInAction } from "mobx";
import { IGameForAccount } from "../models/game";
import agent from "../api/agent";

export default class GameStore {
  rootStore: RootStore;

  constructor(rootStore: RootStore) {
    this.rootStore = rootStore;
  }

  @observable profileGames: IGameForAccount[] = [];
  @observable loadingProfile = false;

  @action loadProfileGames = async () => {
    this.loadingProfile = true;
    try {
      const games = await agent.Game.listProfileGames();
      runInAction(() => {
        this.profileGames = games;
        this.loadingProfile = false;
      });
    } catch (error) {
      console.log(error);
    }
  };
}
