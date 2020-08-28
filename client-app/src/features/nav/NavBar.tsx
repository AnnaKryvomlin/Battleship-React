import React, { useContext } from "react";
import { Menu } from "semantic-ui-react";
import { observer } from "mobx-react-lite";
import { RootStoreContext } from "../../app/stores/rootStore";
import AuthNavBar from "./AuthNavBar";
import AnonymousNavBar from "./AnonymousNavBar";

const NavBar: React.FC = () => {
  const rootStore = useContext(RootStoreContext);
  const { logout, user } = rootStore.userStore;

  return (
    <Menu fixed="top" inverted>
      {user && <AuthNavBar/>}
      {!user && <AnonymousNavBar/>}
    </Menu>
  );
};

export default observer(NavBar);
