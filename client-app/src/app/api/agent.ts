import axios, { AxiosResponse } from "axios";
import { IUser, IUserFormValues, IUserLoginValues } from "../models/user";
import { toast } from "react-toastify";
import { history } from "../..";
import { IGameForAccount } from "../models/game";
import { IStatisticPaged } from "../models/statistic";
import { ICoordinate } from "../models/coordinate";
import { IRecord } from "../models/record";

axios.defaults.baseURL = "https://localhost:44336/api";

axios.interceptors.request.use(
  (config) => {
    const token = window.localStorage.getItem("jwt");
    if (token) config.headers.Authorization = `Bearer ${token}`;
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

axios.interceptors.response.use(undefined, (error) => {
  if (error.message === "Network Error" && !error.response) {
    toast.error("Network error - make sure API is running!");
  }
  const { status, data, config } = error.response;
  if (status === 404) {
    history.push("/notfound");
  }
  if (
    status === 400 &&
    config.method === "get" &&
    data.errors.hasOwnProperty("id")
  ) {
    history.push("/notfound");
  }
  if (status === 500) {
    toast.error("Server error - check the terminal for more info!");
  }
  throw error.response;
});

const responseBody = (response: AxiosResponse) => response.data;

const sleep = (ms: number) => (response: AxiosResponse) =>
  new Promise<AxiosResponse>((resolve) =>
    setTimeout(() => resolve(response), ms)
  );

const requests = {
  get: (url: string) => axios.get(url).then(sleep(1000)).then(responseBody),
  post: (url: string, body: {}) =>
    axios.post(url, body).then(sleep(1000)).then(responseBody),
  put: (url: string, body: {}) =>
    axios.put(url, body).then(sleep(1000)).then(responseBody),
  del: (url: string) => axios.delete(url).then(sleep(1000)).then(responseBody),
};

const User = {
  current: (): Promise<IUser> => requests.get("/account"),
  login: (user: IUserLoginValues): Promise<IUser> =>
    requests.post(`account/login`, user),
  register: (user: IUserFormValues): Promise<IUser> =>
    requests.post(`account/register`, user),
};

const Game = {
  listProfileGames: (): Promise<IGameForAccount[]> =>
    requests.get("/PersonalAccount"),
  myCoordsForGame: (id: number): Promise<ICoordinate[]> =>
    requests.get(`/game/my_coords/${id}`),
  enemyCoordsForGame: (id: number): Promise<ICoordinate[]> =>
    requests.get(`/game/enemy_coords/${id}`),
  recordsForGame: (id: number): Promise<IRecord[]> =>
    requests.get(`/game/get_records/${id}`),
};

const Statistic = {
  list: (params: URLSearchParams): Promise<IStatisticPaged> =>
    axios.get(`/Statistics`, { params: params }).then(responseBody),
};

export default {
  User,
  Game,
  Statistic,
};
