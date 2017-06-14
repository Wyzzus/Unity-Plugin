//
//  MotionDna.cs
//  MotionDna
//
//  Created by Joseph on 6/5/17.
//  Copyright © 2017 Navisens. All rights reserved.
//

#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
#define MOBILE
#endif

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MotionDna
{
	private struct XY
	{
		public double x, y;
	}

	private struct XYZ
	{
		public double x, y, z;
	}

	private struct WXYZ
	{
		public double w, x, y, z;
	}

	public struct GlobalLocation
	{
		public double latitude, longitude, altitude;
	}

	public struct Attitude
	{
		public double roll, pitch, yaw;
	}

	public struct MotionStatistics
	{
		public double dwelling, walking, stationary;
	}

	private class Device
	{
		public NativeDevice nativeDevice;
		public XYZ localLocation;
		public GlobalLocation globalLocation;
		public XY uncertainty;
		public Attitude attitude;
		public MotionStatistics motionStatistics;
		public WXYZ quaternion;

		public float timestamp;

		public Device ()
		{
			nativeDevice = new NativeDevice ();
			localLocation = new XYZ ();
			globalLocation = new GlobalLocation ();
			uncertainty = new XY ();
			attitude = new Attitude ();
			motionStatistics = new MotionStatistics ();
			quaternion = new WXYZ ();
		}

		public static implicit operator bool (Device device)
		{
			return !object.ReferenceEquals (device, null);
		}
	}

	private struct NativeDevice
	{
		public int locationStatus;
		//		public XYZ localLocation;
		//		public GlobalLocation globalLocation;
		public double heading;
		public double localHeading;
		//		public XY uncertainty;
		public int direction;
		public int type;

		//		public Attitude attitude;

		public double stepFrequency;
		public int primaryMotion;
		public int secondaryMotion;

		public string deviceName;

		//		public MotionStatistics motionStatistics;

		//		public WXYZ quaternion;
	}

	public enum PrimaryMotion
	{
		STATIONARY = 0,
		FIDGETING,
		FORWARD,
		INVALID = -1}

	;

	public enum SecondaryMotion
	{
		UNDEFINED = 0,
		FORWARD_IN_HAND,
		FORWARD_IN_HAND_SWINGING,
		FORWARD_IN_POCKET,
		FORWARD_IN_CALL,
		DWELLING,
		JUMPING,
		INVALID = -1}

	;

	public enum PowerConsumptionMode
	{
		SUPER_LOW_CONSUMPTION,
		LOW_CONSUMPTION,
		MEDIUM_CONSUMPTION,
		PERFORMANCE}

	;

	public enum ExternalPositioningState
	{
		OFF,
		HIGH_ACCURACY,
		LOW_ACCURACY,
		EXTERNAL_UNDEFINED}

	;

	public enum VerticalDirection
	{
		UP = 0,
		DOWN,
		CONSTANT,
		INVALID = -1}

	;

	public enum VerticalType
	{
		ESCALATOR_STAIRS,
		ELEVATOR,
		LEVEL,
		INVALID = -1}

	;

	/// <summary>
	/// UNINITIALIZED if no location was initialized.
	/// USER_INITIALIZED if location set using latitude, longitude, and heading.
	/// GPS_INITIALIZED if resolved using GPS.
	/// NAVISENS_INITIALIZING if using the internal algorithm to compute location.
	/// NAVISENS_INITIALIZED if a location was compued using the internal algorithm.
	/// BEACON_INITIALIZED if set via a beacon.
	/// INVALID if no device was found when retrieving location status.
	/// </summary>
	public enum LocationStatus
	{
		UNINITIALIZED = 0,
		USER_INITIALIZED,
		GPS_INITIALIZED,
		NAVISENS_INITIALIZING,
		NAVISENS_INITIALIZED,
		BEACON_INITIALIZED,
		INVALID = -1}

	;

	public enum EstimationMode
	{
		LOCAL,
		GLOBAL}

	;

	// Extern Functions
	// ================================================================================ //

	#if UNITY_IOS
	const string dllName = "__Internal";
	// #elif UNITY_ANDROID
	// const string dllName = "MotionDna";
	#else
	const string dllName = "null";
	#endif

	[DllImport (dllName)]
	private static extern void _StartMotionDna (string devKey);

	[DllImport (dllName)]
	private static extern void _StopMotionDna ();

	[DllImport (dllName)]
	private static extern void _SetLocationLatitudeLongitudeAndHeadingInDegrees (double latitude, double longitude, double heading);

	[DllImport (dllName)]
	private static extern void _SetLocationAndHeadingGPSMag ();

	[DllImport (dllName)]
	private static extern void _SetLocationLatitudeLongitude (double latitude, double longitude);

	[DllImport (dllName)]
	private static extern void _SetLocationGPSOnly ();

	[DllImport (dllName)]
	private static extern void _SetHeadingMagInDegrees ();

	[DllImport (dllName)]
	private static extern void _SetHeadingInDegrees (double heading);

	[DllImport (dllName)]
	private static extern void _SetLocationNavisens ();

	[DllImport (dllName)]
	private static extern void _Pause ();

	[DllImport (dllName)]
	private static extern void _Resume ();

	[DllImport (dllName)]
	private static extern void _SetMapCorrectionEnabled (bool state);

	[DllImport (dllName)]
	private static extern void _SetCallbackUpdateRateInMs (double rate);

	[DllImport (dllName)]
	private static extern void _SetNetworkUpdateRateInMs (double rate);

	[DllImport (dllName)]
	private static extern void _SetBinaryFileLoggingEnabled (bool state);

	[DllImport (dllName)]
	private static extern void _SetExternalPositioningState (int state);

	[DllImport (dllName)]
	private static extern void _StartUDP ();

	[DllImport (dllName)]
	private static extern void _StopUDP ();

	[DllImport (dllName)]
	private static extern void _SetBackgroundModeEnabled (bool state);

	[DllImport (dllName)]
	private static extern void _SetPowerMode (int mode);

	[DllImport (dllName)]
	private static extern void _SetLocalHeadingOffsetInDegrees (double hdg);

	[DllImport (dllName)]
	private static extern void _SetCartesianOffsetInMetersXY (double x, double y);

	[DllImport (dllName)]
	private static extern void _SetARModeEnabled (bool state);

	[DllImport (dllName)]
	private static extern void _SetEstimationMode (int mode);

	[DllImport (dllName)]
	private static extern void _ResetLocalEstimation ();

	[DllImport (dllName)]
	private static extern string _GetDeviceID ();

	[DllImport (dllName)]
	private static extern void _GetDevice (string deviceID, ref NativeDevice device, ref XYZ localLocation, ref GlobalLocation globalLocation, ref XY uncertainty, ref Attitude attitude, ref MotionStatistics motionStatistics, ref WXYZ quaternion);

	// Variables
	// ================================================================================ //

	public static MotionDna Singleton { get { return _singleton; } }

	private static MotionDna _singleton = new MotionDna ();
	private static string deviceID = "";
	private static Dictionary<string, Device> devices = new Dictionary<string, Device> ();
	private static Device device;

	// Helpers
	// ================================================================================ //

	/// <summary>
	/// Notified by MotionDna when a new MotionDna is ready, defined by the deviceID.
	/// 
	/// Do not call this method unless you need to force update the data attributes of a device due to an internal error.
	/// The timestamp will be incorrect if you call this yourself.
	/// This method is also very costly as it must make a managed-to-native call, and you should refrain from using it multiple times each frame.
	/// </summary>
	/// <param name="deviceID">Device ID.</param>
	public void ReceivedMotionDna (string deviceID)
	{
		if (!devices.ContainsKey (deviceID)) {
			devices.Add (deviceID, new Device ());
		}
		Device device = devices [deviceID];
		_GetDevice (deviceID, ref device.nativeDevice, ref device.localLocation, ref device.globalLocation, ref device.uncertainty, ref device.attitude, ref device.motionStatistics, ref device.quaternion);
		device.timestamp = Time.time;
	}

	/// <summary>
	/// Gets the device. If deviceID is null, then the current device is used instead.
	/// </summary>
	/// <returns>The device.</returns>
	/// <param name="deviceID">Device ID.</param>
	private static Device GetDevice (string deviceID)
	{
		if (deviceID == null)
			deviceID = GetDeviceID ();
		if (devices.ContainsKey (deviceID))
			return devices [deviceID];
		return null;
	}

	// Interface
	// ================================================================================ //

	/// <summary>
	/// Gets the current device's ID.
	/// </summary>
	/// <returns>The device ID.</returns>
	public static string GetDeviceID ()
	{
		#if MOBILE
		if (deviceID.Length == 0)
			deviceID = _GetDeviceID ();
		#endif
		return deviceID;
	}

	/// <summary>
	/// Gets all seen devices' IDs.
	/// Call GetTimestamp(deviceID) for each device's latest timestamp to verify if the device is still active.
	/// </summary>
	/// <returns>A collection of device IDs.</returns>
	public static ICollection GetDeviceIDs ()
	{
		return devices.Keys;
	}

	/// <summary>
	/// Gets the total number of unique devices seen.
	/// </summary>
	/// <returns>The devices count.</returns>
	public static int GetNumberOfUniqueDevices ()
	{
		return devices.Count;
	}

	/// <summary>
	/// Gets the location status of the device with id deviceID, if it exists.
	/// <para></para>
	/// If no deviceID is provided, then default to the current device instead.
	/// </summary>
	/// <returns>The location status.</returns>
	/// <param name="deviceID">Device ID.</param>
	public static LocationStatus GetLocationStatus (string deviceID = null)
	{
		int ordinal = -1;
		if (device = GetDevice (deviceID))
			ordinal = device.nativeDevice.locationStatus;
		return (LocationStatus)ordinal;
	}

	/// <summary>
	/// Gets the local position of the current device, or Vector3.zero if a device could not be found.
	/// Shorthand for MotionDna.GetLocalPosition() ?? Vector3.zero.
	/// </summary>
	/// <value>The position.</value>
	public static Vector3 Position {
		get {
			return GetLocalPosition () ?? Vector3.zero;
		}
	}

	/// <summary>
	/// Gets the local position of the device with id deviceID, if it exists, or null if it does not.
	/// <para></para>
	/// If no deviceID is provided, then default to the current device instead.
	/// </summary>
	/// <returns>The local position.</returns>
	/// <param name="deviceID">Device ID.</param>
	public static Vector3? GetLocalPosition (string deviceID = null)
	{
		if (device = GetDevice (deviceID)) {
			XYZ localLocation = device.localLocation;
			return new Vector3 ((float)-localLocation.y, (float)localLocation.z, (float)localLocation.x);
		}
		return null;
	}

	/// <summary>
	/// Gets the global location of the device with id deviceID, if it exists, or null if it does not.
	/// 
	/// If the location status is UNINITIALIZED, this will always return null.
	/// <para></para>
	/// If no deviceID is provided, then default to the current device instead.
	/// </summary>
	/// <returns>The global position.</returns>
	/// <param name="deviceID">Device ID.</param>
	public static GlobalLocation? GetGlobalLocation (string deviceID = null)
	{
		if (device = GetDevice (deviceID)) {
			if (device.nativeDevice.locationStatus == (int)LocationStatus.UNINITIALIZED)
				return null;
			return device.globalLocation;
		}
		return null;
	}

	/// <summary>
	/// Gets the global heading of the device with id deviceID, if it exists, or null if it does not.
	/// 
	/// If the location status is UNINITIALIZED, this will always return null.
	/// <para></para>
	/// If no deviceID is provided, then default to the current device instead.
	/// </summary>
	/// <returns>The global heading.</returns>
	/// <param name="deviceID">Device ID.</param>
	public static double? GetGlobalHeading (string deviceID = null)
	{
		if (device = GetDevice (deviceID)) {
			if (device.nativeDevice.locationStatus == (int)LocationStatus.UNINITIALIZED)
				return null;
			return device.nativeDevice.heading;
		}
		return null;
	}

	/// <summary>
	/// Gets the heading of the current device, or 0 if no device could be found.
	/// Shorthand for MotionDna.GetLocalHeading() ?? 0.
	/// </summary>
	/// <value>The heading.</value>
	public static double Heading {
		get {
			return GetLocalHeading () ?? 0;
		}
	}

	/// <summary>
	/// Gets the local heading of the device with id deviceID, if it exists, or null if it does not.
	/// <para></para>
	/// If no deviceID is provided, then default to the current device instead.
	/// </summary>
	/// <returns>The local heading.</returns>
	/// <param name="deviceID">Device ID.</param>
	public static double? GetLocalHeading (string deviceID = null)
	{
		if (device = GetDevice (deviceID))
			return device.nativeDevice.localHeading;
		return null;
	}

	/// <summary>
	/// Gets the uncertainty of the current device, or Vector2.zero if a device could not be found.
	/// Shorthand for MotionDna.GetUncertainty() ?? Vector2.zero.
	/// </summary>
	/// <value>The uncertainty.</value>
	public static Vector2 Uncertainty {
		get {
			return GetUncertainty () ?? Vector2.zero;
		}
	}

	/// <summary>
	/// Gets the cartesian uncertainty of the device with id deviceID, if it exists, or null if it does not.
	/// 
	/// The uncertainty is the error from the initial Cartesian location.
	/// <para></para>
	/// If no deviceID is provided, then default to the current device instead.
	/// </summary>
	/// <returns>The cartesian uncertainty.</returns>
	/// <param name="deviceID">Device ID.</param>
	public static Vector2? GetUncertainty (string deviceID = null)
	{
		if (device = GetDevice (deviceID)) {
			XY uncertainty = device.uncertainty;
			return new Vector2 ((float)uncertainty.x, (float)uncertainty.y);
		}
		return null;
	}

	/// <summary>
	/// Gets the vertical motion direction of the device with id deviceID, if it exists, or INVALID if it does not.
	/// <para></para>
	/// If no deviceID is provided, then default to the current device instead.
	/// </summary>
	/// <returns>The vertical motion direction.</returns>
	/// <param name="deviceID">Device ID.</param>
	public static VerticalDirection GetVerticalMotionDirection (string deviceID = null)
	{
		int ordinal = -1;
		if (device = GetDevice (deviceID))
			ordinal = device.nativeDevice.direction;
		return (VerticalDirection)ordinal;
	}

	/// <summary>
	/// Gets the vertical motion type of the device with id deviceID, if it exists, or INVALID if it does not.
	/// <para></para>
	/// If no deviceID is provided, then default to the current device instead.
	/// </summary>
	/// <returns>The vertical motion type.</returns>
	/// <param name="deviceID">Device ID.</param>
	public static VerticalType GetVerticalMotionType (string deviceID = null)
	{
		int ordinal = -1;
		if (device = GetDevice (deviceID))
			ordinal = device.nativeDevice.type;
		return (VerticalType)ordinal;
	}

	/// <summary>
	/// Gets the attitude of the device with id deviceID, if it exists, or null if it does not.
	/// <para></para>
	/// If no deviceID is provided, then default to the current device instead.
	/// </summary>
	/// <returns>The attitude.</returns>
	/// <param name="deviceID">Device ID.</param>
	public static Attitude? GetAttitude (string deviceID = null)
	{
		if (device = GetDevice (deviceID))
			return device.attitude;
		return null;
	}

	/// <summary>
	/// Gets the step frequency of the device with id deviceID, if it exists, or null if it does not.
	/// <para></para>
	/// If no deviceID is provided, then default to the current device instead.
	/// </summary>
	/// <returns>The step frequency.</returns>
	/// <param name="deviceID">Device ID.</param>
	public static double? GetStepFrequency (string deviceID = null)
	{
		if (device = GetDevice (deviceID))
			return device.nativeDevice.stepFrequency;
		return null;
	}

	/// <summary>
	/// Gets the primary motion of the device with id deviceID, if it exists, or INVALID if it does not.
	/// <para></para>
	/// If no deviceID is provided, then default to the current device instead.
	/// </summary>
	/// <returns>The primary motion.</returns>
	/// <param name="deviceID">Device ID.</param>
	public static PrimaryMotion GetPrimaryMotion (string deviceID = null)
	{
		int ordinal = -1;
		if (device = GetDevice (deviceID))
			ordinal = device.nativeDevice.primaryMotion;
		return (PrimaryMotion)ordinal;
	}

	/// <summary>
	/// Gets the secondary motion of the device with id deviceID, if it exists, or INVALID if it does not.
	/// <para></para>
	/// If no deviceID is provided, then default to the current device instead.
	/// </summary>
	/// <returns>The secondary motion.</returns>
	/// <param name="deviceID">Device ID.</param>
	public static SecondaryMotion GetSecondaryMotion (string deviceID = null)
	{
		int ordinal = -1;
		if (device = GetDevice (deviceID))
			ordinal = device.nativeDevice.secondaryMotion;
		return (SecondaryMotion)ordinal;
	}

	/// <summary>
	/// Gets the name of the device with id deviceID, if it exists, or null if it does not.
	/// <para></para>
	/// If no deviceID is provided, then default to the current device instead.
	/// </summary>
	/// <returns>The device name.</returns>
	/// <param name="deviceID">Device ID.</param>
	public static string GetDeviceName (string deviceID = null)
	{
		if (device = GetDevice (deviceID))
			return device.nativeDevice.deviceName;
		return null;
	}

	/// <summary>
	/// Gets the motion statistics of the device with id deviceID, if it exists, or null if it does not.
	/// <para></para>
	/// If no deviceID is provided, then default to the current device instead.
	/// </summary>
	/// <returns>The motion statistics.</returns>
	/// <param name="deviceID">Device I.</param>
	public static MotionStatistics? GetMotionStatistics (string deviceID = null)
	{
		if (device = GetDevice (deviceID))
			return device.motionStatistics;
		return null;
	}

	/// <summary>
	/// The orientation quaternion of the current device, or Quaternion.identity if a device could not be found.
	/// Shorthand for MotionDna.GetOrientation() ?? Quaternion.identity.
	/// </summary>
	/// <value>The orientation.</value>
	public static Quaternion Orientation {
		get {
			return GetOrientation () ?? Quaternion.identity;
		}
	}

	/// <summary>
	/// Gets the orientation quaternion of the device with id deviceID, if it exists, or null if it does not.
	/// <para></para>
	/// If no deviceID is provided, then default to the current device instead.
	/// </summary>
	/// <returns>The orientation quaternion.</returns>
	/// <param name="deviceID">Device ID.</param>
	public static Quaternion? GetOrientation (string deviceID = null)
	{
		if (device = GetDevice (deviceID)) {
			WXYZ quaternion = device.quaternion;
			return new Quaternion ((float)quaternion.y, (float)-quaternion.x, (float)quaternion.z, (float)quaternion.w);
		}
		return null;
	}

	/// <summary>
	/// Gets the timestamp equal to Time.time, of the most recent received packet from the device with id deviceID, if it exists, or -1 if it does not.
	/// <para></para>
	/// If no deviceID is provided, then default to the current device instead.
	/// </summary>
	/// <returns>The timestamp.</returns>
	/// <param name="deviceID">Device ID.</param>
	public static float GetTimestamp (string deviceID = null)
	{
		if (device = GetDevice (deviceID))
			return device.timestamp;
		return -1;
	}

	// SDK Settings
	// ================================================================================ //

	/// <summary>
	/// Starts up a MotionDna object and the SDK and begins publishing estimation results.
	/// </summary>
	/// <param name="devKey">Developer key.</param>
	public static MotionDna Init (string devKey)
	{
		#if MOBILE
		_StartMotionDna (devKey);
		#endif
		return _singleton;
	}

	/// <summary>
	/// Shuts down the SDK, and releases all resources.
	/// </summary>
	public static void Stop ()
	{
		#if MOBILE
		_StopMotionDna ();
		#endif
		_singleton = null;
	}

	/// <summary>
	/// Sets the location using latitude and longitude, and heading in degrees.
	/// </summary>
	/// <param name="latitude">Latitude.</param>
	/// <param name="longitude">Longitude.</param>
	/// <param name="heading">Heading in degrees.</param>
	public MotionDna SetLocation (double latitude, double longitude, double heading)
	{
		#if MOBILE
		_SetLocationLatitudeLongitudeAndHeadingInDegrees (latitude, longitude, heading);
		#endif
		return _singleton;
	}

	/// <summary>
	/// Sets the location and heading using GPS and magnetic compass.
	/// </summary>
	public MotionDna ResolveLocationAndHeading ()
	{
		#if MOBILE
		_SetLocationAndHeadingGPSMag ();
		#endif
		return _singleton;
	}

	/// <summary>
	/// Sets the location using latitude and longitude.
	/// </summary>
	/// <param name="latitude">Latitude.</param>
	/// <param name="longitude">Longitude.</param>
	public MotionDna SetLocation (double latitude, double longitude)
	{
		#if MOBILE
		_SetLocationLatitudeLongitude (latitude, longitude);
		#endif
		return _singleton;
	}

	/// <summary>
	/// Sets the location using the GPS only.
	/// </summary>
	public MotionDna ResolveLocation ()
	{
		#if MOBILE
		_SetLocationGPSOnly ();
		#endif
		return _singleton;
	}

	/// <summary>
	/// Sets the heading using the magnetic compass in degrees.
	/// </summary>
	public MotionDna ResolveHeading ()
	{
		#if MOBILE
		_SetHeadingMagInDegrees ();
		#endif
		return _singleton;
	}

	/// <summary>
	/// Sets the heading in degrees.
	/// </summary>
	/// <param name="heading">Heading.</param>
	public MotionDna SetHeading (double heading)
	{
		#if MOBILE
		_SetHeadingInDegrees (heading);
		#endif
		return _singleton;
	}

	/// <summary>
	/// Computes the location using an internal algorithm. Location callibration may require walking 1-2 blocks.
	/// </summary>
	public MotionDna ComputeLocation ()
	{
		#if MOBILE
		_SetLocationNavisens ();
		#endif
		return _singleton;
	}

	/// <summary>
	/// Pauses MotionDna. Estimation results will not be updated until resume is called.
	/// </summary>
	public static MotionDna Pause ()
	{
		#if MOBILE
		_Pause ();
		#endif
		return _singleton;
	}

	/// <summary>
	/// Resumes MotionDna. Estimation results will continue to update.
	/// </summary>
	public static MotionDna Resume ()
	{
		#if MOBILE
		_Resume ();
		#endif
		return _singleton;
	}

	/// <summary>
	/// Enables or disables map correction. This requires internet access to cache maps.
	/// </summary>
	/// <param name="state">If set to <c>true</c> state.</param>
	public MotionDna EnableMapCorrection (bool state = true)
	{
		#if MOBILE
		_SetMapCorrectionEnabled (state);
		#endif
		return _singleton;
	}

	/// <summary>
	/// Sets the callback update rate in ms.
	/// </summary>
	/// <param name="rate">Period between updates.</param>
	public MotionDna SetCallbackUpdateRateInMs (double rate)
	{
		#if MOBILE
		_SetCallbackUpdateRateInMs (rate);
		#endif
		return _singleton;
	}

	/// <summary>
	/// Sets the network update rate in ms.
	/// </summary>
	/// <param name="rate">Period between updates.</param>
	public MotionDna SetNetworkUpdateRateInMs (double rate)
	{
		#if MOBILE
		_SetNetworkUpdateRateInMs (rate);
		#endif
		return _singleton;
	}

	/// <summary>
	/// Enables or disables binary file logging.
	/// </summary>
	/// <param name="state">If set to <c>true</c> state.</param>
	public MotionDna EnableBinaryFileLogging (bool state = true)
	{
		#if MOBILE
		_SetBinaryFileLoggingEnabled (state);
		#endif
		return _singleton;
	}

	/// <summary>
	/// Enables or disables GPS, determined by accuracy required.
	/// </summary>
	/// <param name="state">External positioning state.</param>
	public MotionDna SetExternalPositioningState (ExternalPositioningState state)
	{
		#if MOBILE
		_SetExternalPositioningState ((int)state);
		#endif
		return _singleton;
	}

	/// <summary>
	/// Starts broadcasting and receiving, allowing this device's Motion Dna to be seen by other devices.
	/// </summary>
	public static MotionDna StartUDP ()
	{
		#if MOBILE
		_StartUDP ();
		#endif
		return _singleton;
	}

	/// <summary>
	/// Stops broadcasting and receiving.
	/// </summary>
	public static MotionDna StopUDP ()
	{
		#if MOBILE
		_StopUDP ();
		#endif
		return _singleton;
	}

	/// <summary>
	/// Enables or disables background mode.
	/// </summary>
	/// <param name="state">If set to <c>true</c> state.</param>
	public MotionDna EnableBackgroundMode (bool state = true)
	{
		#if MOBILE
		_SetBackgroundModeEnabled (state);
		#endif
		return _singleton;
	}

	/// <summary>
	/// Sets the power consumption mode.
	/// </summary>
	/// <param name="mode">Power consumption mode.</param>
	public MotionDna SetPowerMode (PowerConsumptionMode mode)
	{
		#if MOBILE
		_SetPowerMode ((int)mode);
		#endif
		return _singleton;
	}

	/// <summary>
	/// Sets the local heading offset in degrees.
	/// </summary>
	/// <param name="hdg">Hdg.</param>
	public MotionDna SetLocalHeadingOffsetInDegrees (double hdg)
	{
		#if MOBILE
		_SetLocalHeadingOffsetInDegrees (hdg);
		#endif
		return _singleton;
	}

	/// <summary>
	/// Sets the cartesian XY offset in meters.
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	public MotionDna SetCartesianOffsetInMetersXY (double x, double y)
	{
		#if MOBILE
		_SetCartesianOffsetInMetersXY (x, y);
		#endif
		return _singleton;
	}

	/// <summary>
	/// Enables or disables AR mode. AR mode broadcasts orientation data at a higher frequency.
	/// </summary>
	/// <param name="state">If set to <c>true</c> state.</param>
	public MotionDna EnableARMode (bool state = true)
	{
		#if MOBILE
		_SetARModeEnabled (state);
		#endif
		return _singleton;
	}

	/// <summary>
	/// Sets the estimation mode.
	/// </summary>
	/// <param name="mode">Estimation mode.</param>
	public MotionDna SetEstimationMode (EstimationMode mode)
	{
		#if MOBILE
		_SetEstimationMode ((int)mode);
		#endif
		return _singleton;
	}

	/// <summary>
	/// Resets the local estimation.
	/// </summary>
	public static MotionDna ResetLocalEstimation ()
	{
		#if MOBILE
		_ResetLocalEstimation ();
		#endif
		return _singleton;
	}
}
