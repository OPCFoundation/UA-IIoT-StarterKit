import React from 'react';
import Box from '@mui/material/Box/Box';
import BrokerView from '../controls/BrokerView';
import { ApplicationContext, IMessage } from '../ApplicationProvider';

export const BrokerPage = () => {
   const context = React.useContext(ApplicationContext);

   return (
      <Box display="flex" p={2} pb={4} sx={{ width: '100%' }}>
         <Box flexGrow={1}>
            <BrokerView onMessage={(topic: string, message: IMessage, isData: boolean) => context.messageReceived(topic, message, isData)} />
         </Box>
      </Box>
   );
};

export default BrokerPage;