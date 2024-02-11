import * as React from 'react';
import { useTranslation } from 'react-i18next';
import { Link } from "react-router-dom";

import MenuIcon from '@mui/icons-material/Menu';
import Box from '@mui/material/Box';
import Button from '@mui/material/Button';
import IconButton from '@mui/material/IconButton';
import Menu from '@mui/material/Menu';
import MenuItem from '@mui/material/MenuItem';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import SettingsMenu from './SettingsMenu';

import { PageRoute } from "../AppRoutes";

interface TopMenuProps {
   appId?: number
   pages?: PageRoute[] | null
   title?: string | null
   subtitle?: string | null
}

export const TopMenu = ({ appId, pages, title, subtitle }: TopMenuProps) => {
   const path = window.location.pathname;
   const [menuAnchor, setMenuAnchor] = React.useState<HTMLButtonElement | null>(null);
   const { t } = useTranslation();

   const onOpen = (e: React.MouseEvent<HTMLButtonElement>) => {
      e.preventDefault();
      setMenuAnchor(e.currentTarget);
   }

   const onClose = () => {
      setMenuAnchor(null);
   };

   return (
      <Toolbar disableGutters sx={{ py: 0, minHeight: '52px' }}>
         {(pages?.filter(x => x.appId === appId)?.length) ? (
            <Box sx={{ flexGrow: 1, display: { xs: 'flex', md: 'none' } }}>
               <IconButton
                  size="large"
                  aria-haspopup="true"
                  onClick={onOpen}
                  color="inherit"
               >
                  <MenuIcon />
               </IconButton>
               <Menu
                  id="menu-appbar"
                  anchorEl={menuAnchor}
                  anchorOrigin={{
                     vertical: 'bottom',
                     horizontal: 'left',
                  }}
                  keepMounted
                  transformOrigin={{
                     vertical: 'top',
                     horizontal: 'left',
                  }}
                  open={Boolean(menuAnchor)}
                  onClose={onClose}
                  sx={{
                     display: { xs: 'block', md: 'none' },
                  }}
               >
                  {pages?.map((page) => {
                     if (page.appId !== appId) {
                        return null;
                     }
                     return (
                        <MenuItem key={page.name}>
                           <Link to={page.path}>{t(page.name)}</Link>
                        </MenuItem>
                     );
                  })}
               </Menu>
            </Box>) : null}
         <Typography
            variant="h6"
            noWrap
            component="p"
            sx={{
               px: 6,
               display: { xs: 'flex', md: 'none' },
               flexGrow: 1
            }}
         >
            {title}
         </Typography>
         <Box ml={6} my={0} pt={2} sx={{ flexGrow: 0, display: { xs: 'none', md: 'flex' } }}>
            <img src='/logo.png' alt='Logo' height={50} style={{
               borderTop: '1px solid white',
               borderLeft: '1px solid white',
               borderBottom: '1px solid black',
               borderRight: '1px solid black'
            }} />
         </Box>
         <Box ml={6} mr="auto" sx={{ flexGrow: 1, display: { xs: 'none', md: 'flex' } }}>
            <Box ml={2}>
               <Typography variant="subtitle1" component="div" noWrap>
                  {title}
               </Typography>
               <Typography variant="body1" component="div" sx={{ fontWeight: 'lighter', fontSize: 'smaller' }} noWrap>
                  {`${(subtitle?.length)?subtitle:''}`}
               </Typography>
            </Box>
         </Box>
         <Box ml={6} sx={{ flexGrow: 0, display: { xs: 'none', md: 'flex' } }}>
            {pages?.map((page) => {
               if (page.appId !== appId) {
                  return null;
               }
               return (
                  <Button
                     className={(page.path === path) ? "selected" : undefined}
                     key={page.name}
                     disableRipple={false}
                     sx={{ my: 2, color: 'white', display: 'flex' }}
                  >
                     <Link to={page.path}>{t(page.name)}</Link>
                  </Button>
               );
            })}
         </Box>
         <Box sx={{ flexGrow: 0 }}>
            <SettingsMenu />
         </Box>
      </Toolbar>
   );
}
