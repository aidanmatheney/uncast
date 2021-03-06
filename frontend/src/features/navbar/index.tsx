import React, { FunctionComponent } from 'react';
import styled, { css } from 'styled-components';

import TabId from '../../common/TabId';

import {
  FaHome,
  FaCloud,
  FaUser
} from 'react-icons/fa';
import { IconType } from 'react-icons/lib'

import '../../index.css';

import {
  NavLink
} from "react-router-dom";

const Container = styled.div`
  background: ${props => props.theme.pageBackground};
  display: grid;
  grid-template-columns: 1fr 1fr 1fr;
`;

const Button = styled.button<{ isActive: boolean; }>`
  text-align: center;
  margin: 0.25rem;
  cursor: pointer;
  background: ${props => props.theme.background};
  color: ${props => props.theme.color};
  border: 2px solid ${props => props.theme.borderColor};
  border-radius: 3px;

  ${({ isActive }) => isActive && css``}
`;

const IconContainer = styled.div``;
const TextContainer = styled.div``;

const StyledNavLink = styled(NavLink)`
  color: ${props => props.theme.color};
`;

const TabDescriptions: {
  tab: TabId;
  name: string;
  Icon: IconType;
}[] = [
  {
    tab: TabId.Library,
    name: 'Library',
    Icon: FaHome
  },
  {
    tab: TabId.Catalog,
    name: 'Catalog',
    Icon: FaCloud
  },
  {
    tab: TabId.Profile,
    name: 'Profile',
    Icon: FaUser
  }
];

const NavBar: FunctionComponent<{
  activeTab: TabId;
  onTabClick?(tab: TabId): void;
}> = ({
  activeTab,
  onTabClick
}) => {
  return (
    <Container>
      {TabDescriptions.map(({ tab, name, Icon }) => {
        const isActive = activeTab === tab;

        return (
          <Button key={tab} isActive={isActive} onClick={() => onTabClick?.(tab)}>
            <StyledNavLink to={`/${name}`} activeStyle={{ fontSize: 32 }}>
              <IconContainer><Icon /></IconContainer>
              <TextContainer>{name}</TextContainer>
            </StyledNavLink>
          </Button>
        );
      })}
    </Container>
  );
};
export default NavBar;
