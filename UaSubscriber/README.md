## OPC UA IIoT StarterKit – UaSubscriber
### Overview

This application has two modes:

1) A OPC UA Subscriber that discovers publishers and subscribes to Data and MetaData messages. 
2) A OPC UA Requestor that discovers action responders and provides a simple console based UI to send action requests.

Passing the arguement '--responder' will start the application in requestor mode.

### Subscriber Mode
When in subscriber mode the application does the following

1) Subscribes to the Status topic.
2) Subscribes to the Connection and Application topic when a Status message with Status=Operational
3) Subcribes to the Data and MetaData topics when a Connection message arrives (the topics are contained in the Connection message)
4) Prints the contents of Data messages.

Sample output in subscriber mode:
```
================================================================================
MessageId: '41e6e802-b0f0-413d-af83-c933b71d5a5e'
MessageType: 'ua-data'
PublisherId: 'opcf-iiot-kit-dotnet'
================================================================================
DataSetWriterId: '3001'
SequenceNumber: '5'
MessageType: 'ua-deltaframe'
MinorVersion: '1694559840'
Status: 'Good'
Timestamp: '07:16:17.505'
================================================================================
Pipe1002/FT1002/Output: 2111.87 Pa
Drum1001/LI1001/Output: 59.52 cm
Pipe1001/FT1001/Output: 4.03 l/s
FC1001/SetPoint: 21.17
Pipe1001/Valve1001/EnergyConsumption: {
  "Period": 3600,
  "MaxPower": 180,
  "AveragePower": 5,
  "Consumption": 18
}
```

### Requestor Mode
When in requestor mode the application does the following:

1) Subscribes to the Status topic.
2) Subscribes to the Responder topic when a Status message with Status=Operational
3) Subcribes to the Action MetaData topic when a Responder message arrives (the topic is contained in the Connection message)
4) Subscribes to the response topic ()
4) Prompt use to pick available Action Targets (press any key to bring up the menu)
5) Prompt user to enter the input arguments
6) Sends the Action Request
7) Displays the output arguments when the Response message arrives.

**Press any key to bring up the menu**

Sample output in requestor mode:
```
[opcf-iiot-kit-dotnet] DataSetMetaData
   Name: 'BoilerResetAction'
   Description: 'Resets the boiler simulation.'
Please choose a target:
   1) Boilers.Reset.Boiler1
   2) Boilers.Reset.Boiler2
Press 'X' to cancel.
Please enter arguments:
  NewLevelSetPoint [Double]: The drum level set point after reset.
Sending action request.
Data sent to 'opcua-test/Site_Riverdale/ProcessCell_South/BoilerReset'.
[opcf-iiot-kit-dotnet] ActionResponse: RequestId=1 ActionState=Executing.
Output Arguments:
   OldLevelSetPoint [Double]: 100
```

