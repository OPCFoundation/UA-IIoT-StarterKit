import React from 'react';

import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import ChevronRightIcon from '@mui/icons-material/ChevronRight';
import { TreeView } from '@mui/x-tree-view/TreeView/TreeView';
import Paper from '@mui/material/Paper/Paper';

import { ApplicationContext, Publisher } from '../ApplicationProvider';
import { TreeItem } from '@mui/x-tree-view/TreeItem/TreeItem';
import FolderIcon from '@mui/icons-material/Folder';
import LabelIcon from '@mui/icons-material/Label';
import WidgetsIcon from '@mui/icons-material/Widgets';
import Box from '@mui/material/Box/Box';
import Typography from '@mui/material/Typography/Typography';
import TableContainer from '@mui/material/TableContainer/TableContainer';
import Table from '@mui/material/Table/Table';
import TableHead from '@mui/material/TableHead/TableHead';
import TableRow from '@mui/material/TableRow/TableRow';
import TableCell from '@mui/material/TableCell/TableCell';
import TableBody from '@mui/material/TableBody/TableBody';

import { HandleFactory } from '../opcua-utils';
import * as Web from '../Web';
import * as OpcUa from '../opcua';

const renderProperties = (publishers: Map<string, Publisher>, node: BrokerTopicTreeElement) => {
   if (!publishers || !node?.publisherId) return null;
   const publisher = publishers.get(node.publisherId);
   if (!publisher) return null;
   let table = null;
   if (node.fieldName) {
      const writer = publisher.DataSetWriters?.find(x => x.DataSetWriterId === node.dataSetWriterId);
      if (!writer) return null;
      const field = writer.MetaData?.Fields?.find(x => x.Name === node.fieldName);
      if (!field) return null;
      const list = [];
      list.push({ name: 'Description', value: field.Description?.Text });
      if (field.Properties) {
         field.Properties.forEach((ii) => {
            if (ii.Key?.Name && ii.Value) {
               // eslint-disable-next-line @typescript-eslint/no-explicit-any
               let value = (ii.Value as any)?.Body;
               if (value.Body) {
                  value = value.Body;
               }
               list.push({ name: ii.Key.Name, value: JSON.stringify(value) });
            }
         });
      }
      table = (
         <Table>
            <TableHead>
               <TableRow>
                  <TableCell><Typography variant='h6'>Property</Typography></TableCell>
                  <TableCell sx={{ width: '100%' }}><Typography variant='h6'>Value</Typography></TableCell>
               </TableRow>
            </TableHead>
            <TableBody>
               {list.map((ii) => {
                  return (
                     <TableRow key={ii.name}>
                        <TableCell><Typography>{ii.name}</Typography></TableCell>
                        <TableCell sx={{ width: '100%' }}><Typography>{ii.value}</Typography></TableCell>
                     </TableRow>
                  );
               })}
            </TableBody>
         </Table>
      );
   }
   else {
      const map = new Map<string, BrokerTopicTreeElement>();
      collectChildren(node, [], map);
      table = (
         <Table>
            <TableHead>
               <TableRow>
                  <TableCell><Typography variant='h6'>Name</Typography></TableCell>
                  <TableCell><Typography variant='h6'>Timestamp</Typography></TableCell>
                  <TableCell><Typography variant='h6'>DataType</Typography></TableCell>
                  <TableCell sx={{ width: '100%' }}><Typography variant='h6'>Value</Typography></TableCell>
               </TableRow>
            </TableHead>
            <TableBody>
               {Array.from(map.entries()).map((ii) => {
                  const writer = publisher.DataSetWriters?.find(x => x.DataSetWriterId === ii[1].dataSetWriterId);
                  if (!writer) return null;
                  // eslint-disable-next-line @typescript-eslint/no-explicit-any
                  const dv: any = writer?.Values?.get(ii[1].fieldName ?? '');
                  // eslint-disable-next-line @typescript-eslint/no-explicit-any
                  const eu: any = writer.MetaData?.Fields?.
                     find(x => x.Name === ii[1].fieldName)?.Properties?.
                     find(x => x.Key?.Name === 'EngineeringUnits')?.Value;
                  return (
                     <TableRow key={ii[0]}>
                        <TableCell><Typography>{ii[0]}</Typography></TableCell>
                        <TableCell><Typography>{Web.formatTime(dv?.Timestamp)}</Typography></TableCell>
                        <TableCell><Typography>{dv?.DataType}</Typography></TableCell>
                        <TableCell sx={{ width: '100%' }}><Typography>{((dv?.Status & 0x80000000) !== 0) ? OpcUa.StatusCodes[dv?.Status] : JSON.stringify(dv?.Value)} {eu?.Body?.Body?.DisplayName?.Text}</Typography></TableCell>
                     </TableRow>
                  );
               })}
            </TableBody>
         </Table>
      );
   }

   return (
      <TableContainer component={Paper} elevation={3} sx={{ height: '100%', width: '100%' }}>
         {table}
      </TableContainer>
   );
};

export type BrokerTopicTreeElement = {
   id: number,
   name: string,
   path?: string,
   publisherId?: string,
   dataSetWriterId?: number,
   fieldName?: string,
   expanded: boolean,
   children: BrokerTopicTreeElement[]
   noLongerUsed?: boolean
}

const renderChildren = (node: BrokerTopicTreeElement) => {
   const icon = (!node.dataSetWriterId) ? FolderIcon : (!node.fieldName) ? WidgetsIcon : LabelIcon;
   const iconColor = (!node.dataSetWriterId) ? 'gold' : (!node.fieldName) ? 'blue' : 'green';
   return (
      <TreeItem
         key={node.name}
         nodeId={`${node.id}`}
         label={
            <Box
               sx={{
                  display: 'flex',
                  alignItems: 'center',
                  p: 0.5,
                  pr: 0,
               }}
            >
               <Box component={icon} color={iconColor} sx={{ mr: 2 }} />
               <Typography variant="body2" sx={{ flexGrow: 1 }}>
                  {node.name}
               </Typography>
            </Box>
         }
      >
         {node.children?.map((child) => renderChildren(child))}
      </TreeItem>
   );
}

const collectChildren = (node: BrokerTopicTreeElement, path: string[], collection: Map<string, BrokerTopicTreeElement>) => {
   const children = node.children;
   if (children?.length) {
      for (let ii = 0; ii < children.length; ii++) {
         path.push(children[ii].name);
         if (children[ii].fieldName) {
            collection.set(path.join('.'), children[ii]);
         }
         else {
            collectChildren(children[ii], path, collection);
         }
         path.pop();
      }
   }
}

const findChild = (id: number, nodes?: BrokerTopicTreeElement[]): BrokerTopicTreeElement | undefined => {
   if (nodes) {
      for (let ii = 0; ii < nodes.length; ii++) {
         if (nodes[ii].id === id) {
            return nodes[ii];
         }
         const child = findChild(id, nodes[ii].children);
         if (child) {
            return child;
         }
      }
   }
   return undefined;
}

function pruneTree(nodes: BrokerTopicTreeElement[]): BrokerTopicTreeElement[] {
   if (!nodes.length) {
      return nodes;
   }
   const filteredNodes = nodes.filter((node) => !node.noLongerUsed);
   filteredNodes.forEach((node) => {
      node.children = pruneTree(node.children);
   });
   return filteredNodes;
}

export const BrokerTopicTree = () => {
   const [nodes, setNodes] = React.useState<BrokerTopicTreeElement[]>([]);
   const [expanded, setExpanded] = React.useState<string[]>([]);
   const [selection, setSelection] = React.useState<BrokerTopicTreeElement | undefined>();
   const context = React.useContext(ApplicationContext);

   const handleNodeSelect = React.useCallback((_e: React.SyntheticEvent, nodeId: string) => {
      const id = Number(nodeId);
      const hit = findChild(id, nodes);
      setSelection(hit);
   }, [nodes]);

   const handleNodeToggle = React.useCallback((e: React.SyntheticEvent, nodeIds: string[]) => {
      e.preventDefault();
      setExpanded(nodeIds);
   }, []);

   React.useEffect(() => {
      if (context?.publishers) {
         setNodes(existing => {
            let rootNodes: BrokerTopicTreeElement[] = existing.map(x => {
               x.noLongerUsed = true;
               return x;
            });
            Array.from(context?.publishers.values()).forEach((publisher) => {
               if (publisher.Status !== 2 || !publisher.DataSetWriters?.length) {
                  if (selection?.publisherId === publisher.PublisherId) {
                     setSelection(undefined);
                  }
                  rootNodes = rootNodes.filter(x => x.publisherId !== publisher.PublisherId);
                  return;
               }
               publisher.DataSetWriters?.forEach((writer) => {
                  const levels = writer.DataTopic?.split('/').filter(x => x);
                  if (levels?.length) {
                     let parent: BrokerTopicTreeElement | undefined = undefined;
                     let children = rootNodes;
                     for (let ii = 0; ii < levels.length; ii++) {
                        const index = children.findIndex((x: BrokerTopicTreeElement) => x.name === levels[ii]);
                        const node = (index < 0) ?
                           {
                              id: HandleFactory.increment(),
                              name: levels[ii],
                              publisherId: publisher.PublisherId,
                              expanded: false,
                              children: []
                           } : { ...children[index] };
                        node.noLongerUsed = false;
                        (index < 0) ? children.push(node) : children[index] = node;
                        if (parent) {
                           parent.children = children;
                        }
                        parent = node;
                        children = [...node.children];
                     }
                     if (parent) {
                        if (writer.MetaData?.Fields?.length) {
                           const fields = writer.MetaData?.Fields;
                           const subroot = children;
                           for (let ii = 0; ii < fields.length; ii++) {
                              const sublevels = fields[ii].Name?.split('/').filter(x => x);
                              children = subroot;
                              let subparent: BrokerTopicTreeElement | undefined = undefined;
                              if (sublevels?.length) {
                                 for (let jj = 0; jj < sublevels.length; jj++) {
                                    const index = children.findIndex((x: BrokerTopicTreeElement) => x.name === sublevels[jj]);
                                    const node = (index < 0) ?
                                       {
                                          id: HandleFactory.increment(),
                                          name: sublevels[jj],
                                          publisherId: publisher.PublisherId,
                                          dataSetWriterId: writer.DataSetWriterId,
                                          fieldName: (jj == sublevels.length - 1) ? fields[ii].Name : undefined,
                                          expanded: false,
                                          children: []
                                       } : { ...children[index] };
                                    node.noLongerUsed = false;
                                    (index < 0) ? children.push(node) : children[index] = node;
                                    if (subparent) {
                                       subparent.children = children;
                                    }
                                    subparent = node;
                                    children = [...node.children];
                                 }
                              }
                           }
                           if (parent) {
                              parent.children = subroot;
                           }
                        }
                     }
                  }
               })
            });
            return pruneTree(rootNodes);
         });
      }
   }, [context?.publishers, selection?.publisherId]);

   return (
      <Box display="flex" p={2} pb={4} sx={{ width: '100%', minHeight: '300px' }}>
         <Box flexGrow={0}>
            <Paper elevation={3} sx={{ minWidth: '300px', mr: '5px', height: '100%', width: 'auto' }}>
               <TreeView
                  defaultCollapseIcon={<ExpandMoreIcon />}
                  defaultExpandIcon={<ChevronRightIcon />}
                  onNodeSelect={(e: React.SyntheticEvent, nodeId: string) => handleNodeSelect(e, nodeId)}
                  onNodeToggle={(e: React.SyntheticEvent, nodeIds: string[]) => handleNodeToggle(e, nodeIds)}
                  expanded={expanded}
               >
                  {nodes?.map((node) => renderChildren(node))}
               </TreeView>
            </Paper>
         </Box>
         <Box flexGrow={1}>
            {(selection) ? renderProperties(context.publishers, selection) : null}
         </Box >
      </Box >
   );
}

export default BrokerTopicTree;