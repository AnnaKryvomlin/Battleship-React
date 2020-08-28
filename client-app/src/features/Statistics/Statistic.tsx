import React, { useContext, useEffect, useState, Fragment } from "react";
import { Table, Pagination, PaginationProps, Button } from "semantic-ui-react";
import { RootStoreContext } from "../../app/stores/rootStore";
import LoadingComponent from "../../app/layout/LoadingComponent";
import StatisticRecord from "./StatisticRecord";
import { observer } from "mobx-react-lite";
import StatisticFilters from "./StatisticFilters";
import { Link } from "react-router-dom";

const Statistic = () => {
  const rootStore = useContext(RootStoreContext);
  const {
    statisticRecords,
    setPage,
    page,
    getStatistic,
    totalCount,
    name,
    filterMoveState,
    onlyIntactShips,
    changeSortName,
    changeMoveSort,
    changeShipCountSort,
    sortOrder,
  } = rootStore.statisticStore;
  const [loadindNext, setLoadingNext] = useState(false);

  const handleGetNext = (
    e: React.MouseEvent<HTMLAnchorElement>,
    pageInfo: PaginationProps
  ) => {
    setLoadingNext(true);
    setPage(+pageInfo.activePage!);
    getStatistic().then(() => setLoadingNext(false));
  };

  useEffect(() => {
    getStatistic();
  }, [getStatistic, name, filterMoveState, onlyIntactShips, sortOrder]);

  if (rootStore.statisticStore.loadingStatistic)
    return <LoadingComponent content="Loading statistic" />;

  return (
    <Fragment>
      <StatisticFilters />
      <Table celled>
        <Table.Header>
          <Table.Row>
            <Table.HeaderCell>
              <Button basic onClick={changeSortName}>
                Winner
              </Button>
            </Table.HeaderCell>
            <Table.HeaderCell>
              <Button basic onClick={changeMoveSort}>
                MoveCount
              </Button>
            </Table.HeaderCell>
            <Table.HeaderCell>
              <Button basic onClick={changeShipCountSort}>
                Winner Ships Count
              </Button>
            </Table.HeaderCell>
            <Table.HeaderCell>Winner Ships</Table.HeaderCell>
          </Table.Row>
        </Table.Header>

        <Table.Body>
          <StatisticRecord />
        </Table.Body>

        <Table.Footer>
          <Table.Row>
            <Table.HeaderCell colSpan="4">
              <Pagination
                totalPages={Math.ceil(totalCount / 3)}
                activePage={page}
                onPageChange={handleGetNext}
              />
            </Table.HeaderCell>
          </Table.Row>
        </Table.Footer>
      </Table>
    </Fragment>
  );
};

export default observer(Statistic);
