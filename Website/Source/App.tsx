import React from 'react';
import { HashRouter, Route, Switch } from 'react-router-dom';
import { HomePage } from './Pages/HomePage';
import { CallbackPage } from './Pages/CallbackPage';
import { DashboardPage } from './Pages/DashboardPage';
import { LoginPage } from './Pages/LoginPage';
import { LogoutPage } from './Pages/LogoutPage';

export const App: React.FunctionComponent<any> = () => {
  return (
    <HashRouter hashType="noslash">
      <Switch>
        <Route exact path="/" component={HomePage} />
        <Route path="/callback" component={CallbackPage} />
        <Route path="/dashboard" component={DashboardPage} />
        <Route path="/login" component={LoginPage} />
        <Route path="/logout" component={LogoutPage} />
      </Switch>
    </HashRouter>
  );
};
