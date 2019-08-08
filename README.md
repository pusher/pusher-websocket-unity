## Pusher Channels Unity Client Library

The purpose of this repo is to be:
-  A Base Unity project which demonstrates how to integrate Pusher Channels with Unity
-  A build environment for the `PusherWebsocketUnity` _unitypackage_ and its Asset Store release

### Usage
Download the latest `.unitypackage` compatible with you Unity version from the [releases section](/../../releases) or Download it from the Asset Store: [Pusher Channels Realtime Client](https://assetstore.unity.com/).


### Unity Versions Support
TODO: table

- Unity 2018.1
  - Unity 2018.1.0
  - Unity 2018.1.2
  - Unity 2018.1.3

- All Unity 2018.2.x >= 2018.2.6

- All Unity 2018.3.x

- All Unity 2018.4.x

- All Unity 2019.1.x

- All Unity 2019.2.x

### Unity Platforms Support

#### Supported Platforms:
- Windows
- MacOS
- Android
- iOS

#### Unsupported Platforms:
- WebGL due to incompatibility with Websockets (more [here](https://docs.unity3d.com/Manual/webgl-networking.html) under the _"No direct socket access"_ section)


### Build


### Create a new Release


### Publish to the Asset Store


### Known Issues

This library is INCOMPATIBLE with 2018.1.4 <= Unity <= 2018.2.5 due to a [bug](https://issuetracker.unity3d.com/issues/opened-event-of-a-websocket4net-dot-websocket-does-not-get-called-when-opening-a-web-socket) in the Unity engine that prevents WebSocket4Net (one of the dependencies) to work correctly

