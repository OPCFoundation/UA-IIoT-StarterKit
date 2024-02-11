import * as React from 'react';
import i18n from "i18next";

import { ThemeProvider } from '@mui/material/styles';

import { DarkTheme, LightTheme, ThemeModes } from './theme';
import { Account, UserLoginStatus } from './api';

import * as Web from "./Web";

export const DefaultUserName = 'Anonymous';

export interface IUserContext {
   user: Account,
   setUser: (value: Account) => void,
   loginStatus: UserLoginStatus,
   setLoginStatus: (value: UserLoginStatus) => void,
   themeMode?: ThemeModes,
   setThemeMode: (value: ThemeModes) => void,
   language: string,
   setLanguage: (value: string) => void,
}

export const UserContext = React.createContext<IUserContext>({
   user: {},
   setUser: () => {},
   loginStatus: UserLoginStatus.Unknown,
   setLoginStatus: () => {},
   themeMode: ThemeModes.Light,
   setThemeMode: () => {},
   language: 'en',
   setLanguage: () => {}
});

interface UserProviderProps {
   children?: React.ReactNode
}

export const UserProvider = ({ children }: UserProviderProps) => {
   const [user, setUser] = React.useState<Account | null>({ name: DefaultUserName });
   const [loginStatus, setLoginStatus] = React.useState<UserLoginStatus>(UserLoginStatus.Unknown);
   const [themeMode, setThemeMode] = React.useState(localStorage.getItem('themeMode') || ThemeModes.Light);
   const [language, setLanguage] = React.useState(localStorage.getItem('language') || 'en');

   const theme = (themeMode === ThemeModes.Light) ? LightTheme : DarkTheme;

   React.useEffect(() => {
      console.log(`UserProvider: ${user?.name}`);
   }, [user]);

   const userContext = {
      user,
      setUser,
      loginStatus,
      setLoginStatus,
      themeMode,
      setThemeMode,
      language,
      setLanguage
   } as IUserContext;

   React.useEffect(() => {
      localStorage.setItem('themeMode', themeMode);
   }, [themeMode]);

   React.useEffect(() => {
      localStorage.setItem('language', language);
      i18n.changeLanguage(language);
   }, [language]);

   React.useEffect(() => {
      const controller = new AbortController();
      if (UserLoginStatus.LoggedIn === loginStatus || UserLoginStatus.Unknown === loginStatus) {
         fetch();
      }
      async function fetch() {
         const response = await Web.httpGet(`/api/account/current`, controller);
         if (response?.errorCode) {
            if (!response.silent) {
               setUser({ name: DefaultUserName });
               setLoginStatus(UserLoginStatus.LoggedOut);
               console.error(`Error logging in: ${response.errorCode}: ${response.errorText}`);
            }
         }
         else {
            setUser(response?.result);
         }
      }
      return function cleanup() {
         controller.abort();
      };
   }, [loginStatus]);

   return (
      <UserContext.Provider value={userContext}>
         <ThemeProvider theme={theme}>{children}</ThemeProvider>
      </UserContext.Provider>
   );
};

export default UserProvider;