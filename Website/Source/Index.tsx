import ReactDOM from 'react-dom';
import React from 'react';
import { App } from './App';

// eslint-disable-next-line @typescript-eslint/no-unused-vars
import i18n from './Utils/I18N';

import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min';
import 'bootstrap-icons/font/bootstrap-icons.css';
import './Index.css';

const app = document.getElementById('app');
ReactDOM.render(<App />, app);
