using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour
{
	public int queueBlockIndex; // from 0->2; thi co dung 3 thang de chon thui ma haha
	public int type;
	public int[,] array;
	public tk2dSprite[] listSprite;
	public GameObject spriteModel;
	public Vector3 originCenterBlockPos;
	public int w;
	public int h;
	public Bounds bounds;

	// cai nay de luu toa do tag to board cua block;
	public int xTag;
	public int yTag;
	public enum BLOCKSTATE
	{
		QUEUE,
		SELECT,
		INBOARD
	}
	public BLOCKSTATE blockState;
	public Game mainGameScript;
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void setType (int type, int queueBlockIndex)
	{
		this.type = type;
		this.queueBlockIndex = queueBlockIndex;
		gameObject.name = " block: " + queueBlockIndex;
		this.originCenterBlockPos = new Vector3 ((queueBlockIndex - 1) * Config.SCREEN_WIDTH / 4, -6f, 0);
		this.array = Config.GetArrayBlockFromType (type);
		this.w = array.GetLength (0);
		this.h = array.GetLength (1);
		blockState = BLOCKSTATE.QUEUE;
		if (listSprite != null && listSprite.Length > 0) {
			for (int i =0; i<listSprite.Length; i++) {
				if (listSprite [i] != null) {
					Destroy (listSprite [i].gameObject);
				}
			}
		}
		listSprite = new tk2dSprite[w * h];
		transform.localScale = Vector3.one;
		transform.position = originCenterBlockPos - new Vector3 (w * Config.CELL_SIZE / 2 * Config.CELL_SCALE_SMALL, h * Config.CELL_SIZE / 2 * Config.CELL_SCALE_SMALL, 0);
		this.bounds = new Bounds ();
		bounds.SetMinMax (originCenterBlockPos - new Vector3 ((w + 1) * Config.CELL_SIZE / 2 * Config.CELL_SCALE_SMALL, (h + 1) * Config.CELL_SIZE / 2 * Config.CELL_SCALE_SMALL, 0), originCenterBlockPos + new Vector3 ((w + 1) * Config.CELL_SIZE / 2 * Config.CELL_SCALE_SMALL, (h + 1) * Config.CELL_SIZE / 2 * Config.CELL_SCALE_SMALL, 0));

		for (int i =0; i< array.GetLength(0); i++) {
			for (int j =0; j<array.GetLength(1); j++) {
				GameObject go = Instantiate (spriteModel) as GameObject;
				tk2dSprite sprite = go.GetComponent<tk2dSprite> ();
				go.transform.parent = transform;

				if (sprite != null) {
					sprite.SetSprite ("1_" + type);
					listSprite [j * w + i] = sprite;
				}


			}
		}
		constructBlock ();
	}

	public void constructBlock ()
	{
		for (int i =0; i<listSprite.Length; i++) {
			if (listSprite [i] != null) {
				tk2dSprite sprite = listSprite [i];
				int x = i % w;
				int y = i / w;
				if (blockState == BLOCKSTATE.QUEUE) {
					sprite.transform.localScale = new Vector3 (Config.CELL_SCALE_SMALL, Config.CELL_SCALE_SMALL, 1);
					sprite.transform.localPosition = new Vector3 ((x + 0.5f) * Config.CELL_SCALE_SMALL * Config.CELL_SIZE, (y + 0.5f) * Config.CELL_SCALE_SMALL * Config.CELL_SIZE, 0);
				}
			}
		}
	}

	public void updateConstructBlock (BLOCKSTATE state)
	{

	}

	public Vector3 getLocalPositionOfCell (int x, int y)
	{
		return new Vector3 (x * Config.CELL_SIZE + 0.5f * Config.CELL_SIZE, y * Config.CELL_SIZE + 0.5f * Config.CELL_SIZE, 0);
	}

	public void SetSelection (Vector3 touchPos)
	{
		blockState = BLOCKSTATE.SELECT;
		StartCoroutine (UpdateSetSelection (touchPos));
	
		//transform.localScale = new Vector3 (Config.CELL_SCALE_SELECTION, Config.CELL_SCALE_SELECTION, 1);
	}

	IEnumerator UpdateSetSelection (Vector3 touchPos)
	{
		float d = 0.05f;
		float p = 0;
		float timeRun = 0.05f;
//		Debug.Log ("run update set selection");
		//
		Vector3 srcPos = transform.position;
		Vector3 destPos = touchPos;
		destPos.z = 0;
		destPos.y = (touchPos.y - originCenterBlockPos.y) * 1.3f + originCenterBlockPos.y;
		destPos.x = touchPos.x - w * Config.CELL_SIZE / 2;

		while (p<=1) {

			p += Time.deltaTime / timeRun;
			float scale1 = Mathf.Lerp (Config.CELL_SCALE_SMALL, Config.CELL_SCALE_SELECTION, p);
			float scale2 = Mathf.Lerp (Config.CELL_SCALE_SMALL, Config.CELL_SCALE_NORMAL, p);
//			Debug.Log ("update set selection: " + scale);
			for (int i =0; i<listSprite.Length; i++) {

				if (listSprite [i] != null) {

					tk2dSprite sprite = listSprite [i];
					int x = i % w;
					int y = i / w;
//					Debug.Log ("update sprite: " + x + ", " + y);

					sprite.transform.localScale = new Vector3 (scale1, scale1, 1);
					sprite.transform.localPosition = new Vector3 ((x + 0.5f) * scale2 * Config.CELL_SIZE, (y + 0.5f) * scale2 * Config.CELL_SIZE, 0);

				}
			}
			transform.position = Vector3.Lerp (srcPos, destPos, p);
			yield return null;
		}
//		Debug.Log ("finish update set selection");

	}

	public void SetSelectionPos (Vector3 touchPos)
	{
		//transform.position = touchPos - new Vector3(w*Config.CELL_SIZE/2,h*Config.CELL_SIZE/2,0);
		Vector3 pos = touchPos;
		pos.z = 0;
		pos.y = (touchPos.y - originCenterBlockPos.y) * 1.3f + originCenterBlockPos.y;
		pos.x = touchPos.x - w * Config.CELL_SIZE / 2;
		transform.position = pos;
	}

	public void SetUnSelection ()
	{
		//transform.localScale = new Vector3 (Config.CELL_SCALE_SMALL, Config.CELL_SCALE_SMALL, 1);
		blockState = BLOCKSTATE.QUEUE;
		StartCoroutine (UpdateUnSetSelection ());


	}

	IEnumerator UpdateUnSetSelection ()
	{
		float d = 0.05f;
		float p = 0;
		float timeRun = 0.05f;
		Vector3 currPos = transform.position;
		Vector3 destPos = originCenterBlockPos - new Vector3 (w * Config.CELL_SIZE / 2 * Config.CELL_SCALE_SMALL, h * Config.CELL_SIZE / 2 * Config.CELL_SCALE_SMALL, 0);
		//		Debug.Log ("run update set selection");
		while (p<=1) {
			
			p += Time.deltaTime / timeRun;
			float scale1 = Mathf.Lerp (Config.CELL_SCALE_SELECTION, Config.CELL_SCALE_SMALL, p);
			float scale2 = Mathf.Lerp (Config.CELL_SCALE_NORMAL, Config.CELL_SCALE_SMALL, p);
			//			Debug.Log ("update set selection: " + scale);
			for (int i =0; i<listSprite.Length; i++) {
				
				if (listSprite [i] != null) {
					
					tk2dSprite sprite = listSprite [i];
					int x = i % w;
					int y = i / w;
					//					Debug.Log ("update sprite: " + x + ", " + y);
					
					sprite.transform.localScale = new Vector3 (scale1, scale1, 1);
					sprite.transform.localPosition = new Vector3 ((x + 0.5f) * scale2 * Config.CELL_SIZE, (y + 0.5f) * scale2 * Config.CELL_SIZE, 0);
					
				}
			}
			transform.position = Vector3.Lerp (currPos, destPos, p);
			yield return null;
		}
		//		Debug.Log ("finish update set selection");
		
	}

	public bool CheckPointIn (Vector3 point)
	{
		point.z = 0;
		return bounds.Contains (point);
	}

	public void TagToPos(Vector3 pos)
	{
		StartCoroutine(UpdateTagPos(pos));
	}

	IEnumerator UpdateTagPos(Vector3 pos)
	{
		float d = 0.05f;
		float p = 0;
		float timeRun = 0.05f;
		Vector3 currPos = transform.position;
		Vector3 destPos = pos;

		while (p<=1) {
			
			p += Time.deltaTime / timeRun;
			float scale1 = Mathf.Lerp (Config.CELL_SCALE_SELECTION, Config.CELL_SCALE_NORMAL, p);


			for (int i =0; i<listSprite.Length; i++) {
				
				if (listSprite [i] != null) {
					
					tk2dSprite sprite = listSprite [i];
					int x = i % w;
					int y = i / w;

					sprite.transform.localScale = new Vector3 (scale1, scale1, 1);

				}
			}
			transform.position = Vector3.Lerp (currPos, destPos, p);
			yield return null;
		}

		mainGameScript.FinishTagPos();

	}

}

