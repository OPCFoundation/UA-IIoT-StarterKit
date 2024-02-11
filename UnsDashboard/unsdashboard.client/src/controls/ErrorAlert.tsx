import { Trans } from 'react-i18next';

import { SxProps, Theme } from '@mui/material';
import Alert from '@mui/material/Alert';
import AlertTitle from '@mui/material/AlertTitle';
import Container from '@mui/material/Container';

interface ErrorAlertProps {
   errorCode?: string | null
   errorText?: string | null
   silent?: boolean
   failed?: boolean
   sx?: SxProps<Theme>
}

export const ErrorAlert = ({ errorCode, errorText, silent, failed, sx }: ErrorAlertProps) => {
   if (silent || !errorCode) {
      return null;
   }
   return (
      <Container disableGutters={true} sx={sx}>
         <Alert severity={(failed === undefined || failed) ? "error" : "info"} sx={{ m: 0 }}>
            <AlertTitle>
               <Trans i18nKey={`error.${errorCode}`} values={{ errorCode }} defaults="{{errorCode}}">
                  placeholder
               </Trans>
            </AlertTitle>
            {
               (errorText) ?
                  <Trans i18nKey={`error.${errorText}`} values={{ errorText }} defaults="{{errorText}}">
                     placeholder
                  </Trans> : null
            }
         </Alert>
      </Container>
   );
}

export default ErrorAlert;