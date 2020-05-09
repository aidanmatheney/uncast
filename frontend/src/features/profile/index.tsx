import React, { FunctionComponent } from 'react';
import styled from 'styled-components';

const Container = styled.div`
  background: ${props => props.theme.pgBgColor};
  color: ${props => props.theme.color};
  height: 100%;

  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(8rem, 1fr));
  align-content: start;
`;

const Profile: FunctionComponent = () => {


  return (
    <Container>PROFILE TEST</Container>
  );
};

export default Profile;
