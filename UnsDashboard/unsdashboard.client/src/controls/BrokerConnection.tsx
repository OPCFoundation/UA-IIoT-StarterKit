import React from 'react'
import mqtt from 'mqtt';
import { useTranslation } from 'react-i18next';

import StyledDialog from './StyledDialog';
import TableContainer from '@mui/material/TableContainer/TableContainer';
import Table from '@mui/material/Table/Table';
import TableBody from '@mui/material/TableBody/TableBody';
import TableRow from '@mui/material/TableRow/TableRow';
import TableCell from '@mui/material/TableCell/TableCell';
import Paper from '@mui/material/Paper/Paper';
import Typography from '@mui/material/Typography/Typography';
import { MenuItem, Select, SelectChangeEvent, TextField } from '@mui/material';
interface BrokerConnectionProps {
   defaultOpen?: boolean
   onConnect?: (host: string, mqttOptions: mqtt.IClientOptions) => void
}

export const BrokerConnection = ({ defaultOpen,  onConnect }: BrokerConnectionProps) => {
   const [protocol, setProtocol] = React.useState<string>('wss');
   const [host, setHost] = React.useState<string>('broker.hivemq.com');
   const [port, setPort] = React.useState<number>(8884);
   const [clientId, setClientId] = React.useState<string>('opcua_' + Math.random().toString(16).substring(2, 8));
   const [username, setUsername] = React.useState<string | undefined>('');
   const [password, setPassword] = React.useState<string | undefined>('');
   const [open, setOpen] = React.useState<boolean>(false);
   const [closing, setClosing] = React.useState<boolean>(false);
   const { t } = useTranslation();

   React.useEffect(() => {
      setOpen(defaultOpen ?? false)
   }, [defaultOpen]);

   React.useEffect(() => {
      setPort(protocol === 'wss' ? 8884 : 8000)
   }, [protocol]);

   const handleConnect = () => {
      if (onConnect) {
         const url = `${protocol}://${host}:${port}/mqtt`
         const options = {
            clientId,
            username,
            password,
            clean: true,
            reconnectPeriod: 1000, // ms
            connectTimeout: 30 * 1000, // ms
         }
         setClosing(true); 
         onConnect(url, options);
         setOpen(false);
      }
   }

   return (
      <StyledDialog
         title={'broker.connection'}
         open={open}
         disabled={closing}
         onOk={handleConnect}
         onCancel={() => setOpen(false)}
         okText={'broker.connect'}
         cancelText={'broker.cancel'}
      >
         <TableContainer component={Paper}>
            <Table size="small" sx={{ borderBottom: `0px` }}>
               <TableBody>
                  <TableRow>
                     <TableCell style={{ whiteSpace: 'nowrap' }}><Typography variant="bold" component="label">{t('broker.protocol')}</Typography></TableCell>
                     <TableCell style={{ width: '100%' }}>
                        <Select
                           labelId="protocol-select-label"
                           id="protocol-select"
                           value={protocol}
                           label="Protocol"
                           onChange={(event: SelectChangeEvent) => setProtocol(event.target.value as string)}
                        >
                           <MenuItem value={'ws'}>ws</MenuItem>
                           <MenuItem value={'wss'}>wss</MenuItem>
                        </Select>
                     </TableCell>
                  </TableRow>
                  <TableRow>
                     <TableCell style={{ whiteSpace: 'nowrap' }}><Typography variant="bold" component="label">{t('broker.host')}</Typography></TableCell>
                     <TableCell style={{ width: '100%' }}>
                        <TextField
                           id="host-text"
                           label="host-text-label"
                           value={host}
                           onChange={(event: React.ChangeEvent<HTMLInputElement>) => setHost(event.target.value)}
                        />
                     </TableCell>
                  </TableRow>
                  <TableRow>
                     <TableCell style={{ whiteSpace: 'nowrap' }}><Typography variant="bold" component="label">{t('broker.port')}</Typography></TableCell>
                     <TableCell style={{ width: '100%' }}>
                        <TextField
                           id="port-text"
                           label="port-text-label"
                           value={port}
                           onChange={(event: React.ChangeEvent<HTMLInputElement>) => setPort(Number(event.target.value))}
                        />
                     </TableCell>
                  </TableRow>
                  <TableRow>
                     <TableCell style={{ whiteSpace: 'nowrap' }}><Typography variant="bold" component="label">{t('broker.clientId')}</Typography></TableCell>
                     <TableCell style={{ width: '100%' }}>
                        <TextField
                           id="clientId-text"
                           label="clientId-text-label"
                           value={clientId}
                           onChange={(event: React.ChangeEvent<HTMLInputElement>) => setClientId(event.target.value)}
                        />
                     </TableCell>
                  </TableRow>
                  <TableRow>
                     <TableCell style={{ whiteSpace: 'nowrap' }}><Typography variant="bold" component="label">{t('broker.username')}</Typography></TableCell>
                     <TableCell style={{ width: '100%' }}>
                        <TextField
                           id="username-text"
                           label={t('broker.username')}
                           value={username}
                           onChange={(event: React.ChangeEvent<HTMLInputElement>) => setUsername(event.target.value)}
                        />
                     </TableCell>
                  </TableRow>
                  <TableRow>
                     <TableCell style={{ whiteSpace: 'nowrap' }}><Typography variant="bold" component="label">{t('broker.password')}</Typography></TableCell>
                     <TableCell style={{ width: '100%' }}>
                        <TextField
                           id="password-text"
                           label={t('broker.password')}
                           value={password}
                           onChange={(event: React.ChangeEvent<HTMLInputElement>) => setPassword(event.target.value)}
                        />
                     </TableCell>
                  </TableRow>
               </TableBody>
            </Table>
         </TableContainer>
      </StyledDialog>
   )
}
