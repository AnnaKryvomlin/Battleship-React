import { RootStore } from "./rootStore";

export default class GameStore{
    rootStore: RootStore;

    constructor(rootStore: RootStore) {
        this.rootStore = rootStore;
    }

}