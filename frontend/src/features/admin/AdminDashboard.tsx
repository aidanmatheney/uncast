import React, { FunctionComponent } from 'react';
import {
  Admin,
  Resource,
  ListGuesser,
  EditGuesser,
  fetchUtils,
  List,
  Datagrid,
  TextField,
  ReferenceField
} from 'react-admin';
import simpleRestProvider from 'ra-data-simple-rest';
import { apiUrl } from '../../common/constants';
import { history } from '../../app/createStore';
import { store } from '../../app/App';

export const LibraryRssPodcastList = (props: any) => (
  <List {...props}>
    <Datagrid rowClick="edit">
      <TextField source="id" />
      <TextField source="name" />
      <TextField source="author" />
      <TextField source="description" />
      <ReferenceField source="thumbnailFileId" reference="thumbnailFiles">
        <TextField source="id" />
      </ReferenceField>
      <TextField source="feedUrl" />
    </Datagrid>
  </List>
);

export const dataProvider = simpleRestProvider(`${apiUrl}/Api`, async (url, options = { }) => {
  const token = store.getState().authentication.user?.access_token;
  const response = await fetchUtils.fetchJson(
    url.replace('/Api/Admin/', '/Api/'),
    {
      ...options,
      headers: new Headers({
        ...options.headers,
        Accept: 'application/json',
        Authorization: `Bearer ${token}`
      })
    }
  );

  return {
    ...response,
    headers: {
      ...response.headers,
      has: () => true,
      get: () => '/1',
    }
  };
});

const AdminDashboard: FunctionComponent = () => {
  return (
    <Admin dataProvider={dataProvider} history={history}>
      <Resource name="Admin/Podcasts/Library/Rss" list={LibraryRssPodcastList} edit={EditGuesser} />
      <Resource name="Admin/Podcasts/Library/YouTube" list={ListGuesser} />
    </Admin>
  );
};

export default AdminDashboard;
