import React from 'react';
import LoginPage from './pages/LoginPage';
import BrokerPage from './pages/BrokerPage';

export interface PageRoute {
   name: string
   path: string
   element: React.ReactNode
   appId: number
}

const AppRoutes : PageRoute[] = [
   {
      name: "main.broker",
      path: '/',
      appId: 1,
      element: <BrokerPage />
   },
   {
      name: "main.login",
      path: '/login',
      appId: -1,
      element: <LoginPage />
   }
];

export default AppRoutes;
