import React, { FunctionComponent } from "react";
import styled, { css } from "styled-components";

import { TabId } from '../../TabId';

import {
  FaHome,
  FaCloud,
  FaUser
} from 'react-icons/fa';
import { IconType } from 'react-icons/lib'

const Container = styled.div`
  background: #35369A;

  display: grid;
  grid-template-columns: 1fr 1fr 1fr;
`;


const Button = styled.button<{ isActive: boolean }>`
  text-align: center;
  margin: 0.25rem;
  cursor: pointer;

  ${({ isActive }) => isActive && css`font-size: 2em;`}
`;

const IconContainer = styled.div``;
const TextContainer = styled.div``;


const TabDescriptions: {
  tab: TabId,
  name: string,
  Icon: IconType
}[] = [
  {
    tab: 'library',
    name: 'Library',
    Icon: FaHome
  },
  {
    tab: 'catalog',
    name: 'Catalog',
    Icon: FaCloud
  },
  {
    tab: 'profile',
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
          <Button isActive={isActive} onClick={() => onTabClick?.(tab)}>
            <IconContainer><Icon /></IconContainer>
            <TextContainer>{name}</TextContainer>
          </Button>
        );
      })}
    </Container>
  );
};

export default NavBar;
