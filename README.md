## !EXPERIMENTAL! Pusher Channels Unity Client Library

**WARNING this is an experimental release and might not work or change in the future**

### Usage
1) Download the latest `.unitypackage` from this repo's [Releases](/../../releases)
2) Open a new/existing Unity project and make sure it is being opened by a [supported version of Unity](#unity-versions-support)
3) [if your Unity version is **2018.x.x**] Make sure that under `Edit -> Project Settings -> Player` the `Configuration -> Scripting Runtime Version` is set to **.NET 4.x Equivalent**
4) Click on `Assets -> Import Package -> Custom Package...`, find and select the `PusherWebsocketUnity-x.x.x-xxxxxxx.unitypackage` and click *Import* on the `Import Unity Package` window.
5) Create a Pusher Channels app at https://pusher.com/channels.
6) Open the `PusherManager.cs` file in the Assets folder and add your keys as values for `private const string APP_KEY` and `private const string APP_CLUSTER` obtained at the previous step.
7) Create a new GameObject, by going on `GameObject -> Create Empty`. Drag the `PusherManager.cs` script onto the GameObject Inspector to set it as a script for the object.
8) Save and click Play to start the game in Unity.
9) Verify that in the Console tab the following is logged: `Connection state changed`, `Connected`, `Subscribed`.
10) You can now customise the channel name (by default is `"my-channel"`) and events to bind to (by default is `"my-event"`) in the `PusherManager.cs` script.

### Repository Structure
-  `BaseProject` a Unity Base project which demonstrates how to integrate Pusher Channels with Unity
-  `Builds` a build environment for the `PusherWebsocketUnity` _unitypackage_ and its Asset Store release
-  `Examples` example projects that combines Unity and Pusher Channels

### Unity Versions Support
- Unity 2018.1
  - Unity 2018.1.0
  - Unity 2018.1.2
  - Unity 2018.1.3

- All Unity 2018.2.x >= 2018.2.5f1

- All Unity 2018.3.x

- All Unity 2018.4.x

- All Unity 2019.1.x

- All Unity 2019.2.x

### Unity Platforms Support

#### Supported Platforms:
The supported platforms should be the ones that can be targeted by Unity.
Thus far we tested this, and can confirm that works, on:
- Windows
- MacOS
- Android
- iOS

#### Unsupported Platforms:
- WebGL: due to incompatibility with Websockets (more [here](https://docs.unity3d.com/Manual/webgl-networking.html) under the _"No direct socket access"_ section)

<!--
### Update the Package
TODO

### Build
TODO
-->

### Known Issues
This library is INCOMPATIBLE with 2018.1.4 <= Unity <= 2018.2.4 due to
a [bug](https://issuetracker.unity3d.com/issues/opened-event-of-a-websocket4net-dot-websocket-does-not-get-called-when-opening-a-web-socket)
in the Unity engine that prevents WebSocket4Net (one of the dependencies) to work correctly

### Credits
- Unity Technologies https://unity3d.com/company - for the Unity3d environment
- Patrick McCarthy (GlitchEnzo) https://github.com/GlitchEnzo - for [NuGetForUnity](https://github.com/GlitchEnzo/NuGetForUnity)
