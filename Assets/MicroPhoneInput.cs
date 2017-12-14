using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[RequireComponent (typeof(AudioSource))]
public class MicroPhoneInput : MonoBehaviour
{

	private static MicroPhoneInput m_instance;

	public float sensitivity = 100;
	public float loudness = 0;

	private static string[] micArray = null;

	const int HEADER_SIZE = 44;

	const int RECORD_TIME = 600;
	//10//3600

	//	void Start ()
	//	{
	//		foreach (string device in Microphone.devices) {
	//			Debug.Log ("Name: " + device);
	//			txtDEV.text = device;
	//		}
	//	}

	void Update ()
	{
		loudness = GetAveragedVolume () * sensitivity;
		if (loudness > 1) {
			Debug.Log ("loudness = " + loudness);
		}

//		if (Volume > 0) {
//			print ("Volume=" + Volume);
//		}
	}

	public static MicroPhoneInput getInstance ()
	{
		if (m_instance == null) {
			micArray = Microphone.devices;
			if (micArray.Length == 0) {
				Debug.LogError ("Microphone.devices is null");
			}
			foreach (string deviceStr in Microphone.devices) {
				Debug.Log ("device name = " + deviceStr);
			}
			if (micArray.Length == 0) {
				Debug.LogError ("no mic device");
			}

			GameObject MicObj = new GameObject ("MicObj");
			m_instance = MicObj.AddComponent<MicroPhoneInput> ();
		}
		return m_instance;
	}

	public void StartRecord ()
	{
		GetComponent<AudioSource> ().Stop ();
		if (micArray.Length == 0) {
			Debug.Log ("No Record Device!");
			return;
		}
		GetComponent<AudioSource> ().Stop ();//不知道是否必要的
		GetComponent<AudioSource> ().loop = false;//同上，因为下面还有个loop
		GetComponent<AudioSource> ().mute = false;//必须打开，否则没声音录进去
		GetComponent<AudioSource> ().clip = Microphone.Start (micArray [0], true, RECORD_TIME, 128); //22050 //44100//采样率，两边相差很大，开高会怎样？
		while (!(Microphone.GetPosition (null) > 0)) {
		}
		GetComponent<AudioSource> ().Play ();//经测试，这里必须打开，否则也不会有声音输出
		Debug.Log ("StartRecord");
		//倒计时
//		StartCoroutine (TimeDown ());//没用的关闭

	}

	//这里是倒计时停止录音，另一代码中是角色死后停止
	public  void StopRecord ()
	{
		if (micArray.Length == 0) {
			Debug.Log ("No Record Device!");
			return;
		}
		if (!Microphone.IsRecording (null)) {
			return;
		}
		Microphone.End (micArray [0]);
		GetComponent<AudioSource> ().Stop ();

		Debug.Log ("StopRecord");

	}

	/// <summary>
	/// 获取字节数组的版本
	/// </summary>
	/// <returns>The clip data in byte.</returns>
	public Byte[] GetClipDataInByte ()
	{
		if (GetComponent<AudioSource> ().clip == null) {
			Debug.Log ("GetClipData audio.clip is null");
			return null; 
		}

		float[] samples = new float[GetComponent<AudioSource> ().clip.samples];

		GetComponent<AudioSource> ().clip.GetData (samples, 0);


		Byte[] outData = new byte[samples.Length * 2];
		//Int16[] intData = new Int16[samples.Length];
		//converting in 2 float[] steps to Int16[], //then Int16[] to Byte[]

		int rescaleFactor = 32767; //to convert float to Int16

		for (int i = 0; i < samples.Length; i++) {
			short temshort = (short)(samples [i] * rescaleFactor);

			Byte[] temdata = System.BitConverter.GetBytes (temshort);

			outData [i * 2] = temdata [0];
			outData [i * 2 + 1] = temdata [1];


		}
		if (outData == null || outData.Length <= 0) {
			Debug.Log ("GetClipData intData is null");
			return null; 
		}
		//return intData;
		return outData;
	}

	/// <summary>
	/// Unity默认的clip.GetData是float[]
	/// </summary>
	/// <returns>The clip data in float.</returns>
	public float[] GetClipDataInFloat ()
	{
		if (GetComponent<AudioSource> ().clip == null) {
			Debug.Log ("GetClipData audio.clip is null");
			return null; 
		}
		float[] samples = new float[GetComponent<AudioSource> ().clip.samples];
		GetComponent<AudioSource> ().clip.GetData (samples, 0);
		return samples;
	}

	/// <summary>
	/// 输入Int16的版本
	/// </summary>
	/// <param name="intArr">Int arr.</param>
	public void PlayClipData (Int16[] intArr)
	{
		string aaastr = intArr.ToString ();
		long aaalength = aaastr.Length;
		Debug.LogError ("aaalength=" + aaalength);

		string aaastr1 = Convert.ToString (intArr);
		aaalength = aaastr1.Length;
		Debug.LogError ("aaalength=" + aaalength);

		if (intArr.Length == 0) {
			Debug.Log ("get intarr clipdata is null");
			return;
		}
		//从Int16[]到float[]
		float[] samples = new float[intArr.Length];
		int rescaleFactor = 32767;
		for (int i = 0; i < intArr.Length; i++) {
			samples [i] = (float)intArr [i] / rescaleFactor;
		}

		//从float[]到Clip
		AudioSource audioSource = gameObject.GetComponent<AudioSource> ();
		if (audioSource.clip == null) {
			audioSource.clip = AudioClip.Create ("playRecordClip", intArr.Length, 1, 44100, false, false);
		}
		audioSource.clip.SetData (samples, 0);
		audioSource.mute = false;
		audioSource.Play ();
	}

	/// <summary>
	/// 播放默认的float[]
	/// </summary>
	/// <param name="intArr">Int arr.</param>
	public void PlayClipData (float[] intArr)
	{
//		string aaastr = intArr.ToString ();
//		long aaalength = aaastr.Length;
//		Debug.LogError ("aaalength=" + aaalength);
//
//		string aaastr1 = Convert.ToString (intArr);
//		aaalength = aaastr1.Length;
//		Debug.LogError ("aaalength=" + aaalength);

		if (intArr.Length == 0) {
			Debug.Log ("get intarr clipdata is null");
			return;
		}
		//从Int16[]到float[]
//		float[] samples = new float[intArr.Length];
//		int rescaleFactor = 32767;
//		for (int i = 0; i < intArr.Length; i++) {
//			samples [i] = (float)intArr [i] / rescaleFactor;
//		}

		//从float[]到Clip
		AudioSource audioSource = gameObject.GetComponent<AudioSource> ();
		if (audioSource.clip == null) {
			audioSource.clip = AudioClip.Create ("playRecordClip", intArr.Length, 1, 44100, false, false);
		}
		audioSource.clip.SetData (intArr, 0);
		audioSource.mute = false;
		audioSource.Play ();
	}

	public void PlayRecord ()
	{
		if (GetComponent<AudioSource> ().clip == null) {
			Debug.Log ("audio.clip=null");
			return;
		}
		GetComponent<AudioSource> ().mute = false;
		GetComponent<AudioSource> ().loop = false;
		GetComponent<AudioSource> ().Play ();
		Debug.Log ("PlayRecord:" + GetComponent<AudioSource> ().clip.length);

	}

	public float GetAveragedVolume ()
	{
		float[] data = new float[256];
		float a = 0;
		GetComponent<AudioSource> ().GetOutputData (data, 0);
		foreach (float s in data) {
			a += Mathf.Abs (s);
		}
		return a / 256;
	}

	public string[] GetDeviceList ()
	{
		return micArray;
	}

	private IEnumerator TimeDown ()
	{
		Debug.Log (" IEnumerator TimeDown()");

		int time = 0;
		while (time < RECORD_TIME) {
			if (!Microphone.IsRecording (null)) { //如果没有录制
				Debug.Log ("IsRecording false");
				yield break;
			}
			Debug.Log ("yield return new WaitForSeconds " + time);
			yield return new WaitForSeconds (1);
			time++;
		}
		if (time >= 10) {
			Debug.Log ("RECORD_TIME is out! stop record!");
			StopRecord ();
		}
		yield return 0;
	}

	//-----------其他的代码----------------------------------------------------------

	public float Volume {
		get {
			if (Microphone.IsRecording (null)) {
				int sampleSize = 128;
				float[] samples = new float[sampleSize];
				int startPosition = Microphone.GetPosition (micArray [0]) - (sampleSize + 1);
				print (samples.ToString () + "---" + startPosition);
				gameObject.GetComponent<AudioSource> ().clip.GetData (samples, startPosition);//TODO,这个方法不灵的
				float levelMax = 0;
				for (int i = 0; i < sampleSize; ++i) {//这里是在全部采样中求峰值，上边的方法则是求平均值,如果是声控游戏也许峰值更适合
					float wavePeak = samples [i];
					if (levelMax < wavePeak)
						levelMax = wavePeak;
				}
				return levelMax * 100;
			}
			return 0;
		}
	}
}

