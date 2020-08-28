import { IUser, IUserFormValues, IUserLoginValues } from "../models/user";
import { observable, computed, action, runInAction } from "mobx";
import agent from "../api/agent";
import { RootStore } from "./rootStore";
import { history } from "../..";

export default class UserStore {
  rootStore: RootStore;

  constructor(rootStore: RootStore) {
    this.rootStore = rootStore;
  }

  @observable user: IUser | null = null;
  @observable userId: number | null = null;

  @computed get isLoggedIn() {
    return !!this.user;
  }

  @action login = async (values: IUserLoginValues) => {
    try {
      const user = await agent.User.login(values);
      this.setUser(user);
      this.rootStore.commonStore.setToken(user.token);
      history.push("/");
    } catch (error) {
      throw error;
    }
  };

  @action getUser = async () => {
    try {
      const user = await agent.User.current();
      this.setUser(user);
    } catch (error) {
      console.log(error);
    }
  };

  @action setUser = (user: IUser) => {
    this.user = user;
    this.userId = user.id;
  };

  @action logout = () => {
    this.rootStore.commonStore.setToken(null);
    this.user = null;
    this.userId = null;
    history.push("/");
  };

  @action register = async (values: IUserFormValues) => {
    try {
      const user = await agent.User.register(values);
      history.push("/login");
    } catch (error) {
      throw error;
    }
  };
}
