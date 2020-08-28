import React from "react";
import { Container, Menu, Dropdown } from "semantic-ui-react";
import { NavLink, Link } from "react-router-dom";

const AnonymousNavBar = () => {
  return (
    <Container>
      <Menu.Item header as={NavLink} exact to="/">
        Battleship
      </Menu.Item>
      <Menu.Item position="right" name="Login" as={NavLink} to="/login" />
      <Menu.Item name="Register" as={NavLink} to="/register" />
    </Container>
  );
};

export default AnonymousNavBar;
