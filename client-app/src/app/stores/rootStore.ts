import UserStore from './userStore';
import { createContext } from 'react';
import CommonStore from './commonStore';

export class RootStore {
    userStore: UserStore;
    commonStore: CommonStore;

    constructor() {
        this.userStore = new UserStore(this);
        this.commonStore = new CommonStore(this);
    }
}

export const RootStoreContext = createContext(new RootStore());