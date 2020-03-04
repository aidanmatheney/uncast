import React, { ComponentType } from 'react';
import ReactDOM from 'react-dom';
import * as serviceWorker from './serviceWorker';

import App from './app/App';

import './index.css';

declare global {
  interface NodeModule {
    hot?: {
      accept(dependencies: string | string[], callback: () => void): void;
    };
  }
};

const render = (Component: ComponentType) => {
  ReactDOM.render(<Component />, document.getElementById('root'));
};

render(App);

if (module.hot) {
  module.hot.accept('./app/App', async () => {
    const NextApp = (await import('./app/App')).default;
    render(NextApp);
  });
}

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();
