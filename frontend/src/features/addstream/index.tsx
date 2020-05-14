import React, { FunctionComponent, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useForm } from 'react-hook-form';
import styled from 'styled-components';
import {
  FaPlus,
  FaFileUpload,
  FaRss
} from 'react-icons/fa';
import { IconType } from 'react-icons/lib';

import UserTabId from '../../common/UserTabId';
import AddStreamTabId from '../../common/AddStreamTabId';
import { subscribeToPodcast, addFilePodcastEpisode } from '../podcast/podcastSlice';
import { FileEpisode, FilePodcast } from '../../common/entities';
import { RootState } from '../../app/rootReducer';

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

function onChange(event: any) {
  var file = event.target.files[0];
  var reader = new FileReader();
  reader.onload = function(event) {
    // The file's text will be printed here
    //console.log(event.target.result)
  };

  reader.readAsText(file);
}

const FileForm: FunctionComponent = () => {
  const dispatch = useDispatch();

  const podcastById = useSelector((state: RootState) => state.podcast.podcastById);
  const filePodcasts = Object.values(podcastById).filter(podcast => podcast.type === 'file') as FilePodcast[];

  const { register, handleSubmit } = useForm<{
    files: FileList;
    episodeName: string;
    existingPodcastId: string;
  }>();

  const onSubmit = handleSubmit(({ files, episodeName, existingPodcastId }) => {
    if (files.length === 0) {
      return;
    }

    const file = files[0];

    console.log('FileForm submit:', { file, episodeName, existingPodcastId })

    const reader = new FileReader();
    reader.onload = () => {
      const dataUrl = reader.result as string;

      dispatch(addFilePodcastEpisode({
        dataUrl,
        name: episodeName,
        podcast: existingPodcastId === ''
          ? { type: 'new', name: 'New Custom' }
          : { type: 'existing', id: existingPodcastId }
      }));
    };
    reader.readAsDataURL(file);
  });

  return (
    <AddContainer>
      <Form onSubmit={onSubmit}>
        <div>
          <label>
            Audio File:
            <input type="file" name="files" ref={register} />
          </label>
        </div>
        <div>
          <label>
            Episode name:
            <input name="episodeName" ref={register} />
          </label>
        </div>
        <div>
          <label>
            Existing podcast:
            <select name="existingPodcastId" ref={register}>
              <option />
              {filePodcasts.map(podcast => (
                <option key={podcast.id} value={podcast.id}>{podcast.name}</option>
              ))}
            </select>
          </label>
        </div>
        <div><input type="submit" value="Submit" /></div>
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

    dispatch(subscribeToPodcast({ feedUrl }));
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
        <div key={tab}>
          <MenuButton onClick={() => setShow(!show)}>
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
        <div key={tab}>
          <MenuButton onClick={() => setShow(!show)}>
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
