using UnityEngine;
using System.Collections;

public class Prefs
{

	private static Prefs _instance;
	private static string HIGH_SCORE = "high_score";
//	private static string MUSIC_ON = "music_on";
	private static string SOUND_ON = "sound_on";
	private static string ADS = "ads";


	public static Prefs Instance {
		get {
			if (_instance == null) {
				_instance = new Prefs ();
				
			}
			return _instance;
		}
		
		
	}
	
	public Prefs ()
	{
		if (PlayerPrefs.HasKey(HIGH_SCORE))
		{
			Config.isSoundOn = GetSoundOn();
//			Config.isMusicOn = GetMusicOn();
		}else
		{
			SetInt(HIGH_SCORE,0);
			SetSoundOn(true);
			Config.isSoundOn = true;
			SetBuyAds(false);

		}
		
	}

	public int GetHighScore()
	{
		return GetInt(HIGH_SCORE);
	}

	public void SetHighScore(int score)
	{
		SetInt(HIGH_SCORE,score);
	}

	public void SetSoundOn(bool on)
	{
		SaveBool(SOUND_ON,on);
	}

	public bool GetSoundOn()
	{
		return GetBool(SOUND_ON);
	}

	public bool IsBuyAds()
	{
		return GetBool(ADS);
	}

	public void SetBuyAds(bool buy)
	{
		SaveBool(ADS,buy);
	}
	///////////////////////////////////////////
	
	private int GetInt (string name)
	{
		if (PlayerPrefs.HasKey (name)) {
			return PlayerPrefs.GetInt (name);
		} else
			return 0;
	}
	
	private void SetInt (string name, int value)
	{
		PlayerPrefs.SetInt (name, value);
		PlayerPrefs.Save ();
	}
	
	private bool GetBool (string name)
	{
		if (PlayerPrefs.HasKey (name)) {
			if (PlayerPrefs.GetInt (name) == 1)
				return true;
			else
				return false;
		} else
			return false;
	}
	
	private void SaveBool (string name, bool value)
	{
		if (value)
			PlayerPrefs.SetInt (name, 1);
		else
			PlayerPrefs.SetInt (name, 0);
		PlayerPrefs.Save ();
	}
	
	private void SaveString (string name, string value)
	{
		PlayerPrefs.SetString (name, value);
		PlayerPrefs.Save ();
	}
	
	private string GetString (string name)
	{
		return PlayerPrefs.GetString (name);
	}
	
	private void SaveFloat (string name, float value)
	{
		PlayerPrefs.SetFloat (name, value);
		PlayerPrefs.Save ();
	}
	
	private float GetFloat (string name)
	{
		if (PlayerPrefs.HasKey (name)) {
			return PlayerPrefs.GetFloat (name);
		} else
			return 0f;
	}
}

