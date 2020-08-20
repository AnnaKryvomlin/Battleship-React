import { RootStore } from "./rootStore";
import { observable, action, runInAction, computed, $mobx } from "mobx";
import { IStatistic, IStatisticPaged } from "../models/statistic";
import agent from "../api/agent";

export default class StatisticStore {
  rootStore: RootStore;

  constructor(rootStore: RootStore) {
    this.rootStore = rootStore;
  }

  @observable statisticRecords: IStatistic[] = [];
  @observable loadingStatistic = false;
  @observable page = 1;
  @observable totalCount = 0;
  @observable name = "";
  @observable filterMoveState = 0;
  @observable onlyIntactShips = false;
  @observable sortOrder = 0;

  private sortName: number = 0;
  private moveSort: number = 2;
  private ShipCountSort: number = 4;

  @computed get axiosParams() {
    const params = new URLSearchParams();
    params.append("page", this.page.toString());
    params.append("name", this.name);
    params.append("filterMoveState", this.filterMoveState.toString());
    params.append("onlyIntactShips", this.onlyIntactShips.toString());
    params.append("sortOrder", this.sortOrder.toString())
    return params;
  }

  @action changeSortName = () => {
    this.sortName == 0 ? (this.sortName = 1) : (this.sortName = 0);
    this.sortOrder = this.sortName;
  };

  @action changeMoveSort = () => {
    this.moveSort == 2 ? (this.moveSort = 3) : (this.moveSort = 2);
    this.sortOrder = this.moveSort;
  };

  @action changeShipCountSort = () => {
    this.ShipCountSort == 4 ? (this.ShipCountSort = 5) : (this.ShipCountSort = 4);
    this.sortOrder = this.ShipCountSort;
  };

  @action setOnlyIntactOtion = (OIOption: boolean) => {
    console.log(OIOption);
    this.onlyIntactShips = OIOption;
  };

  @action setName = (filterName: string) => {
    this.name = filterName;
  };

  @action setFilter = (filterNum: number) => {
    this.filterMoveState = filterNum;
  };

  @action setPage = (page: number) => {
    this.page = page;
  };

  @action getStatistic = async () => {
    this.loadingStatistic = true;
    try {
      const pagedRecords = await agent.Statistic.list(this.axiosParams);

      runInAction(() => {
        this.statisticRecords = pagedRecords.statisticsRecords;
        this.totalCount = pagedRecords.totalPages;
        this.loadingStatistic = false;
      });
    } catch (error) {
      this.loadingStatistic = false;
      console.log(error);
    }
  };
}
