import React, { useContext } from "react";
import { Menu, Container, Button, Dropdown } from "semantic-ui-react";
import { observer } from "mobx-react-lite";
import { NavLink, Link } from "react-router-dom";
import { RootStoreContext } from "../../app/stores/rootStore";

const NavBar: React.FC = () => {
  const rootStore = useContext(RootStoreContext);
  const { logout, user } = rootStore.userStore;
  return (
    <Menu fixed="top" inverted>
      <Container>
        <Menu.Item header as={NavLink} exact to="/">
          Battleship
        </Menu.Item>
        {user && <Menu.Item name="Start Game" as={NavLink} to="/creategame" />}
        {user && (
          <Menu.Item position="right">
            <Dropdown pointing="top left" text={user.userName}>
              <Dropdown.Menu>
                <Dropdown.Item
                  as={Link}
                  to={`/profile/${user.userName}`}
                  text="My profile"
                  icon="user"
                />
                <Dropdown.Item onClick={logout} text="Logout" icon="power" />
              </Dropdown.Menu>
            </Dropdown>
          </Menu.Item>
        )}
        {!user && (
          <Menu.Item position="right" name="Login" as={NavLink} to="/login" />
        )}
        {!user && (
          <Menu.Item
          
            name="Register"
            as={NavLink}
            to="/register"
          />
        )}
      </Container>
    </Menu>
  );
};

export default observer(NavBar);
