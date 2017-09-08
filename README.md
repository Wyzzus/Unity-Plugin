# Navisens/Unity-Plugin #
-----

This repository integrates our SDK with [Unity3D](https://unity3d.com).

## Download ##

A Unity package is included in the repository. Download [this](MotionDna.unitypackage) for quick installation.
The sources for the development version are also included in the `/Assets` directory as a raw download.

## How to use ##

1. Follow the setup procedures below for different targets for importing the plugin
	* [iOS](#ios)
	* [Android](#android)
2. After you import, you will have access to the following
    * `/MotionDna` folder containing a Prefab object and the Unity C# scripts
    * `/Plugins` folder containing native libraries and wrapper classes for Unity3D integration
3. Open a scene, and drag the MotionDna.prefab into the scene.
* This GameObject provides a callback for our SDK to notify Unity3D that new estimation results are ready to be published, via `MotionDnaCallback.ReceivedMotionDna(string)` function. Please make sure only **one** of these exists so you don't make unnecessary queries. If you no longer require live updates, you can remove this object from the scene to prevent automatic updates from our SDK.
4. Create a new script and in the `Start()` function, set up our MotionDna SDK
    * Here is an example of a basic set up. See the [API](#api) below for more details.
```csharp
    private const string DEVELOPER_KEY = "/* YOUR DEVELOPER KEY */";
    void Start ()
    {
        MotionDna.Init (DEVELOPER_KEY).SetCallbackUpdateRateInMs (50)
            .SetExternalPositioningState (MotionDna.ExternalPositioningState.OFF);
    }
```
5. Once you have `Init` our MotionDna SDK, you can begin accessing our estimation results.
    * Simple example of updating the position and heading of the player.
```csharp
    // Update is called once per frame
    void Update ()
    {
        GetComponent<Transform> ().position = MotionDna.Position;
        
        double heading = MotionDna.GetLocalHeading() ?? 0;
        Vector3 eAngles = transform.eulerAngles;
        eAngles.y = (float) -heading;
        GetComponent<Transform> ().eulerAngles = eAngles;
    }
```
6. Compile and run! Some additional setup will be necessary. See below.

## iOS ##
To set up the plugin for iOS development, Unity3D and Xcode both need to be configured.

Video tutorial for setup - note that there is a typo in the video: "rotation" should be "localRotation".
<a href="https://youtu.be/J8430X2g7fE" target="_blank"><img src="http://i.imgur.com/ws5Gmiy.png" title="https://youtu.be/J8430X2g7fE"></a>

1. Download the plugin and import it to your Unity project.

2. Set the _Platform_ to `iOS` in _Build Settings_.
3. Open up _Player Settings_, and set _Other Settings_ -> _Configuration_ -> _Target minimum iOS Version_ to `8.0`.
4. Build your application to **Xcode**.
5. Go in **Xcode**, and configure settings.
6. In _General_ -> _Embedded Binaries_, press the `+` and select the `Plugins/iOS/MotionDnaSDK.framework`
7. Set the _Build Settings_ -> _Build Options_ -> _Enable Bitcode_ setting to `No`.
8. Update your `Info.plist` file with privacy information on location usage `NSLocationWhenInUseUsageDescription` (iOS won't let you build if you don't have all of the info messages).
9. Build and test your app!

## Android ##
To set up the plugin for Android development, Unity3D will build using gradle, and additional configuration will be done after the build.

Video tutorial for setup.
[![https://youtu.be/9CgXvxqJcjY](http://i.imgur.com/Wozs3BZ.png)](https://youtu.be/9CgXvxqJcjY)

1. Download the plugin and import it to your Unity project.

2. Set the _Platform_ to `Android` in _Build Settings_.
3. Set the _Build System_ to `Gradle` in _Build Settings_ and enable the `Export` option.
4. Build your application.
5. Open up the `build.gradle` and make the following changes
    1. Remove the Unity3D comment, so Unity will not override the changes we make to `build.gradle`
    ```gradle
    // GENERATED BY UNITY. REMOVE THIS COMMENT TO PREVENT OVERWRITING WHEN EXPORTING AGAIN
    ```
	2. Update gradle to a newer version
	```gradle
    buildscript {
        // ...

        dependencies {
            classpath 'com.android.tools.build:gradle:2.3.3'
        }
    }
	```
	3. Add the following maven repositories
    ```gradle
    allprojects {
        repositories {
           // ...

           maven {
               url 'https://oss.sonatype.org/content/groups/public'
           }
           maven {
               url 'https://maven.fabric.io/public'
           }
       }
    }
    ```
    4. Add the following dependencies
    ```gradle
    dependencies {
        // ...
        compile 'com.android.support:appcompat-v7:24.2.1'
        compile 'org.altbeacon:android-beacon-library:2.+'
        compile 'com.google.code.gson:gson:2.8.1'
    }
    ```
    * Note: The `appcompat` package is necessary, because buildtools will update to 25.0.3 (or future)
6. Build and test your app!

# Changelog #
-----
#### 2.0.0
Added networking support! Improved algorithms.

Known bug when using networking on Android devices occasionally causes crash. Patch in near future.

#### 1.3.0
Updated internals for better recognization of Augmented Reality type motions

Reorganized repository

#### 1.2.0
Fixed bug where trivial error messages would display while playing app on non-mobile devices

#### 1.1.0
Added Android support

Added Versions folder to log previous working unitypackages

Fixed bug in iOS where can't re-init after stopping in same session

#### 1.0.0
Initial release with iOS support

# API #
-----
All functions are linked in `MotionDna.cs` for convenience.
Functions are grouped below by usage.
 * [Setup](#setup)
 * [Actions](#actions)
 * [Getters](#getters)

All callbacks are linked in `MotionDnaCallback.cs`.
 * [Callbacks](#callbacks)

-----
## Setup ##
_It is highly advised that all setup is done in a `MonoBehaviour.Start()` function or any "paused" state, as these calls can be expensive to execute._

* [`MotionDna.Init(string key)`](#motiondnainitstring-key)
* [`.SetLocation(double lat, double long, double head)`](#setlocationdouble-lat-double-long-double-head)
* [`.SetLocation(double lat, double long)`](#setlocationdouble-lat-double-long)
* [`.SetHeading(double heading)`](#setheadingdouble-heading)
* [`.ResolveLocation()`](#resolvelocation)
* [`.ResolveHeading()`](#resolveheading)
* [`.ResolveLocationAndHeading()`](#resolvelocationandheading)
* [`.ComputeLocation()`](#computelocation)
* [`.SetCallbackUpdateRateInMs(double ms)`](#setcallbackupdaterateinmsdouble-ms)
* [`.SetNetworkUpdateRateInMs(double ms)`](#setnetworkupdaterateinmsdouble-ms)
* [`.SetLocalHeadingOffsetInDegrees(double offset)`](#setlocalheadingoffsetindegreesdouble-offset)
* [`.SetCartesianOffsetInMetersXY(double x, double y)`](#setcartesianoffsetinmetersxydouble-x-double-y)
* [`.SetExternalPositioningState(ExternalPositioningState state)`](#setexternalpositioningstateexternalpositioningstate-state)
* [`.SetPowerMode(PowerConsumptionMode mode)`](#setpowermodepowerconsumptionmode-mode)
* [`.SetEstimationMode(EstimationMode mode)`](#setestimationmodeestimationmode-mode)
* [`.EnableARMode(bool state = true)`](#enablearmodebool-state--true)
* [`.EnableBackgroundMode(bool state = true)`](#enablebackgroundmodebool-state--true)
* [`.EnableBinaryFileLogging(bool state = true)`](#enablebinaryfileloggingbool-state--true)
* [`.EnableMapCorrection(bool state = true)`](#enablemapcorrectionbool-state--true)

-----
#### `MotionDna.Init(string key)`
This initializes our MotionDna SDK. You **must** call this function first before doing anything.

**Params**
The `key` parameter is the _Developer Key_ hash that we give you.

**Returns**
A reference to our SDK, which can be used to call further setup functions. All setup functions will also return the same reference, so you can link setup calls together. (see example)

**Example**
```csharp
private const string DEVELOPER_KEY = "/* YOUR DEV KEY */";
void Start ()
{
    MotionDna.Init (DEVELOPER_KEY)
        .SetLocation(...)
        ...
        .EnableMapCorrection();
}
```

-----
#### `.SetLocation(double lat, double long, double head)`
Manually sets the global latitude, longitude, and heading. This enables receiving a latitude and longitude instead of cartesian coordinates.

Use this if you have other sources of information (for example, user-defined address), and need readings more accurate than GPS can provide.

**Params**
The latitude `lat` and longitude `long`, and heading `head` in degrees.

**Returns**
A reference to our MotionDna SDK object.

-----
#### `.SetLocation(double lat, double long)`
Manually set only the global latitude and longitude.

Use this if you have other sources of information (for example, user-defined address), and need readings more accurate than GPS can provide.

**Params**
The latitude `lat` and longitude `long`.

**Returns**
A reference to our MotionDna SDK object.

-----
#### `.SetHeading(double heading)`
Manually set the initial global heading

It is advised if you are using global heading, to manually set this, or allow the user to set this.

**Params**
The `heading`, as a rotation in degrees.

**Returns**
A reference to our MotionDna SDK object.

-----
#### `.ResolveLocation()`
Wait for the next GPS reading, and set the latitude and longitude to that reading.

Given the limitations of GPS, it is best to use this only if you can guarantee the user has accurate GPS readings, and is outside, or you do not need a very accurate position.

**Returns**
A reference to our MotionDna SDK object.

-----
#### `.ResolveHeading()`
Wait for the next magnetic compass reading, and set the heading to that reading.

Given the limitations of the magnetic compass, don't use this unless you are sure the device supports readings at high accuracy.

**Returns**
A reference to our MotionDna SDK object.

-----
#### `.ResolveLocationAndHeading()`
Waits for both GPS and magnetic compass readings, and sets the global latitude, longitude, and heading to those readings.

Note that both GPS and magnetic compass readings may be inaccurate.

**Returns**
A reference to our MotionDna SDK object.

-----
#### `.ComputeLocation()`
Use our internal algorithm to compute your location. Calibration may take some time, and may require the user to walk around a bit.

While we are computing the user's location, you can use [`MotionDna.GetLocationStatus`](#locationstatus-getlocationstatusstring-deviceid--null) to determine when an accurate location has been determined.

**Returns**
A reference to our MotionDna SDK object.

-----
#### `.SetCallbackUpdateRateInMs(double ms)`
Tell our SDK how often to provide estimation results. Note that there is a limit on how fast our SDK can provide results, but usually setting a slower update rate improves results. Setting the rate to `0ms` will output estimation results at our maximum rate.

**Params**
The number of milliseconds `ms` between each update. Setting this to 5000 will publish estimation results every 5 seconds, while setting this to 0 will publish results at the fastest rate our SDK is capable of running estimations (though not consistently timed).

**Returns**
A reference to our MotionDna SDK object.

-----
#### `.SetNetworkUpdateRateInMs(double ms)`
Tell our SDK how often to publish our location to your servers. This allows your device to receive MotionDna information on other devices on the same network.

**Params**
The number of milliseconds `ms` between each network update.

**Returns**
A reference to our MotionDna SDK object.

-----
#### `.SetLocalHeadingOffsetInDegrees(double offset)`
Adds an offset to the device heading. This also rotates the cartesian coordinates appropriately.

**Params**
The heading `offset` in degrees.

**Returns**
A reference to our MotionDna SDK object.

-----
#### `.SetCartesianOffsetInMetersXY(double x, double y)`
Adds an offset to the local cartesian coordinates.

**Params**
The `x` and `y` offset from the cartesian origin in meters.

_**NOTE**: at the current time, this function does not automatically rotate left-handed and right-handed coordinates (Unity and our SDK use different coordinate systems). Therefore, please pass in Unity coordinates `(z, -x)` in place of `(x, y)`_

**Returns**
A reference to our MotionDna SDK object.

-----
#### `.SetExternalPositioningState(ExternalPositioningState state)`
Sets how much integration of GPS data in our estimation results. Note that GPS uses more battery, but can be diabled to save battery. It is recommended to disable this after we have determined your location.

**Params**
`ExternalPositionState state` can be either `OFF`, `HIGH_ACCURACY`, or `LOW_ACCURACY`

**Returns**
A reference to our MotionDna SDK object.

-----
#### `.SetPowerMode(PowerConsumptionMode mode)`
Sets how often to process data by changing the CPU cycles.

**Params**
`PowerConsumptionMode mode` is set to `PERFORMANCE` by default, running at full load. `MEDIUM_CONSUMPTION` reduces updates by 2 times, `LOW_CONSUMPTION` by 4 times, and `SUPER_LOW_CONSUMPTION` by 8 times. At lowest settings, data is processed roughly once every 2 minutes.

**Returns**
A reference to our MotionDna SDK object.

-----
#### `.SetEstimationMode(EstimationMode mode)`
Sets the estimation mode of MotionDna.

**Params**
`EstimationMode mode` is set to `LOCAL` by default. This ensures estimations remain in cartesian space. Setting to `GLOBAL` estimates in spherical & cartesian space so the SDK can be used in a global context.

**Returns**
A reference to our MotionDna SDK object.

-----
#### `.EnableARMode(bool state = true)`
Enables AR mode. AR mode publishes orientation quaternion at a higher rate.

**Params**
`state` whether to enable or disable AR mode

**Returns**
A reference to our MotionDna SDK object.

-----
#### `.EnableBackgroundMode(bool state = true)`
Allow our SDK to continue publishing estimation results while app is in background.

**Params**
`state` whether to enable or disable background mode

**Returns**
A reference to our MotionDna SDK object.

-----
#### `.EnableBinaryFileLogging(bool state = true)`
Allow our SDK to record data and use it to enhance our estimation system.

**Params**
`state` whether to enable or disable binary file logging

**Returns**
A reference to our MotionDna SDK object.

-----
#### `.EnableMapCorrection(bool state = true)`
Allow our SDK to download map data and use it to enhance our estimation system.

**Params**
`state` whether to enable or disable map correction

**Returns**
A reference to our MotionDna SDK object.

-----

## Actions ##
_Actions are used to control our MotionDna behavior._

* [`MotionDna.Pause()`](#motiondnapause)
* [`MotionDna.Resume()`](#motiondnaresume)
* [`MotionDna.Stop()`](#motiondnastop)
* [`MotionDna.StartUDP(string host = null, string port = null, string room = default)`](#motiondnastartudpstring-host--null-string-port--null-string-room--default)
* [`MotionDna.StopUDP()`](#motiondnastopudp)
* [`MotionDna.SetUDPRoom(string room)`](#motiondnasetudproomstring-room)
* [`MotionDna.SendUDP(string message)`](#motiondnasendudpstring-message)
* [`MotionDna.QueryUDPRooms(string[] rooms)`](#motiondnaqueryudproomsstring-rooms)&dagger;
* [`MotionDna.ResetLocalEstimation()`](#motiondnaresetlocalestimation)

&dagger;<sub><sup>This feature not in Unity yet, and is only available for native SDKs. Unity release date TBD.</sup></sub>

-----
#### `MotionDna.Pause()`
Pauses and halts publishing of estimation results. You can still continue getting data with the [getters](#getters), but the data will no longer be realtime.

To continue publishing results, call [`MotionDna.Resume()`](#motiondnaresume)

**Returns**
A reference to our MotionDna SDK object.

-----
#### `MotionDna.Resume()`
Resumes publishing of estimation results.

**Returns**
A reference to our MotionDna SDK object.

-----
#### `MotionDna.Stop()`
Stops `MotionDna` indefinitely, and releases any resources. You cannot use `resume` after this, and no further data will be processed until `MotionDna` has been initialized again.

Although [getters](#getters) will continue functioning, they will no longer be updated. Any other [setup](#setup) functions (except [`Init`](#motiondnainitstring-key)) will fail.

-----
#### `MotionDna.StartUDP(string host = null, string port = null, string room = default)`
Connect to your own server and specify a room. Any other device connected to the same room and also under the same developer will receive any udp packets this device sends.

This device will automatically begin broadcasting the `MotionDna` to your servers, so other devices will receive positional data. Using the [`MotionDna.SendUDP(string message)`](#motiondnasendudpstring-message) call will allow sending arbitrary strings of data, although it is recommended to use only safe and secure characters if communicating with devices of differing OS.

All messages and data are encrypted and sent securely; the server is unable to decode anything - only clients can read and interpret messages.

If `host` or `port` is null, a default public server will be used. Note that the public server has a limited amount of space, and may not always be available for use, so it is best to only use it for testing and instead use your own server for production.

If `room` is not provided, the default room will be used. Also note that the default room has a maximum number of device connections provisioned for it, so this room should only be used for testing and not any formal usage.

**Returns**
A reference to our MotionDna SDK object.

-----
#### `MotionDna.StopUDP()`
Stop broadcasting positional data to your servers.

**Returns**
A reference to our MotionDna SDK object.

-----
#### `MotionDna.SendUDP(string message)`
Send a string of data that will be broadcast to all other devices also in the same server room.

Data is encrypted and sent securely. It is recommended that data use only safe and secure characters to reduce chances of failure.

To receive the messages from other devices, see [`.OnReceiveUDP(string, string)`](#onreceiveudpstring-string)

**Returns**
A reference to our MotionDna SDK object.

-----
#### `MotionDna.QueryUDPRooms(string[] rooms)`&dagger;
Query the server for the current capacity of the listed rooms. Note that the server was designed so that you cannot retrieve a list of rooms without modifying the server. This is to encourage privacy through obscurity.

To receive the results of a query, see [`.OnReceiveUDPRooms(Dictionary)`](#onreceiveudproomsdictionary)

**Returns**
A reference to our MotionDna SDK object.

&dagger;<sub><sup>This feature not in Unity yet, and is only available for native SDKs. Unity release date TBD.</sup></sub>

-----
#### `MotionDna.ResetLocalEstimation()`
Resets the cartesian coordinates back to the origin `(0, 0)`, and sets the heading to `0` degrees.

**Returns**
A reference to our MotionDna SDK object.

-----

## Getters ##
_Getter functions retrieve the most recent data for devices. All device-specific getters require a deviceID when retrieving data. If none is provided, then MotionDna will default to the current device._

#### Example ####
```csharp
// Retrieve the cartesian position of a specified device as a Nullable
string deviceID = "/* retrieve device ID */";
Vector3? devicePosition = MotionDna.GetLocalPosition(deviceID);

// Retrieve the cartesian position of the current device as a Nullable
Vector3? myPosition = MotionDna.GetLocalPosition();

// Certain attributes have default values. Position, for example, defaults to Vector3.zero, and we can use the shorthand notation
Vector3 position = MotionDna.Position;
```

* [`string MotionDna.GetDeviceID()`](#string-motiondnagetdeviceid)
* [`ICollection MotionDna.GetDeviceIDs()`](#icollection-motiondnagetdeviceids)
* [`int GetNumberOfUniqueDevices()`](#int-getnumberofuniquedevices)
* [`LocationStatus GetLocationStatus(string deviceID = null)`](#locationstatus-getlocationstatusstring-deviceid--null)
* [`Vector3 MotionDna.Position`](#vector-motiondnaposition)
* [`Vector3? MotionDna.GetLocalPosition(string deviceID = null)`](#vector-motiondnagetlocalpositionstring-deviceid--null)
* [`GlobalLocation? MotionDna.GetGlobalLocation(string deviceID = null)`](#globallocation-motiondnagetgloballocationstring-deviceid--null)
* [`double MotionDna.Heading`](#double-motiondnaheading)
* [`double? MotionDna.GetLocalHeading(string deviceID = null)`](#double-motiondnagetlocalheadingstring-deviceid--null)
* [`double? MotionDna.GetGlobalHeading(string deviceID = null)`](#double-motiondnagetglobalheadingstring-deviceid--null)
* [`Vector2 MotionDna.Uncertainty`](#vector-motiondnauncertainty)
* [`Vector2? MotionDna.GetUncertainty(string deviceID = null)`](#vector-motiondnagetuncertaintystring-deviceid--null)
* [`VerticalDirection MotionDna.GetVerticalMotionDirection (string deviceID = null)`](#verticaldirection-motiondnagetverticalmotiondirection-string-deviceid--null)
* [`VerticalType MotionDna.GetVerticalMotionType (string deviceID = null)`](#verticaltype-motiondnagetverticalmotiontype-string-deviceid--null)
* [`Attitude? MotionDna.GetAttitude (string deviceID = null)`](#attitude-motiondnagetattitude-string-deviceid--null)
* [`double? MotionDna.GetStepFrequency (string deviceID = null)`](#double-motiondnagetstepfrequency-string-deviceid--null)
* [`PrimaryMotion GetPrimaryMotion (string deviceID = null)`](#primarymotion-getprimarymotion-string-deviceid--null)
* [`SecondaryMotion GetSecondaryMotion (string deviceID = null)`](#secondarymotion-getsecondarymotion-string-deviceid--null)
* [`string GetDeviceName (string deviceID = null)`](#string-getdevicename-string-deviceid--null)
* [`MotionStatistics? GetMotionStatistics (string deviceID = null)`](#motionstatistics-getmotionstatistics-string-deviceid--null)
* [`Quaternion MotionDna.Orientation`](#quaternion-motiondnaorientation)
* [`Quaternion? MotionDna.GetOrientation (string deviceID = null)`](#quaternion-motiondnagetorientation-string-deviceid--null)
* [`float MotionDna.GetTimestamp (string deviceID = null)`](#float-motiondnagettimestamp-string-deviceid--null)

-----
#### `string MotionDna.GetDeviceID()`
Gets the unique identifier of the current device. You can use it with `GetDeviceIDs` to determine what other devices are on the network.

**Returns**
The current device's ID

-----
#### `ICollection MotionDna.GetDeviceIDs()`
Get all unique devices that have been seen since this session started. You must call `StartUDP` if you want to broadcast to and receive results from your server.

**Returns**
A collection of unique identifiers, which can be used to get more information from other devices by passing the ID into different getters.

-----
#### `int GetNumberOfUniqueDevices()`
Returns how many unique devices have been seen

**Returns**
The number of unique devices seen so far.

-----
#### `LocationStatus GetLocationStatus(string deviceID = null)`
Get the location status of a device. The location status is used to determine how the device initialized its global location, for example, manually, by gps, or by our algorithm.

**Params**
Optional `deviceID` of query device. If blank, then the current device is used.

**Returns**
A `LocationStatus`.
* `UNINITIALIZED` if no global location was set
* `INVALID` if no device with the `deviceID` was found
* `USER_INITIALIZED` if location set using latitude, longitude, and heading.
* `GPS_INITIALIZED` if resolved using GPS.
* `NAVISENS_INITIALIZING` if still using the internal algorithm to compute location.
* `NAVISENS_INITIALIZED` if a location was compued using the internal algorithm.
* `BEACON_INITIALIZED` if set via a beacon.

-----
#### `Vector3 MotionDna.Position`
Attempts to get the position of the current device, but will default to `Vector3.zero` if the current device doesn't have a valid MotionDna instance running.

Note that the height value can vary depending on the atmospheric pressure, due to fluctuations in barometer readings. Currently the altitude is not locally calibrated, and developers should either compute the initial reading (which has a few seconds delay due depending on hardware), or synchronize against a local world barometric reading.

**`Get-only` Property**
The position of the current device, if it exists.

-----
#### `Vector3? MotionDna.GetLocalPosition(string deviceID = null)`
Gets the cartesian coordinates of a device.

Note that the height value can vary depending on the atmospheric pressure, due to fluctuations in barometer readings. Currently the altitude is not locally calibrated, and developers should either compute the initial reading (which has a few seconds delay due depending on hardware), or synchronize against a local world barometric reading.

**Params**
Optional `deviceID` of query device. If blank, then the current device is used.

**Returns**
A `Nullable<Vector3>` which contains the position of the query device. Will be `null` only if no valid device was found.

-----
#### `GlobalLocation? MotionDna.GetGlobalLocation(string deviceID = null)`
Gets the global location of a device. Will return `null` if no global coordinates were initialized.

Note that the height value can vary depending on the atmospheric pressure, due to fluctuations in barometer readings. Currently the altitude is not locally calibrated, and developers should either compute the initial reading (which has a few seconds delay due depending on hardware), or synchronize against a local world barometric reading.

**Params**
Optional `deviceID` of query device. If blank, then the current device is used.

**Returns**
A `Nullable<GlobalLocation>` which contains the position of the query device. Will be `null` if either no valid device was found or no global coordinates were initialized.

-----
#### `double MotionDna.Heading`
Gets the local heading of the current device, but will default to 0 if the current device doesn't have a valid MotionDna instance running.

**`Get-only` Property**
The local heading of the current device.

-----
#### `double? MotionDna.GetLocalHeading(string deviceID = null)`
Gets the local heading of a device.

**Params**
Optional `deviceID` of query device. If blank, then the current device is used.

**Returns**
A `Nullable<double>` which contains the local heading of the query device. Will be `null` only if no valid device was found.

-----
#### `double? MotionDna.GetGlobalHeading(string deviceID = null)`
Gets the global heading of a device.

**Params**
Optional `deviceID` of query device. If blank, then the current device is used.

**Returns**
A `Nullable<double>` which contains the global heading of the query device. Will be `null` only if either no valid device was found or no global heading was initialized.

-----
#### `Vector2 MotionDna.Uncertainty`
Gets the cartesian error of the current device, but will default to `Vector2.zero` if the current device doesn't have a valid MotionDna instance running.

**`Get-only` Property**
The cartesian error from the origin of local positional values on the current device.

-----
#### `Vector2? MotionDna.GetUncertainty(string deviceID = null)`
Gets the cartesian error from the origin of local positional coordinates on a device.

**Params**
Optional `deviceID` of query device. If blank, then the current device is used.

**Returns**
A `Nullable<Vector2>` which contains the cartesian error of the query device. Will be `null` only if no valid device was found.

-----
#### `VerticalDirection MotionDna.GetVerticalMotionDirection (string deviceID = null)`
Gets the direction of any detected vertical motion

**Params**
Optional `deviceID` of query device. If blank, then the current device is used.

**Returns**
A `VerticalDirection`
* `UP`
* `DOWN`
* `CONSTANT`
* `INVALID` if no device with the `deviceID` was found

-----
#### `VerticalType MotionDna.GetVerticalMotionType (string deviceID = null)`
Gets the type of detected motion

**Params**
Optional `deviceID` of query device. If blank, then the current device is used.

**Returns**
A `VerticalType`
* `ESCALATOR_STAIRS`
* `ELEVATOR`
* `LEVEL`
* `INVALID` if no device with the `deviceID` was found

-----
#### `Attitude? MotionDna.GetAttitude (string deviceID = null)`
Gets the attitude of a device

**Params**
Optional `deviceID` of query device. If blank, then the current device is used.

**Returns**
A `Nullable<Attitude>` which contains the `pitch`, `roll`, and `yaw` of the query device. Will be `null` only if no valid device was found.

-----
#### `double? MotionDna.GetStepFrequency (string deviceID = null)`
Gets the step frequency

**Params**
Optional `deviceID` of query device. If blank, then the current device is used.

**Returns**
A `Nullable<double>` which contains the step frequency of the query device. Will be `null` only if no valid device was found.

-----
#### `PrimaryMotion GetPrimaryMotion (string deviceID = null)`
Gets the primary motion. (see below)

**Params**
Optional `deviceID` of query device. If blank, then the current device is used.

**Returns**
A `PrimaryMotion` which can be one of the following:
* `STATIONARY` if the device is placed on a stable surface and is not moving
* `FIDGETING` if the device is in the possession of a person, and is moving chaotically
* `FORWARD` if user is walking with the device. Use `GetSecondaryMotion()` to determine how the user is walking
* `INVALID` if no device with the `deviceID` was found

-----
#### `SecondaryMotion GetSecondaryMotion (string deviceID = null)`
Gets the secondary motion, if any. (see below)

**Params**
Optional `deviceID` of query device. If blank, then the current device is used.

**Returns**
A `SecondaryMotion` which can be one of the following:
* `UNDEFINED` if no definite motion was found (chaotic behavior)
* `FORWARD_IN_HAND` if user is holding phone in hand in front of him/her and walking
* `FORWARD_IN_HAND_SWINGING` if user is walking, but phone is by his/her side, usually swinging
* `FORWARD_IN_POCKET` if user is walking with phone in pocket
* `FORWARD_IN_CALL` if user is walking while on a call
* `DWELLING` if user is not walking, but is holding his/her phone
* `JUMPING` if user is jumping
* `INVALID` if no device with the `deviceID` was found

-----
#### `string GetDeviceName (string deviceID = null)`
Gets the name of the device. This can be used to determine what device a user is using, and make appropriate changes. For example, iPhones and iPads will always have the term `iOS` in their names.

**Params**
Optional `deviceID` of query device. If blank, then the current device is used.

**Returns**
A `string` which contains the name of the query device. Will be `null` only if no valid device was found.

-----
#### `MotionStatistics? GetMotionStatistics (string deviceID = null)`
Gets some motion statistics on a device. The motion statistics store a percentages of time spent in certain poses (see below).

**Params**
Optional `deviceID` of query device. If blank, then the current device is used.

**Returns**
A `Nullable<MotionStatistics>` which contains the motion statistics of the query device. Will be `null` only if no valid device was found.

`MotionStatistics` contains three fields `dwelling`, `walking`, and `stationary`. Each field can be a value between 0 and 100, describing the percentage of time the user spent in that pose.
* `dwelling` is defined as when the user is holding the device in their hands, or performing any action other than `walking`.
* `walking` can be any form of detected user walking.
* `stationary` occurs when the device is placed on a stable surface, and does is not moving.

-----
#### `Quaternion MotionDna.Orientation`
Gets the orientation quaternion of the current device, but will default to `Quaternion.identity` if the current device doesn't have a valid MotionDna instance running.

**`Get-only` Property**
The orientation quaternion on the current device.

**Hint**
If you want to set the in-game camera rotation to the same rotation as the user's device, recall that in Unity, the default rotation for a camera faces in the `+z` direction, which is horizontal. However, in most other applications, the camera should face downwards in the `-y` direction.

Since the orientation quaternion of a device is "zero" when facing downwards, in order to convert in Unity, we need to perform a rotation. One way to do this is to add an `Empty` to your scene, and add the camera as a child of it. Then, set the rotation of the `Empty` to the following euler angles: `(90, 0, 0)`. In an update function, simply add
```
    camera.GetComponent<Transform> ().localRotation = MotionDna.Orientation;
```
Remember to call [`.EnableARMode()`](#enablearmodebool-state--true) at [setup](#setup) so the orientation quaternion is published at a higher frequency!

-----
#### `Quaternion? MotionDna.GetOrientation (string deviceID = null)`
Gets the orientation quaternion.

**Params**
Optional `deviceID` of query device. If blank, then the current device is used.

**Returns**
A `Nullable<Quaternion>` which contains the orientation of the query device. Will be `null` only if no valid device was found.

-----
#### `float MotionDna.GetTimestamp (string deviceID = null)`
Gets the timestamp of the latest packet received by a device. This can be used to determine if a device timed out.

**Params**
Optional `deviceID` of query device. If blank, then the current device is used.

**Returns**
A float, equivalent to the Time.time when the latest MotionDna was received.

-----

## Callbacks ##
_Callback functions are invoked from the SDK and pass any external information to Unity, such as network data. To listen for a callback in Unity, you must create a listener class which implements `IMotionDnaUDPListener`. The interface contains three methods you must implement as defined below in [`IMotionDnaUDPListener`](#imotiondnaudplistener). Next, you must add the listener using [`MotionDna.AddMotionDnaUDPListener(IMotionDnaUDPListener)`](#bool-motiondnaaddmotiondnaudplistenerimotiondnaudplistener) (see example below)._

* [`bool MotionDna.AddMotionDnaUDPListener(IMotionDnaUDPListener)`](#bool-motiondnaaddmotiondnaudplistenerimotiondnaudplistener)
* [`bool MotionDna.RemoveMotionDnaUDPListener(IMotionDnaUDPListener)`](#bool-motiondnaremovemotiondnaudplistenerimotiondnaudplistener)
* [`.OnReceiveUDPData(string, string)`](#onreceiveudpdatastring-string)
* [`.OnReceiveUDPDeviceLimit()`](#onreceiveudpdevicelimit)
* [`.OnReceiveUDPServerLimit()`](#onreceiveudpserverlimit)

-----
#### `bool MotionDna.AddMotionDnaUDPListener(IMotionDnaUDPListener)`
Adds an event listener that will be notified of any udp network events defined by the `IMotionDnaUDPListener` interface.

**Params**
A listener abiding by the interface `IMotionDnaUDPListener`.

**Returns**
Whether or not the event listener was successfully added. Will return false if this listener is already listening for events (has already been added).

**Example**
```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour, IMotionDnaUDPListener
{
	private const string DEV_KEY = "/* YOUR DEV KEY HERE */";
	private int number = 0;

	// Use this for initialization
	void Start ()
	{
		MotionDna.Init (DEV_KEY);
		MotionDna.AddMotionDnaUDPListener (this);
		MotionDna.StartUDP ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		MotionDna.SendUDPPacket ("Packet #" + number);
		number++;
	}

	// Event triggers when another device in the same room broadcasts a UDP message
	public void OnReceiveUDPData (string deviceID, string msg)
	{
		Debug.Log ("Received '" + msg + "' from " + deviceID);
	}

	// Event triggers if the current room has reached its maximum device limit
	public void OnReceiveUDPDeviceLimit ()
	{
		Debug.Log ("Error: too many devices in current room");
	}

	// Event triggers if the current server has reached its maximum room limit
	public void OnReceiveUDPServerLimit ()
	{
		Debug.Log ("Error: too many rooms on current server");
	}
}
```

-----
#### `bool MotionDna.RemoveMotionDnaUDPListener(IMotionDnaUDPListener)`
Removes an event listener if it was previously added, stopping it from receiving further network updates.

**Params**
A listener abiding by the interface `IMotionDnaUDPListener` that was previously added.

**Returns**
Whether or not the event listener was successfully found and removed. Will return false if this listener has not been added yet or has already been removed.

-----

### IMotionDnaUDPListener ###
The method signatures below are part of the `IMotionDnaUDPListener` interface. The first three are supported in Unity at the current time, and must be implemented in order for full functionality.

-----
#### `.OnReceiveUDP(string, string)`
This callback serves any messages passed using the [`MotionDna.SendUDP(string message)`](#motiondnasendudpstring-message) to any attached listeners. Messages can only be sent to and received from other devices in the same server room.

**Params**
Listeners will receive two `string`s. The first `string` represents the `deviceID`, while the second `string` holds the decrypted message.

-----
#### `.OnReceiveUDPDeviceLimit()`
This callback serves max device errors to any attached listeners. This error notifies when attempting to connect or switch to a room, but the room has hit the maximum number of devices that may connect to it.

The error will be sent continuously and multiple times until there is available space in the room to connect the current device. Once connected, a device cannot be removed from a room unless it times out or switches rooms.

-----
#### `.OnReceiveUDPServerLimit()`
This callback serves max server errors to any attached listeners. When attempting to connect to a non-existent room, the server will attempt to allocate a new room and add the device to it. This error notifies when attempting to create a new room after the server has exceeded the maximum number of rooms it may create.

The error will only be sent once per connection attempt.

-----
#### `.OnReceiveUDPRooms(Dictionary)`&dagger;
This callback serves a room query request to any attached delegates. Room query requests are those sent from the current device using [`MotionDna.QueryUDPRooms(string[] rooms)`](#motiondnaqueryudproomsstring-rooms).

**Params**
Delegates will receive a `Dictionary` which contains key-value pairs where the key is a room's name, and the value is the number of devices connected to the room, including (if applicable) the current device. No indication is made of a room's capacity.

&dagger;<sub><sup>This feature not in Unity yet, and is only available for native SDKs. Unity release date TBD.</sup></sub>
