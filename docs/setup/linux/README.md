## OPC UA IoT StarterKit – Setup Ubuntu Environment
### Overview

1. [Install and Configure MQTT Broker](#1)
2. [Install and Configure .NET Core](#2)
3. [Build StarterKit Code](#3)

### <a name='1'>Install and Configure MQTT Broker</a>
These steps explain how to set up an insecure broker for testing. A broker used in production applications would need to have TLS enabled and some sort of client authentication. 

Install Mosquitto Broker with these commands:
```
sudo apt update
sudo apt install mosquitto
```

Download Java 8 from [here](https://www.oracle.com/java/technologies/javase-jre8-downloads.html).
For Ubuntu 64-bit the download is the "Linux x64 Compressed Archive".
```
cd ~/
tar -xvf jre-8u291-linux-x64.tar.gz 
```

Download mqtt-spy from [here](https://github.com/eclipse/paho.mqtt-spy/releases). 
```
mkdir ~/mqtt-spy
cd ~/mqtt-spy
~/jre1.8.0_291/bin/java -jar mqtt-spy-1.0.0.jar 
```

To connect to a broker using mqtt-spy:
* Create a connection to the broker; 
* Subscribe to all topic (enter ‘#’ as the topic name); 
* Publish to a text topic and verify the response was received. 

### <a name='2'>Install and Configure .NET Core</a>
Install Visual Studio Code:
```
sudo apt install code
```
Manually download Visual Studio Code from [https://code.visualstudio.com/](https://code.visualstudio.com/) if necessary.  

Install the following extensions (select extensions icon on right side toolbar): 
* C#

Install .NET Core SDK:
```
sudo snap install dotnet-sdk --classic
```

Test installation:
```
mkdir helloworld
cd helloworld/
dotnet new console
dotnet build
dotnet bin/Debug/net5.0/helloworld.dll 
```

If all is good the following output should be printed:
```
Hello World!
```

### <a name='3'>Build StarterKit Code</a>

Fetch code from GitHub:
```
cd ~/
git clone https://github.com/OPCF-Members/UA-IoT-StarterKit.git
cd UA-IoT-StarterKit
git submodule update --init
```

Build code:
```
cd ~/UA-IoT-StarterKit
dotnet build MqttAgent/MqttAgent.csproj 
```

Run code
```
cd ~/UA-IoT-StarterKit/build/bin/Debug/net50/
dotnet MqttAgent.dll --help
```

The following output should be produced:
```
Usage: MqttAgent [options] [command]

Options:
  -?|-h|--help  Show help information

Commands:
  discover   Discovers OPC UA publishers.
  publish    Publishes I/O data to an MQTT broker.
  subscribe  Subscribes for data from OPC UA publishers.

Use "MqttAgent [command] --help" for more information about a command.
```



