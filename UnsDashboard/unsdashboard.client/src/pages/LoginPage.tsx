import * as React from 'react';
import { Trans } from 'react-i18next';
import { useLocation } from 'react-router-dom';

import Container from '@mui/material/Container';
import Typography from '@mui/material/Typography';

import ErrorAlert from '../controls/ErrorAlert';
import { UserContext } from '../UserProvider';

export default function LoginPage() {
   const context = React.useContext(UserContext);
   const query = new URLSearchParams(useLocation().search);
   const url = query.get("url");
   const status = query.get("status");
   const error = query.get("error");
   const errorText = query.get("error_text");

   React.useEffect(() => {
      if (context) {
         context.setLoginStatus(Number(status));
         if (!error && url) {
            window.location.href = url;
         }
      }
   }, [context, status, error, url]);

   return (
      <Container sx={{ mt: 10 }} >
         <ErrorAlert errorCode={error} errorText={errorText} />
         <Trans i18nKey="main.loginError">
            <Typography sx={{ mt: 10 }} variant="body1">
               <a href="mailto:webmaster@opcfoundation.org?subject=Login%20Issues">placeholder</a>
            </Typography>
         </Trans>
      </Container>
   );
}
