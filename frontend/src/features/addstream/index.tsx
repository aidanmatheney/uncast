import React, { FunctionComponent, useState } from 'react';
import { useDispatch } from 'react-redux';
import { useForm } from 'react-hook-form';
import styled from 'styled-components';
import {
  FaPlus,
  FaFileUpload,
  FaRss
} from 'react-icons/fa';
import { IconType } from 'react-icons/lib';

import { subscribe } from '../user/userSlice';
import UserTabId from '../../common/UserTabId';
import AddStreamTabId from '../../common/AddStreamTabId';

const Form = styled.form`
  color: #181818;
`;

const MenuContainer = styled.div`
  display: grid;
  grid-template-columns: 1fr;
  width: 100%;
  background: ${props => props.theme.pageBackground};
`;

const MenuButton = styled.button`
  text-align: center;
  margin: 0.25rem;
  cursor: pointer;
  color: ${props => props.theme.color};
  font-size: 1em;
  width: 525px;
  border: 2px solid ${props => props.theme.borderColor};
  border-radius: 3px;
  background: ${props => props.theme.background};
`;

const AddContainer = styled.div`
  background: ${props => props.theme.addBackground};
  margin: 0.25rem;
  font-size: 1em;
  width: 525px;
`;

const FileTab: {
  tab: AddStreamTabId;
  name: string;
  Icon: IconType;
}[] = [
  {
    tab: AddStreamTabId.AddFile,
    name: 'Add from file',
    Icon: FaFileUpload
  }
];

const RSSTab: {
  tab: AddStreamTabId;
  name: string;
  Icon: IconType;
}[] = [
  {
    tab: AddStreamTabId.AddRss,
    name: 'Add from RSS',
    Icon: FaRss
  }
];

const IconContainer = styled.div``;
const TextContainer = styled.div``;

const TabDescriptions: {
  tab: UserTabId;
  name: string;
  Icon: IconType;
}[] = [
  {
    tab: UserTabId.AddAudio,
    name: 'Add Audio',
    Icon: FaPlus
  }
];

const FileForm: FunctionComponent = () => {
  const { register, handleSubmit } = useForm<{
    files: FileList;
  }>();

  const onSubmit = handleSubmit(({ files }) => {
    console.log('FileFormFC onSubmit', { files });
  });

  return (
    <AddContainer>
      <Form onSubmit={onSubmit}>
        <label>
          Audio File:
          <input type="file" name="files" ref={register} />
        </label>
        <input type="submit" value="Submit" />
      </Form>
    </AddContainer>
  );
};

const RSSForm: FunctionComponent = () => {
  const dispatch = useDispatch();

  const { register, handleSubmit } = useForm<{
    feedUrl: string;
  }>();

  const onSubmit = handleSubmit(({ feedUrl }) => {
    console.log('A name was submitted: ' + feedUrl);

    dispatch(subscribe({
      name: `Podcast${new Date()}`,
      author: 'Author',
      description: 'Description',
      feedUrl: feedUrl
    }));
  });

  return (
    <AddContainer>
    <Form onSubmit={onSubmit}>
      <label>
        RSS Feed Link:
        <input type="text" name="feedUrl" ref={register} />
      </label>
      <input type="submit" value="Submit" />
    </Form>
    </AddContainer>
  );
};

const FileMenu: FunctionComponent = () => {
  const [show, setShow] = useState(false);

  return (
    <MenuContainer>
      {FileTab.map(({ tab, name, Icon }) => (
        <div>
          <MenuButton key={tab} onClick={() => setShow(!show)}>
            <IconContainer><Icon /></IconContainer>
            <TextContainer>{name}</TextContainer>
          </MenuButton>
          {show && <FileForm />}
        </div>
      ))}
    </MenuContainer>
  );
};

const RSSMenu: FunctionComponent = () => {
  const [show, setShow] = useState(false);

  return (
    <MenuContainer>
      {RSSTab.map(({ tab, name, Icon }) => (
        <div>
          <MenuButton key={tab} onClick={() => setShow(!show)}>
            <IconContainer><Icon /></IconContainer>
            <TextContainer>{name}</TextContainer>
          </MenuButton>
          {show && <RSSForm />}
        </div>
      ))}
    </MenuContainer>
  );
};

export const AddStreamMenu: FunctionComponent = () => {
  const [show, setShow] = useState(false);

  return (
    <MenuContainer>
      {TabDescriptions.map(({ tab, name, Icon }) => (
        <div key={tab}>
          <MenuButton key={tab} onClick={() => setShow(!show)}>
            <IconContainer><Icon /></IconContainer>
            <TextContainer>{name}</TextContainer>
          </MenuButton>

          {show && <>
            <FileMenu />
            <RSSMenu />
          </>}
        </div>
      ))}
    </MenuContainer>
  );
};
