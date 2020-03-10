// TypeScript: https://github.com/marmelab/react-admin/issues/1617#issuecomment-595874113

declare module 'react-admin' {
  import { FunctionComponent } from 'react';
  import { History } from 'history';
  import { AdminProps, ResourceProps } from 'ra-core';
  import { FieldProps } from 'ra-ui-materialui/src/field/types';

  export declare const adminReducer: any;
  export declare const adminSaga: any;

  export declare const Admin: FunctionComponent<AdminProps>;
  export declare const Resource: FunctionComponent<ResourceProps>;
  export declare const ListGuesser: FunctionComponent<{}>;
  export declare const List: FunctionComponent<{}>;
  export declare const Create: FunctionComponent<{}>;
  export declare const EditGuesser: FunctionComponent<{}>;
  export declare const Edit: FunctionComponent<{}>;

  export declare const Datagrid: FunctionComponent<{
    rowClick: string | (() => void);
  }>;
  export declare const UrlField: FunctionComponent<FieldProps>;
  export declare const TextField: FunctionComponent<FieldProps>;
  export declare const ReferenceField: FunctionComponent<FieldProps & { reference: string }>;

  export declare const fetchUtils: {
    fetchJson(url: string, options: Options): Promise<{ status: number; headers: Headers; body: string; json: any; }>;
  };
}
