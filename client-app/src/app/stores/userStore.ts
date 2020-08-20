import { IUser, IUserFormValues, IUserLoginValues } from "../models/user";
import { observable, computed, action, runInAction } from "mobx";
import agent from "../api/agent";
import { RootStore } from "./rootStore";
import { history } from "../..";
import { Console } from "console";

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

   @action getCurrentUserId = () => {
       const id =  this.userId;
       return id;
   }

   @action login = async(values: IUserLoginValues) => {
       try{
           const user= await agent.User.login(values);
           runInAction(() =>
           {
            this.user = user;
            this.userId = user.id;
           })
           this.rootStore.commonStore.setToken(user.token);
           history.push('/');
       }
       catch(error){
           throw(error);
       }
   }

   @action getUser = async () => {
       try {
           const user = await agent.User.current();
           runInAction(() => {
           this.user = user;
           this.userId = user.id;
           })
       }
       catch(error) {
           console.log(error);
       }
   }

   @action logout =() => {
       this.rootStore.commonStore.setToken(null);
       this.user = null;
       this.userId = null;
       history.push('/');
   }

   @action register = async (values: IUserFormValues) => {
       try {
           const user = await agent.User.register(values);
           history.push('/login');
       }
       catch(error) {
        throw(error);
    }
   }
}