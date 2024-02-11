import * as React from 'react';

import { Button, Grid, useTheme } from '@mui/material';
import Box from '@mui/material/Box';
import Modal from '@mui/material/Modal';
import Stack from '@mui/material/Stack/Stack';
import Typography from '@mui/material/Typography/Typography';
import { useTranslation } from 'react-i18next';
import ErrorAlert from './ErrorAlert';
import { ErrorInfo } from '../api';

const style = {
   display: 'flex',
   alignItems: 'left',
   justifyContent: 'left',
   position: 'absolute',
   top: '50%',
   left: '50%',
   transform: 'translate(-50%, -50%)',
   minWidth: 600,
   minHeight: 100,
   maxHeight: 800,
   bgcolor: 'background.paper',
   border: '4px solid #084A79',
   boxShadow: 24,
   p: 0
};

interface StyledDialogProps {
   children?: React.ReactNode,
   title?: string
   okText?: string
   cancelText?: string
   open?: boolean
   disabled?: boolean
   error?: ErrorInfo
   onOk?: (controller: AbortController) => void
   onCancel?: () => void;
}

const StyledDialog: React.FC<StyledDialogProps> = ({
   children,
   title,
   okText,
   cancelText,
   open,
   disabled,
   error,
   onOk,
   onCancel }: StyledDialogProps
) => {
   const [controller, setController] = React.useState<AbortController | null>(null);
   const theme = useTheme();
   const { t } = useTranslation();

   React.useEffect(() => {
      return () => {
         controller?.abort();
      };
   }, [controller]);

   React.useEffect(() => {
      if (controller && !open) {
         setController(null);
      }
   }, [open, controller]);

   const handleOk = async () => {
      if (onOk) {
         const ac = new AbortController();
         setController(ac);
         onOk(ac);
      }
   }

   const handleCancel = () => {
      if (onCancel) {
         onCancel();
      }
      controller?.abort();
   };

   return (
      <Modal
         className={controller ? 'waiting' : ''}
         open={open ?? false}
         onClose={() => handleCancel()}
      >
         <Stack sx={style}>
            <Box sx={{ m: 0, p: 5, mb: 10, backgroundColor: theme.palette.primary.main, color: theme.palette.primary.contrastText }}>
               <Typography component="p">{t(title ?? 'dialog.title')}</Typography>
            </Box>
            <ErrorAlert {...error} sx={{ mb: 10, mt: -10 }} />
            {children}
            <Grid container spacing={4} sx={{ p: 5, justifyContent: 'flex-end' }}>
               {(onOk) ? <Grid item><Button variant="contained" disabled={disabled} onClick={() => handleOk()}>{t(okText ?? 'dialog.ok')}</Button></Grid> : null}
               <Grid item><Button variant="contained" onClick={() => handleCancel()}>{t(cancelText ?? 'dialog.cancel')}</Button></Grid>
            </Grid>
         </Stack>
      </Modal>
   );
}

export default StyledDialog;