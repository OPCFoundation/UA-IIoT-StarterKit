import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';

import Container from '@mui/material/Container/Container';
import { PageRoute } from '../AppRoutes';
import { Footer } from './FooterMenu';
import { TopMenu } from './TopMenu';
import ApplicationProvider from '../ApplicationProvider';

interface LayoutProps {
   appId?: number
   topMenu?: PageRoute[]
   title?: string
   subtitle?: string
   children?: React.ReactNode
}

export const Layout = ({ appId, topMenu, title, subtitle, children }: LayoutProps) => {
   return (
      <Container sx={{ display: 'flex', flexDirection: 'column', m: 0, p: 0, width: '100%' }}>
         <AppBar position="fixed" sx={{ zIndex: (theme) => theme.zIndex.drawer + 1 }}>
            <TopMenu
               appId={appId}
               pages={topMenu}
               title={title}
               subtitle={subtitle}
            />
         </AppBar>
         <Box component="main" sx={{ flex: '1 0 0', p: 0, m: 0, minHeight: '100vh', minWidth: '100vw' }}>
            <Toolbar />
            <ApplicationProvider>
               {children}
            </ApplicationProvider>
            <Toolbar variant='dense' disableGutters sx={{ py: 0, minHeight: '36px' }} />
         </Box>
         <AppBar position="fixed" sx={{ top: 'auto', bottom: 0 }}>
            <Footer />
         </AppBar>
      </Container>
   );
}

export default Layout;
