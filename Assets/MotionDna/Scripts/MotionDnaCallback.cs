using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionDnaCallback : MonoBehaviour {
	/// <summary>
	/// Callback from native plugin when new Motion Dna is received.
	/// </summary>
	/// <param name="deviceID">Device ID.</param>
	void ReceivedMotionDna (string deviceID)
	{
		MotionDna.Singleton.ReceivedMotionDna (deviceID);
	}

	/// <summary>
	/// Callback from native plugin when authentication fails.
	/// </summary>
	/// <param name="msg">Message.</param>
	void FailedToAuthenticate (string msg)
	{
		Debug.LogError (msg);
	}
}
