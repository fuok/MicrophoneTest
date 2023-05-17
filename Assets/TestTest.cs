using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestTest : MonoBehaviour
{
	public Text txtDEV, txtVOC;
	public Button btnStartRecord, btnStopRecord, btnPlayRecord;
	public Button btnPlayStream;
	public Rigidbody mCube;

	void Start ()
	{
		//补充这部分内容
		btnStartRecord.onClick.AddListener (() => {
			MicroPhoneInput.getInstance ().StartRecord ();
		});
		btnStopRecord.onClick.AddListener (() => {
			MicroPhoneInput.getInstance ().StopRecord ();
		});
		btnPlayRecord.onClick.AddListener (() => {
			MicroPhoneInput.getInstance ().PlayRecord ();
		});
		btnPlayStream.onClick.AddListener (() => {
			MicroPhoneInput.getInstance ().PlayClipData (MicroPhoneInput.getInstance ().GetClipDataInFloat ());
		});
	}

	void Update ()
	{
		string[] d = MicroPhoneInput.getInstance ().GetDeviceList ();
		for (int i = 0; i < d.Length; i++) {
			txtDEV.text = d [i];
		}

		float volume = MicroPhoneInput.getInstance ().GetAveragedVolume ();
		if (volume > 0f) {
			txtVOC.text = volume.ToString ();
			mCube.AddForce (Vector3.up * volume * 300f);
			
		}
	}
}
