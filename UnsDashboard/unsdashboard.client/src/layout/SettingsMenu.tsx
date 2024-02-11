import * as React from 'react';
import { useTranslation } from 'react-i18next';

import BusinessIcon from '@mui/icons-material/Business';
import LogoutIcon from '@mui/icons-material/Logout';
import PersonIcon from '@mui/icons-material/Person';
import DarkModeOutlinedIcon from '@mui/icons-material/DarkModeOutlined';
import TranslateIcon from '@mui/icons-material/Translate';

import Avatar from '@mui/material/Avatar';
import Box from '@mui/material/Box';
import Button from '@mui/material/Button';
import Drawer from '@mui/material/Drawer';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemButton from '@mui/material/ListItemButton';
import ListItemIcon from '@mui/material/ListItemIcon';
import ListItemText from '@mui/material/ListItemText';
import ListSubheader from '@mui/material/ListSubheader';
import Toolbar from '@mui/material/Toolbar';

import { FormControl, InputLabel, MenuItem, Select, SelectChangeEvent, Switch } from '@mui/material';

import { UserContext } from '../UserProvider';
import { ThemeModes } from '../theme';
import { SupportedLanguages } from '../i18n';
export default function SettingsMenu() {
   const context = React.useContext(UserContext);
   const [open, setOpen] = React.useState(false);
   const [language, setLanguage] = React.useState(context.language ?? 'en');
   const user = context.user;
    const darkMode = context.themeMode === ThemeModes.Dark;
   const { t } = useTranslation();

   const onThemeChange = () => {
      context.setThemeMode((darkMode) ? ThemeModes.Light : ThemeModes.Dark);
   };

   const onLanguageChange = (event: SelectChangeEvent) => {
      setLanguage(() => {
         const newLanguage = event.target.value as string;
         context.setLanguage(newLanguage);
         return newLanguage;
      });
   };

   const toggleKeyHandler = (open: boolean) => (e: React.KeyboardEvent<HTMLDivElement>) => {
      if (e.type === 'keydown' && (e.key === 'Tab' || e.key === 'Shift')) {
         return;
      }
      setOpen(open);
   };

   const toggleMouseHandler = (open: boolean) => () => {
      setOpen(open);
   };

   const login = () => {
      context.setLoginStatus(1);
      window.location.href = `/api/account/login?context=${window.location.pathname}`;
   };

   const logout = () => {
      context.setLoginStatus(0);
      window.location.href = `/api/account/logout?context=${window.location.pathname}`;
   };

   const list = () => (
      <Box
         sx={{ width: 350 }}
         role="presentation"
         onKeyDown={toggleKeyHandler(false)}
      >
         <List subheader={<ListSubheader>{t('main.account')}</ListSubheader>}>
            <ListItem>
               <ListItemIcon>
                  <PersonIcon />
               </ListItemIcon>
               {(user?.id) ?
                  <ListItemText primary={user?.name} />
                  :
                  <ListItemButton onClick={() => login()}>
                     <ListItemText>{t('main.login')}</ListItemText>
                  </ListItemButton>
               }
            </ListItem>
            {(user?.id) ?
               <ListItem>
                  <ListItemIcon>
                     <BusinessIcon />
                  </ListItemIcon>
                  <ListItemText primary={user?.companyName} />
               </ListItem> : null
            }
            {(user?.id) ?
               <ListItem>
                  <ListItemIcon>
                     <LogoutIcon />
                  </ListItemIcon>
                  <ListItemButton onClick={() => logout()}>
                     <ListItemText>{t('main.logout')}</ListItemText>
                  </ListItemButton>
               </ListItem> : null
            }
         </List>
         <List subheader={<ListSubheader>{t('main.settings')}</ListSubheader>}>
            <ListItem>
               <ListItemIcon>
                  <DarkModeOutlinedIcon />
               </ListItemIcon>
               <ListItemText id="switch-list-label-darkmode" primary={t('main.darkMode')} />
               <Switch
                  edge="end"
                  onChange={onThemeChange}
                  checked={darkMode}
                  inputProps={{
                     'aria-labelledby': 'switch-list-label-darkmode',
                  }}
               />
            </ListItem>
            <ListItem>
               <ListItemIcon>
                  <TranslateIcon />
               </ListItemIcon>
               <FormControl sx={{ m: 0 }}>
                  <InputLabel id="select-language-label">{t('main.language')}</InputLabel>
                  <Select
                     id="select-language"
                     labelId='select-language-label'
                     label={t('main.language')}
                     value={language}
                     onChange={onLanguageChange}
                  >
                     {SupportedLanguages.map(x => {
                        return <MenuItem key={x} value={x}>{t(`main.languages.${x}`)}</MenuItem>
                     })}
                  </Select>
               </FormControl>
            </ListItem>
         </List>
      </Box>
   );

   return (
      <div>
         <Button
            onClick={toggleMouseHandler(true)}
            sx={{ my: 2, display: 'flex', borderRightWidth: '0px', minWidth: '0px' }}
         >
            <Avatar sx={{ height: '24px', width: '24px' }}>
               <PersonIcon />
            </Avatar>
         </Button>
         <Drawer
            anchor={'right'}
            open={open}
            onClose={toggleMouseHandler(false)}
         >
            <Toolbar />
            {list()}
         </Drawer>
      </div>
   );
}
