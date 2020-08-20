export interface IStatistic {
    winner: string;
    moveCount: number;
    winnerShipsCount: number;
    winnerShips: IStatisticsShip[];
}

export interface IStatisticsShip {
    size: number;
    injuredCells: number;
}

export interface IStatisticPaged {
    statisticsRecords: IStatistic[];
    totalPages: number;
}