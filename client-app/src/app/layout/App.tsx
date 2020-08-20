import React, { Fragment, useContext, useEffect } from "react";
import {
  Route,
  withRouter,
  RouteComponentProps,
  Switch,
} from "react-router-dom";
import HomePage from "../../features/home/HomePage";
import NavBar from "../../features/nav/NavBar";
import { Container } from "semantic-ui-react";
import PrepareGame from "../../features/prepareGame/PrepareGame";
import { observer } from "mobx-react-lite";
import LoginForm from "../../features/user/LoginForm";
import NotFound from "./NotFound";
import { RootStoreContext } from "../stores/rootStore";
import LoadingComponent from "./LoadingComponent";
import RegisterForm from "../../features/user/RegisterForm";
import PersonalAccount from "../../features/personalAccount/PersonalAccount";
import Statistic from "../../features/Statistics/Statistic";
import PrivateRoute from "./PrivateRoute";

const App: React.FC<RouteComponentProps> = ({ location }) => {
  const rootStore = useContext(RootStoreContext);
  const { setAppLoaded, token, appLoaded } = rootStore.commonStore;
  const { getUser } = rootStore.userStore;

  useEffect(() => {
    if (token) {
      getUser().finally(() => setAppLoaded());
    } else {
      setAppLoaded();
    }
  }, [getUser, setAppLoaded, token]);

  if(!appLoaded) return <LoadingComponent content='Loading app...'/>

  return (
    <Fragment>
      <Fragment>
        <NavBar />
        <Container style={{ marginTop: "7em" }}>
          <Switch>
            <Route exact path="/" component={HomePage} />
            <PrivateRoute exact path="/creategame" component={PrepareGame} />
            <Route exact path="/login" component={LoginForm} />
            <Route exact path="/register" component={RegisterForm} />
            <PrivateRoute  exact path="/profile/:username" component={PersonalAccount} />
            <PrivateRoute exact path="/statistic" component={Statistic} />
            <Route component={NotFound} />
          </Switch>
        </Container>
      </Fragment>
    </Fragment>
  );
};

export default withRouter(observer(App));
