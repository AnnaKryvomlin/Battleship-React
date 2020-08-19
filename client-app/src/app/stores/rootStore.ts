import UserStore from './userStore';
import { createContext } from 'react';
import CommonStore from './commonStore';
import GameStore from './gameStore';

export class RootStore {
    userStore: UserStore;
    commonStore: CommonStore;
    gameStore: GameStore;

    constructor() {
        this.userStore = new UserStore(this);
        this.commonStore = new CommonStore(this);
        this.gameStore = new GameStore(this);
    }
}

export const RootStoreContext = createContext(new RootStore());