## OPC UA IoT StarterKit – Setup Development Environment for Windows and Raspberry Pi
### Overview

1. [Install and Configure MQTT Broker](#1)
2. [Setup Development Machine](#2)
3. [Setup Raspberry Pi](#3)
4. [Install .NET Core on the Raspberry Pi](#4)
5. [Setup remote access to the Raspberry Pi](#5)
6. [Setup remote build environment](#6)

### <a name='1'>Install and configure MQTT Broker</a>
These steps explain how to set up an insecure broker for testing. A broker used in production applications would need to have TLS enabled and some sort of client authentication. 

Download the Mosquitto Broker from here: [https://mosquitto.org/download/](https://mosquitto.org/download/). 

Once installed it has to be configured to allow connections from the network. 

Edit ‘mosquitto.conf’ (in C:\Program Files\mosquitto on Windows). 

Add a listener for the IP address/hostname where the broker is running:
```
listener 1883 <Insert IP address accessible to Raspberry Pi>
allow_anonymous true
```
The section for listeners is around line 193 in the default configuration file. 

Note that an IP address is likely required for access from the Raspberry Pi unless the broker host is registered with a DNS server. 

On Windows machines the firewall needs to be opened. 

From the ‘Advanced Settings’ for the Windows Firewall, add the following rules: 
* Allow Program ‘%ProgramFiles%\mosquitto\mosquitto.exe’
* Allow Port 1883 

Test the broker by using mqtt-spy which is in the mqtt-spy [directory](https://github.com/OPCF-Members/UA-IoT-StarterKit/tree/master/mqtt-spy). 

mqtt-spy can also be be downloaded from [here](https://github.com/eclipse/paho.mqtt-spy/releases). 

It requires Java SE 8 which can be downloaded from: [https://www.oracle.com/java/technologies/javase-downloads.html#JDK8](https://www.oracle.com/java/technologies/javase-downloads.html#JDK8) 

The command to run mqtt-spy is:
```
java -jar  mqtt-spy-1.0.0.jar
```

To connect to a broker using mqtt-spy:
* Create a connection to the broker; 
* Subscribe to all topic (enter ‘#’ as the topic name); 
* Publish to a text topic and verify the response was received. 

### <a name='2'>Setup Development Machine</a>
These instructions assume the development machine is running Windows 10 (1709 or later - must support ssh). 

The process is similar for set up on Linux or Mac. 

The development enviroment requires Visual Studio 2019.

Download Visual Studio 2019 from [https://visualstudio.microsoft.com/vs/](https://visualstudio.microsoft.com/vs/). 

When installating ensure the .NET Core cross platform development feature is selected.

### <a name='3'>Setup Raspberry Pi</a>
Instructions on setting up a Raspberry Pi vary depending on the choice of hardware and development platform. Many good tutorials can be found on the Internet. 

Example of instructions on how to set up a new Raspberry Pi for headless operation can be found [here](https://www.tomshardware.com/reviews/raspberry-pi-headless-setup-how-to,6028.html). 

Before proceeding to the next step please ensure you have installed the latest version of Raspberry Pi OS (32-bit) on the device and have network connectivity between the development machine and the Raspberry Pi. 

### <a name='4'>Install .NET on the Raspberry Pi</a> 
The samples require .NET 5.0 which can be downloaded from: [https://dotnet.microsoft.com/download/dotnet/](https://dotnet.microsoft.com/download/dotnet/). 

Download the latest “Arm32” binaries for the .NET Core 5.0 SDK. 
Note that if you are not using a Raspberry Pi you must download the image that matches your platform.

The default location for downloads when using the Raspberry Pi web browser is ~/Downloads. 

Execute the following commands from a shell: 
```
mkdir dotnet
sudo tar zxf ./Downloads/dotnet-sdk-5.0.300-linux-arm.tar.gz -C ./dotnet/
export DOTNET_ROOT=~/dotnet
export PATH=$PATH:~/dotnet
dotnet –version
```
If everything installed properly the last line prints out the installed version (‘5.0.300' in this case). 

The exact TAR file name should match the current version of the .NET 5.0 SDK. 

The following lines need to added to ~/.bashrc 
```
export DOTNET_ROOT=~/dotnet
export PATH=$PATH:~/dotnet
```

Create a simple application. 
```
mkdir helloworld
cd helloworld/
dotnet new console
dotnet build
dotnet bin/Debug/net5.0/helloworld.dll 
```
If everything installed properly the last line prints out ‘Hello World!’. 

### <a name='5'>Setup remote access to the Raspberry Pi</a>
These steps are designed for users that have never used SSH keys before. 
If an SSH key already exists on the development system then that can be used. It is only necessary to register the key with the Raspberry Pi by adding it to the authorized_keys file on the Raspberry Pi. 

Create a SSH key on the Raspberry Pi that can be used for remote login this command: 
```
cd ~/
mkdir .ssh
ssh-keygen
```
This will show this output:
```
Generating public/private rsa key pair.
Enter file in which to save the key (/home/pi/.ssh/id_rsa): 
```
Do not provide a password and remember the path to the key (e.g. ~/.ssh/id_rsa). 

Using the file name you actually choose, do the following:
```
cat ~/.ssh/id_rsa.pub > ~/.ssh/authorized_keys
sudo chmod 644 ~/.ssh/authorized_keys
sudo chown pi:pi ~/.ssh/authorized_keys
```

On the development machine copy the ssh key from the Raspberry Pi to the workspace: 
```
mkdir .ssh
scp pi@raspberrypi:~/.ssh/id_pi ./.ssh/
```

Enter the Raspberry Pi account password when prompted. 

It is now necessary to remove permissions from the local .ssh folder: 
* Right click on the .ssh folder and select ‘Properties’.
* Go to the ‘Security’ tab.
* Click the ‘Advanced’ button.
* Next to ‘Owner’ click ‘Change’.
* Type in the current account name and click ‘Check Names’.
* Check ‘Replace owner on subcontainers and objects’.
* Click ‘Apply’ and accept any prompts.
* Click the ‘Disable inheritance’ button and choose ‘Remove all’.
* Click ‘Add’ and then ‘Select a principal’.
* Type in the current account name and click ‘Check Names’.
* Click OK on all dialogs.

Go to a command prompt and run: 
```
ssh -i ./.ssh/id_rsa pi@raspberrypi
```

If the remote login succeeds without being prompted to enter a password, then everything has been set up correctly. 

### <a name='6'>Setup remote build environment</a>
The workspace directory is the root of where the code is stored on the development machine. 

Create and run a simple program on the development machine. 
```
mkdir remoteworld
cd remoteworld/
dotnet new console
dotnet run
```

Verify that "Hello World!" was printed. 

Compile the program for the Raspberry Pi. 
```
dotnet publish -f net50 -r linux-arm -o ../remotebuild/remoteworld ./remoteworld.csproj
```
The binaries are written to the remotebuild/remoteworld directory. 

Copy the binaries to the Raspberry Pi 
```
scp -i ./.ssh/id_rsa -r ../remotebuild/remoteworld pi@raspberrypi:~/
```
Go to the Raspberry Pi (remote login or via a remote desktop). 
```
cd ~/remoteworld
dotnet remoteworld.dll
```
The program should print “Hello World!”. 

