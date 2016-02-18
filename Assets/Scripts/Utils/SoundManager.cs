using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
	private static SoundManager instance;
	
	public static SoundManager Instance {
		get {
			return instance;
		}
	}
	
	void Awake ()
	{
		if (instance != null) {
			Destroy (gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad (this.gameObject);
		}
		
	}
	
	public AudioSource audioSource;
	
	void playSound(string filename){
		if (Config.isSoundOn)
			return;
		audioSource.mute = false;
		//		if (!audioSource.isPlaying) {
		AudioClip clip = Resources.Load ("Sound/"+filename) as AudioClip;
		audioSource.volume = 1;
		audioSource.PlayOneShot(clip);
		//		}
	}
	bool isMainSound = false;
	//ham goi khi choi thang
	public void MainSound ()
	{
		if (Config.isSoundOn)
			return;
		audioSource.mute = false;
		if (isMainSound)
			return;
		audioSource.clip = Resources.Load ("Sound/maintheme") as AudioClip;
		audioSource.volume = 1;
		audioSource.loop = true;
		audioSource.Play ();
		//		audio.Stop ();
		isMainSound = true;
	}
	public void setVolume(float vol){
		audioSource.volume = vol;
	}
	public void NotMainSound ()
	{
		audioSource.Stop ();
		isMainSound = false;
	
	}

	public void stopSound ()
	{
		audioSource.Stop ();
	}
	
	public void stopMusic ()
	{
		audioSource.mute = true;
	}
	

}
