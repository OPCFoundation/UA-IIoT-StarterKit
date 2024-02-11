import React from 'react';

import { useTheme } from '@mui/material/styles';
import Box from '@mui/material/Box';
import Button from '@mui/material/Button';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography/Typography';
import Link from '@mui/material/Link/Link';

import { BuildVersion } from '../version';
import { UserContext } from '../UserProvider';

export const Footer = () => {
   const context = React.useContext(UserContext);
   const theme = useTheme();
   return (
      <Toolbar variant='dense' disableGutters sx={{ py: 0, minHeight: '36px', justifyContent: 'space-between' }}>
         <Box ml={6} sx={{ flexGrow: 0, display: { xs: 'none', color: theme.palette.text.primary, md: 'flex' } }}>
            <Button sx={{ mr: 2 }}>
               <Link href='https://opcfoundation.org/about/what-is-opc/'>
                  <Typography variant='body2'>{`© OPC Foundation ${new Date().getFullYear()}`}</Typography>
               </Link>
            </Button>
            <Button sx={{ my: 2 }}>
               <Link href='https://opcfoundation.org/about/contact-us/'>
                  <Typography variant='body2'>Contact Us</Typography>
               </Link>
            </Button>
            <Button sx={{ my: 2 }}>
               <Link href='https://opcfoundation.org/membership/become-a-member/step1'>
                  <Typography variant='body2'>Become a Member</Typography>
               </Link>
            </Button>
            <Button sx={{ my: 2 }}>
               <Link href='https://opcfoundation.org/privacy-policy/'>
                  <Typography variant='body2'>Privacy Policy</Typography>
               </Link>
            </Button>
         </Box>
         <Box
            alignContent='right'
            textAlign='right'
            mx={6} 
            sx={{
               display: 'flex',
               alignItems: 'center'
            }}
         >   <Button sx={{ mr: 2 }}>
               <Link href={`mailto:webmaster@opcfoundation.org?subject=OPC%20Foundation%20Utilities%20App%20Problem%20Report&body=User%3A%20${context?.user?.id}%0D%0APage%3A%20${window.location.href}%0D%0A`}>
                  <Typography variant='body2'>Report Problems</Typography>
               </Link>
            </Button>
            <Typography variant='body2'>{BuildVersion}</Typography>
         </Box>
      </Toolbar>
   );
}
