import * as React from 'react';
import * as OpcUa from './opcua';
import { IUserContext } from './UserProvider';
import { UserContext } from './UserProvider';
import * as Web from './Web';

export interface IMessage {
   MessageId?: string,
   MessageType?: string;
   PublisherId?: string;
   Timestamp?: string;
}

export interface IStatusMessage {
   MessageId?: string,
   MessageType?: string;
   PublisherId?: string;
   Timestamp?: Date;
   Status?: number;
}

export interface IConnectionMessage {
   MessageId?: string,
   MessageType?: string;
   PublisherId?: string;
   Timestamp?: Date;
   Connection?: OpcUa.PubSubConnectionDataType;
}

export interface IDataSetMetaDataMessage {
   MessageId?: string,
   MessageType?: string;
   PublisherId?: string;
   Timestamp?: Date;
   DataSetWriterId?: number,
   DataSetWriterName?: string,
   MetaData?: OpcUa.DataSetMetaDataType;
}

export interface INetworkMessage {
   MessageId?: string,
   MessageType?: string;
   PublisherId?: string;
   WriterGroupName?: string,
   DataSetClassId?: string,
   Messages?: unknown;
}

export interface IDataSetMessage {
   MessageType?: string;
   PublisherId?: string;
   WriterGroupName?: string,
   DataSetWriterId?: number,
   DataSetWriterName?: string,
   SequenceNumber?: number,
   MetaDataVersion?: { MajorVersion?: number, MinorVersion?: number };
   MinorVersion?: number;
   Timestamp?: Date;
   Status?: number;
   Payload?: { [key: string]: object }
}

export type DataValue = {
   Value: object;
   Timestamp?: string;
   Status?: number;
   DataType?: string;
}

export type DataSetWriter = {
   DataTopic: string;
   IsSingleDataSetMessage: boolean;
   DataSetWriterId: number;
   MetaData?: OpcUa.DataSetMetaDataType;
   Values?: Map<string, DataValue>;
}

export type Publisher = {
   PublisherId: string;
   Status?: number;
   Connection?: OpcUa.PubSubConnectionDataType;
   DataSetWriters?: DataSetWriter[];
}
export interface IApplicationContext {
   userContext: IUserContext
   publishers: Map<string, Publisher>
   publisherStateChanges: number
   messageReceived: (topic: string, message: IMessage, isData: boolean) => void
}

export const ApplicationContext = React.createContext<IApplicationContext>({
   userContext: {} as IUserContext,
   publishers: new Map(),
   publisherStateChanges: 0,
   messageReceived: () => { }
});

const findDataTypeName = (id?: OpcUa.NodeId, valueRank?: number): string => {
   let text = '';
   if (id?.Id) {
      for (const name in OpcUa.DataTypeIds) {
         if ((OpcUa.DataTypeIds as { [index: string]: string })[name] === `i=${id?.Id}`) {
            text = name;
            break;
         }
      }
   }
   if (!text) {
      text = `i=${id?.Id}`;
   }
   if (valueRank && valueRank > 0) {
      text += '[]';
      for (let ii = 1; ii < valueRank; ii++) {
         text += '[]';
      }
   }
   return text;
}

export interface ApplicationProviderProps {
   children?: React.ReactNode
}

function getDataTopic(group: OpcUa.WriterGroupDataType, writer: OpcUa.DataSetWriterDataType): string | undefined {
   const settings = writer?.TransportSettings?.Body as OpcUa.BrokerDataSetWriterTransportDataType;
   if (settings.QueueName) {
      return settings.QueueName;
   }
   const groupSettings = group?.TransportSettings?.Body as OpcUa.BrokerWriterGroupTransportDataType;
   if (groupSettings.QueueName) {
      return groupSettings.QueueName;
   }
   return undefined;
}

export const ApplicationProvider = ({ children }: ApplicationProviderProps) => {
   const [publishers, setPublishers] = React.useState<Map<string, Publisher>>(new Map<string, Publisher>);
   const [publisherStateChanges, setPublisherStateChanges] = React.useState<number>(0);
   const userContext = React.useContext(UserContext);
   const name = userContext?.user?.name;

   React.useEffect(() => {
      console.log(`ApplicationProvider: ${name}`);
   }, [name]);

   const context = {
      userContext: userContext,
      publishers,
      publisherStateChanges,
      messageReceived: (topic: string, message: IMessage, isData: boolean) => {
         if (message?.PublisherId) {
            setPublishers(existing => {
               if (message?.PublisherId) {
                  const updated = new Map(existing);
                  let publisher = existing.get(message?.PublisherId);
                  if (!publisher) {
                     publisher = {
                        PublisherId: message?.PublisherId,
                        Status: 0,
                        DataSetWriters: []
                     };
                  }
                  if (isData) {
                     const data = message as IDataSetMessage;
                     if (data) {
                        const writer = publisher.DataSetWriters?.find(x => x.DataSetWriterId === data.DataSetWriterId);
                        if (writer) {
                           if (data.Payload) {
                              Object.keys(data.Payload).forEach((key: string) => {
                                 if (data.Payload && data.Payload[key]) {
                                    const metadata = writer.MetaData?.Fields?.find(x => x.Name === key);
                                    const dv = {
                                       DataType: findDataTypeName(metadata?.DataType, metadata?.ValueRank)
                                    } as DataValue;
                                    // eslint-disable-next-line @typescript-eslint/no-explicit-any
                                    const raw = data.Payload[key] as any;
                                    if (raw.SourceTimestamp) {
                                       dv.Timestamp = raw.SourceTimestamp;
                                    }
                                    else if (raw.ServerTimestamp) {
                                       dv.Timestamp = raw.ServerTimestamp;
                                    }
                                    else if (message.Timestamp) {
                                       dv.Timestamp = message.Timestamp;
                                    }
                                    else {
                                       dv.Timestamp = Web.utcNow();
                                    }
                                    if (raw.Status) {
                                       dv.Status = raw.Status;
                                    }
                                    if (raw.Value?.Body !== undefined) {
                                       dv.Value = raw.Value.Body;
                                    }
                                    else if (raw.Type) {
                                       if (raw.Type === 19 /*OpcUa.DataTypeIds.StatusCode*/) {
                                          dv.Status = raw.Body;
                                       } else {
                                          dv.Value = raw.Body;
                                       }
                                    }
                                    else if (raw.Body !== undefined) {
                                       dv.Value = raw.Body;
                                    }
                                    else {
                                       dv.Value = raw;
                                    }
                                    writer.Values?.set(key, dv);
                                 }
                              });
                           }
                        }
                     }
                  }
                  else {
                     switch (message.MessageType) {
                        case 'ua-status': {
                           const status = (message as IStatusMessage).Status;
                           if (publisher.Status !== status) {
                              publisher.Status = status;
                              publisher.DataSetWriters = [];
                              setPublisherStateChanges((x) => x + 1);
                           }
                           break;
                        }
                        case 'ua-connection': {
                           publisher.Connection = (message as IConnectionMessage).Connection;
                           const writers: DataSetWriter[] | undefined = [];
                           publisher.Connection?.WriterGroups?.forEach((group) => {
                              group.DataSetWriters?.forEach((definition) => {
                                 const topic = getDataTopic(group, definition);
                                 const writer = publisher?.DataSetWriters?.find(x =>
                                    x.DataSetWriterId === definition.DataSetWriterId &&
                                    x.DataTopic === topic);
                                 if (writer) {
                                    writers.push(writer);
                                 }
                              });
                           });
                           publisher.DataSetWriters = writers;
                           break;
                        }
                        case 'ua-metadata': {
                           const metadata = message as IDataSetMetaDataMessage;
                           let writer = publisher.DataSetWriters?.find(x => x.DataSetWriterId === metadata.DataSetWriterId);
                           if (!writer) {
                              writer = {
                                 DataTopic: topic,
                                 DataSetWriterId: metadata.DataSetWriterId ?? 0,
                                 IsSingleDataSetMessage: false,
                                 Values: new Map<string, DataValue>()
                              };
                              publisher.DataSetWriters?.push(writer);
                           }
                           writer.MetaData = metadata.MetaData;
                           publisher.Connection?.WriterGroups?.forEach((group) => {
                              const definition = group.DataSetWriters?.find((x) => writer?.DataSetWriterId === x.DataSetWriterId);
                              if (definition && writer) {
                                 const settings = definition?.TransportSettings?.Body as OpcUa.BrokerDataSetWriterTransportDataType;
                                 if (settings.QueueName) {
                                    writer.DataTopic = settings.QueueName;
                                    writer.IsSingleDataSetMessage = true;
                                 }
                                 else if (settings.MetaDataQueueName) {
                                    const groupSettings = group?.TransportSettings?.Body as OpcUa.BrokerWriterGroupTransportDataType;
                                    if (groupSettings.QueueName) {
                                       writer.DataTopic = groupSettings.QueueName;
                                    }
                                 }
                                 return;
                              }
                           });
                           break;
                        }
                     }
                  }
                  updated.set(message?.PublisherId, publisher);
                  return updated;
               }
               return existing;
            });
         }
      }
   } as IApplicationContext;

   return (
      <ApplicationContext.Provider value={context}>
         {children}
      </ApplicationContext.Provider>
   );
};

export default ApplicationProvider;