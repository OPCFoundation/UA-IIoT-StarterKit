import React from 'react';
import mqtt from 'mqtt';
import { QoS } from 'mqtt-packet';
import { useTranslation } from 'react-i18next';

import Paper from '@mui/material/Paper/Paper';
import Typography from '@mui/material/Typography/Typography';
import { BrokerConnection } from './BrokerConnection';
import Button from '@mui/material/Button/Button';
import Box from '@mui/material/Box/Box';
import Alert from '@mui/material/Alert/Alert';
import { ApplicationContext, IConnectionMessage, IMessage } from '../ApplicationProvider';
import Accordion from '@mui/material/Accordion/Accordion';
import AccordionSummary from '@mui/material/AccordionSummary/AccordionSummary';
import AccordionDetails from '@mui/material/AccordionDetails/AccordionDetails';
import { Table, TableBody, TableCell, TableHead, TableRow, useTheme } from '@mui/material';
import ArrowDropDownIcon from '@mui/icons-material/ArrowDropDown';

import * as Web from '../Web';
import * as OpcUa from '../opcua';
import BrokerTopicTree from './BrokerTopicTree';

interface BrokerViewProps {
   onMessage?: (topic: string, message: IMessage, isData: boolean) => void
}

export const BrokerView = ({ onMessage }: BrokerViewProps) => {
   const [connectStatus, setConnectStatus] = React.useState<string | undefined>('Disconnected');
   const [connecting, setConnecting] = React.useState<boolean>(false);
   const [messages, setMessages] = React.useState<Map<string, IMessage>>(new Map<string, IMessage>);
   const context = React.useContext(ApplicationContext);
   const { t } = useTranslation();
   const theme = useTheme();

   const subscribe = (client: mqtt.MqttClient, subscription: { topic: string, qos: QoS }) => {
      client.subscribe(subscription.topic, { qos: subscription.qos }, (error) => {
         if (error) {
            console.log('Subscribe to topics error', error);
         }
         else {
            console.log(`Subscribed to '${subscription.topic}'.`);
         }
      });
   };

   const subscribeToMetadata = (client: mqtt.MqttClient, message: IConnectionMessage, topics : Map<string, boolean>) => {
      const newTopics: Map<string, boolean> = new Map();
      if (message?.Connection?.WriterGroups?.length) {
         message.Connection.WriterGroups.map((group: OpcUa.WriterGroupDataType) => {
            if (group?.DataSetWriters?.length) {
               group.DataSetWriters.map((writer: OpcUa.DataSetWriterDataType) => {
                  if (writer?.TransportSettings?.Body) {
                     const settings = writer?.TransportSettings?.Body as OpcUa.BrokerDataSetWriterTransportDataType;
                     if (settings.QueueName) {
                        newTopics.set(settings.QueueName, true);
                     }
                     if (settings.MetaDataQueueName) {
                        newTopics.set(settings.MetaDataQueueName, false);
                     }
                  }
                  return writer;
               });
               const settings = group?.TransportSettings?.Body as OpcUa.BrokerWriterGroupTransportDataType;
               if (settings.QueueName) {
                  newTopics.set(settings.QueueName, true);
               }
            }
            return group;
         });
      }
      if (client) {
         Array.from(newTopics.entries()).forEach((topic) => {
            if (!topics.has(topic[0])) {
               client.subscribe(topic[0], { qos: 1 }, (error) => {
                  if (error) {
                     console.log(`Subscribe to ${(topic[1]) ? 'data' : 'metadata'} topic '${topic}' error.`, error);
                  }
                  else {
                     console.log(`Subscribed to ${(topic[1]) ? 'data' : 'metadata'} '${topic}'.`);
                     topics.set(topic[0], topic[1]);
                  }
               });
            }
         });
      }
   };

   const handleConnect = (host: string, mqttOptions: mqtt.IClientOptions) => {
      setConnecting(false);
      setConnectStatus('Connecting');
      const client = mqtt.connect(host, mqttOptions)
      const topics = new Map<string, boolean>();

      if (client) {
         // https://github.com/mqttjs/MQTT.js#event-connect
         client.on('connect', () => {
            setConnectStatus('Connected');
            console.log('connection successful');
            subscribe(client, { topic: 'opcua/json/status/#', qos: 1 });
            subscribe(client, { topic: 'opcua/json/connection/#', qos: 1 });
         })
         // https://github.com/mqttjs/MQTT.js#event-error
         client.on('error', (err) => {
            console.error('Connection error: ', err)
            client.end()
         })
         // https://github.com/mqttjs/MQTT.js#event-reconnect
         client.on('reconnect', () => {
            setConnectStatus('Reconnecting');
         })
         // https://github.com/mqttjs/MQTT.js#event-message
         client.on('message', (topic, message) => {
            try {
               if (!message.byteLength) {
                  return;
               }
               const payload: IMessage = JSON.parse(message.toString());
               if (!payload.Timestamp) {
                  payload.Timestamp = Web.utcNow();
               }
               if (payload.MessageType == 'ua-connection') {
                  subscribeToMetadata(client, payload as IConnectionMessage, topics);
               }
               const isData = topics.get(topic);
               if (!isData && payload.MessageType) {
                  setMessages(existing => {
                     if (payload.MessageId) {
                        const map = new Map(existing);
                        map.set(payload.MessageId, payload);
                        return map;
                     }
                     return existing;
                  });
               }
               if (onMessage) {
                  onMessage(topic, payload, isData ?? false);
               }
               if (topic.startsWith("opcua")) {
                  console.log(`received message from topic: ${topic}`)
               }
            }
            catch (error) {
               console.log(`error receiving message from topic: ${topic}`)
            }
         })
      }
   }

   return (
      <Paper elevation={3} sx={{ minWidth: '300px', mr: '5px', height: '100%', width: 'auto' }}>
         <Box sx={{ display: 'flex', p: 3 }}>
            <Button
               variant="contained"
               onClick={() => setConnecting(true)}
               sx={{ mr: 3 }}
            >
               {(connectStatus === 'Connected') ? 'Disconnect' : t('broker.connect')}
            </Button>
            <Alert severity={"info"} sx={{ m: 0, flexGrow: 1 }}>
               <Typography variant="body1" component="p">
                  {connectStatus}
               </Typography>
            </Alert>
         </Box>
         <Accordion sx={{ mx: 3 }}>
            <AccordionSummary
               aria-controls="namespace-content"
               id="namespace-header"
               expandIcon={<ArrowDropDownIcon />}
               sx={{ backgroundColor: theme.palette.primary.light }}
            >
               <Typography sx={{ ml: 3 }}>Unified Namespace</Typography>
            </AccordionSummary>
            <AccordionDetails>
               <BrokerTopicTree />
            </AccordionDetails>
         </Accordion>
         <Accordion>
            <AccordionSummary
               aria-controls="publishers-content"
               id="publishers-header"
               expandIcon={<ArrowDropDownIcon />}
               sx={{ backgroundColor: theme.palette.primary.light }}
            >
               <Typography sx={{ ml: 3 }}>Publishers</Typography>
            </AccordionSummary>
            <AccordionDetails>
               <Table>
                  <TableHead>
                     <TableRow>
                        <TableCell>Publisher</TableCell>
                        <TableCell>Status</TableCell>
                     </TableRow>
                  </TableHead>
                  <TableBody>
                     {Array.from(context.publishers.values()).map((publisher) => {
                        return (
                           <TableRow key={publisher.PublisherId}>
                              <TableCell>{publisher.PublisherId}</TableCell>
                              <TableCell>{
                                 Object.entries(OpcUa.PubSubState).filter(x => publisher.Status === x[1])?.[0][0]}
                              </TableCell>
                           </TableRow>
                        );
                     })}
                  </TableBody>
               </Table>
            </AccordionDetails>
         </Accordion>
         <Accordion>
            <AccordionSummary
               aria-controls="messages-content"
               id="messages-header"
               expandIcon={<ArrowDropDownIcon />}
               sx={{ backgroundColor: theme.palette.primary.light }}
            >
               <Typography sx={{ ml: 3 }}>Messages</Typography>
            </AccordionSummary>
            <AccordionDetails>
               <Table>
                  <TableHead>
                     <TableRow>
                        <TableCell>Time</TableCell>
                        <TableCell>Type</TableCell>
                        <TableCell>Publisher</TableCell>
                     </TableRow>
                  </TableHead>
                  <TableBody>
                     {Array.from(messages.values()).map((message) => {
                        return (
                           <TableRow key={message.MessageId}>
                              <TableCell>{(message.Timestamp) ? Web.formatTime(message.Timestamp) : ''}</TableCell>
                              <TableCell>{message.MessageType}</TableCell>
                              <TableCell>{message.PublisherId}</TableCell>
                           </TableRow>
                        );
                     })}
                  </TableBody>
               </Table>
            </AccordionDetails>
         </Accordion>
         <BrokerConnection defaultOpen={connecting} onConnect={handleConnect} />
      </Paper>
   );
}

export default BrokerView;