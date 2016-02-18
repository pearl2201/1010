using UnityEngine;
using System.Collections;

public class MyPopup : MonoBehaviour
{
	private Vector3 startPos;

	void Awake()
	{
		startPos = transform.position;
	}
	// Use this for initialization
	void Start ()
	{
	
	}


	void OnEnable()
	{
		iTween.MoveFrom(gameObject,iTween.Hash("y",-10,"time",0.3f,"islocal",true,"easetype",iTween.EaseType.easeInBounce));
	}
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void Close()
	{
		iTween.MoveTo(gameObject,iTween.Hash("y",-10,"time",0.3f,"islocal",true,"easetype",iTween.EaseType.easeInBounce, "oncomplete","Hide","oncompletetarget", gameObject));

	}


	public void Hide()
	{
		transform.position = startPos;
		gameObject.SetActive(false);
	}
}

