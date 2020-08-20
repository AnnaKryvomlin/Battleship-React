import UserStore from './userStore';
import { createContext } from 'react';
import CommonStore from './commonStore';
import GameStore from './gameStore';
import StatisticStore from './statisticStore';

export class RootStore {
    userStore: UserStore;
    commonStore: CommonStore;
    gameStore: GameStore;
    statisticStore: StatisticStore;

    constructor() {
        this.userStore = new UserStore(this);
        this.commonStore = new CommonStore(this);
        this.gameStore = new GameStore(this);
        this.statisticStore = new StatisticStore(this);
    }
}

export const RootStoreContext = createContext(new RootStore());