export interface IUser {
    id: number;
    userName: string;
    token: string;
}

export interface IUserLoginValues {
    userName: string;
    password: string;
}

export interface IUserFormValues {
    email: string;
    userName: string;
    password: string;
}