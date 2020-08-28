import React, { useContext } from 'react'
import { Container, Menu, Dropdown } from 'semantic-ui-react'
import { NavLink, Link } from 'react-router-dom'
import { RootStoreContext } from '../../app/stores/rootStore';

const AuthNavBar = () => {
    const rootStore = useContext(RootStoreContext);
    const { logout, user } = rootStore.userStore;
    
    return (
        <Container>
        <Menu.Item header as={NavLink} exact to="/">
          Battleship
        </Menu.Item>
        <Menu.Item name="Start Game" as={NavLink} to="/creategame" />
        <Menu.Item name="Statistic" as={NavLink} to="/statistic" />      
          <Menu.Item position="right">
            <Dropdown pointing="top left" text={user!.userName}>
              <Dropdown.Menu>
                <Dropdown.Item
                  as={Link}
                  to={`/profile/${user!.userName}`}
                  text="My profile"
                  icon="user"
                />
                <Dropdown.Item onClick={logout} text="Logout" icon="power" />
              </Dropdown.Menu>
            </Dropdown>
          </Menu.Item>
      </Container>
    )
}

export default AuthNavBar
