import { useTranslation } from 'react-i18next';
import { Route, Routes } from 'react-router-dom';
import './App.css';
import Layout from './layout/Layout';
import AppRoutes from './AppRoutes';
import React from 'react';

function App() {
   const [appId, setAppId] = React.useState<number>(0);
   const [title, setTitle] = React.useState<string>('main.title');
   const [subtitle, setSubtitle] = React.useState<string>('main.subtitle');
   const path = window.location.pathname;
   const { t } = useTranslation();

   React.useEffect(() => {
      document.title = t(title);
   }, [t, title]);

   React.useEffect(() => {
      setAppId(1);
      setTitle('main.title');
      setSubtitle('main.subtitle');
   }, [path]);

   return (
      <Layout topMenu={AppRoutes} appId={appId} title={t(title)} subtitle={t(subtitle)}>
         <Routes>
            {AppRoutes.map((route, index) => {
               const { element, ...rest } = route;
               return <Route key={index} {...rest} element={element} />;
            })}
         </Routes>
      </Layout>
   );
}

export default App;