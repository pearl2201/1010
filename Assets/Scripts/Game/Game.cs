using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
	[HideInInspector]
	public List<Block>
		listBlockQueue;
	public GameObject blockModel;
	public Transform parentsBlockQueue;
	private Vector3 botleftBoardPos = new Vector3 (-4.15f, -4.076f, 0f);
	private Vector3 toprightBoardPos = new Vector3 (4.221f, 4.013f, 0f);
	public int[,] board = new int[10, 10];
	public tk2dSprite[,] spriteBoard = new tk2dSprite[10, 10];
	public Transform parentsBoard;
	public GameObject cellMode;
	// Use this for initialization
	void Start ()
	{
		InitSpriteBoard ();
		ResetBoard ();
		createNewBlockQueue ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void createNewBlockQueue ()
	{
		for (int i =0; i<3; i++) {
			GameObject go = Instantiate (blockModel) as GameObject;
			go.transform.parent = parentsBlockQueue;
			Block blockScript = go.GetComponent<Block> ();
			if (blockScript != null) {
				int type = Random.Range (1, 11);

				blockScript.mainGameScript = this;
				blockScript.setType (type, i);
				listBlockQueue.Add (blockScript);
			}

		}
	}

	public Block currSelectionBlock;

	public void OnTouchDown ()
	{
	

		if (listBlockQueue.Count > 0 && currSelectionBlock == null) {
			Vector3 pointClick = GetTouchPos ();
			Debug.Log ("pointClick: " + pointClick);
			for (int i =0; i<listBlockQueue.Count; i++) {
				if (listBlockQueue [i].CheckPointIn (pointClick)) {
					currSelectionBlock = listBlockQueue [i];
					currSelectionBlock.SetSelection (GetTouchPos ());

					StartCoroutine (UpdateBlockSelection (currSelectionBlock));
					break;
				}
			}
		}
	}

	IEnumerator UpdateBlockSelection (Block block)
	{
		yield return new WaitForSeconds (0.05f);
		while (currSelectionBlock!=null) {
			block.SetSelectionPos (GetTouchPos ());
			yield return null;
		}
	}

	public void OnTouchUp ()
	{
		if (currSelectionBlock != null) {
			StopCoroutine ("UpdateBlockSelection");
			currSelectionBlock.SetSelectionPos (GetTouchPos ());
			Vector3 pos = currSelectionBlock.transform.position;
			Debug.Log ("release: " + pos.ToString ());
			if ((pos.x + Config.CELL_SIZE / 2 < toprightBoardPos.x) && (pos.x + Config.CELL_SIZE / 2 > botleftBoardPos.x) && (pos.y + Config.CELL_SIZE / 2 < toprightBoardPos.y) && (pos.y + Config.CELL_SIZE / 2 > botleftBoardPos.y)) {

				Debug.Log ("" + ((pos.x - botleftBoardPos.x + Config.CELL_SIZE / 2) / Config.CELL_SIZE));
				int xTag = (int)((pos.x - botleftBoardPos.x + Config.CELL_SIZE / 2) / Config.CELL_SIZE);
				int yTag = (int)((pos.y - botleftBoardPos.y + Config.CELL_SIZE / 2) / Config.CELL_SIZE);
				if (xTag > 10)
					xTag = 10;
				if (yTag > 10)
					yTag = 10;
				Debug.Log ("pos Tag: " + xTag + ", " + yTag);
				bool isTag = true;
				if (xTag+currSelectionBlock.w>10 || yTag+currSelectionBlock.h>10)
				{
					isTag = false;
				}else
				{
				for (int i = xTag; i<xTag+currSelectionBlock.w && i<10; i++) {
					for (int j =yTag; j<yTag+currSelectionBlock.h && j<10; j++) {
						if (currSelectionBlock.array [i - xTag, j - yTag] != 0 && board [i, j] != -1) {
							isTag = false;
						}
					}
				}
				}

				if (!isTag) {
					currSelectionBlock.SetUnSelection ();
					currSelectionBlock = null;
				} else {
					Vector3 pos1 = new Vector3 (botleftBoardPos.x + Config.CELL_SIZE * (xTag + 0.5f), botleftBoardPos.y + Config.CELL_SIZE * (yTag + 0.5f), currSelectionBlock.transform.position.z);
					currSelectionBlock.TagToPos (pos1);
					currSelectionBlock.xTag = xTag;
					currSelectionBlock.yTag = yTag;
				}
			} else {
				currSelectionBlock.SetUnSelection ();
				currSelectionBlock = null;
			}



		}
	}

	public void FinishTagPos ()
	{
		if (currSelectionBlock != null) {
			int xTag = currSelectionBlock.xTag;
			int yTag = currSelectionBlock.yTag;

			for (int i = xTag; i<xTag+currSelectionBlock.w; i++) {
				for (int j =yTag; j<yTag+currSelectionBlock.h; j++) {
					spriteBoard [i, j].SetSprite ("1_" + currSelectionBlock.type);
					spriteBoard [i, j].gameObject.SetActive (true);
					board [i, j] = currSelectionBlock.type;
				}
			}
		}

		for (int i =0; i<currSelectionBlock.listSprite.Length; i++) {
			if (currSelectionBlock.listSprite [i] != null) {
				Destroy (currSelectionBlock.listSprite [i].gameObject);
			}
		}
		Destroy (currSelectionBlock.gameObject);
		listBlockQueue.Remove (currSelectionBlock);
		currSelectionBlock = null;
		List<int> listRow = new List<int> ();
		List<int> listCol = new List<int> ();

		for (int i =0; i<10; i++) {
			bool addCol = true;
			for (int j =0; j<10; j++) {
				if (board [i, j] == -1) {
					addCol = false;
				}
			}

			if (addCol) {
				for (int j =0; j<10; j++) {
					board [i, j] = -1;

				}
				listCol.Add (i);
			}
		}

		for (int j =0; j<10; j++) {
			bool addRow = true;
			for (int i =0; i<10; i++) {
				if (board [i, j] == -1) {
					addRow = false;
				}
			}

			if (addRow) {
				for (int i =0; i<10; i++) {
					board [i, j] = -1;

				}
				listRow.Add (j);
			}
		}
		StartCoroutine (DestroyRowCol (listRow, listCol));
	}

	IEnumerator DestroyRowCol (List<int> listRow, List<int> listCol)
	{
		List<tk2dSprite> listAct = new List<tk2dSprite> ();

		for (int j =0; j<listRow.Count; j++) {
			for (int i =0; i<10; i++) {
				if (!listAct.Contains (spriteBoard [i, listRow [j]])) {
					listAct.Add (spriteBoard [i, listRow [j]]);
				}
			}
		}

		for (int i =0; i<listCol.Count; i++) {
			for (int j =0; j<10; j++) {
				if (!listAct.Contains (spriteBoard [listCol [i], j])) {
					listAct.Add (spriteBoard [listCol [i], j]);
				}
			}
		}

		float p = 0;
		float timeRun = 0.5f;
		if (listAct.Count > 0) {
			Vector3 scale = Vector3.one;
			while (p<=1) {
				p += Time.deltaTime / timeRun;
				scale.x = 1 - p;
				scale.y = 1 - p;
				for (int i =0; i<listAct.Count; i++) {

					listAct [i].transform.localScale = scale;
				}
				yield return null;
			}
			for (int i =0; i<listAct.Count; i++) {
				
				listAct [i].transform.localScale = Vector3.one;
				listAct [i].SetSprite ("square");
				listAct [i].gameObject.SetActive (false);
			}
	
		}
		ReCreateNewListBlockQueue ();
	}

	public void ReCreateNewListBlockQueue ()
	{
		if (listBlockQueue.Count <= 0) {
			createNewBlockQueue ();
			StartCoroutine (MoveBlockQueueToScreen ());
		} else {
			CheckEndGame ();
		}

	}

	public void CheckEndGame ()
	{
		bool endGame = true;
		foreach (Block block in listBlockQueue) {

			for (int xTag =0; xTag<10; xTag++) {
				for (int yTag = 0; yTag<10; yTag++) {
		

					bool isTag = true;
					if (xTag+block.w>10 || yTag+block.h>10)
					{
						isTag
							 = false;
					}else
					{
					for (int i = xTag; i<xTag+block.w && i<10; i++) {
						for (int j =yTag; j<yTag+block.h && j<10; j++) {

							if (block.array [i - xTag, j - yTag] != 0 && board [i, j] != -1) {
								isTag = false;
							}
						}
					}
					}
				
					if (isTag) {
						endGame = false;
					}
				
				}
			}
		}
		if (endGame) {
			Application.LoadLevel (Application.loadedLevel);
		}
	}

	IEnumerator MoveBlockQueueToScreen ()
	{
		Vector3 destPos = parentsBlockQueue.transform.position;
		Vector3 srcPos = destPos + new Vector3 (4.8f, 0, 0);

		float p = 0;
		float timeMove = 0.5f;
		while (p<=1) {
			p += Time.deltaTime / timeMove;
			parentsBlockQueue.transform.position = Vector3.Lerp (srcPos, destPos, p);
			yield return null;
		}
		CheckEndGame ();
	}

	public Vector3 GetTouchPos ()
	{
		Vector3 pointClick = Vector3.zero;
		{
			if (Application.isMobilePlatform) {
				pointClick = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position);
			} else {
				pointClick = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				
			}
		}
		return pointClick;
	}

	public void InitSpriteBoard ()
	{
		for (int i =0; i<spriteBoard.GetLength(0); i++) {
			for (int j =0; j<spriteBoard.GetLength(1); j++) {
				if (spriteBoard [i, j] == null) {
					GameObject go = Instantiate (cellMode) as GameObject;
					go.name = "nen_cot: " + i + ",dong: " + j;
					go.transform.parent = parentsBoard;
					go.transform.position = new Vector3 (botleftBoardPos.x + Config.CELL_SIZE * (i + 0.5f), botleftBoardPos.y + Config.CELL_SIZE * (j + 0.5f), 2);

					tk2dSprite sprite = go.GetComponent<tk2dSprite> ();
					if (sprite != null) {

						sprite.SetSprite ("square");
					}
				}
			}
		}

		for (int i =0; i<spriteBoard.GetLength(0); i++) {
			for (int j =0; j<spriteBoard.GetLength(1); j++) {

				GameObject go = Instantiate (cellMode) as GameObject;
				go.name = "o_cot: " + i + ",dong: " + j;
				go.transform.parent = parentsBoard;
				go.transform.position = new Vector3 (botleftBoardPos.x + Config.CELL_SIZE * (i + 0.5f), botleftBoardPos.y + Config.CELL_SIZE * (j + 0.5f), 1);
					
				tk2dSprite sprite = go.GetComponent<tk2dSprite> ();
				if (sprite != null) {
					spriteBoard [i, j] = sprite;
					sprite.SetSprite ("square");
				}
				go.SetActive (false);
			}

		}
	}

	public void ResetBoard ()
	{
		// clear board

		// new board;

		for (int i =0; i<board.GetLength(0); i++) {
			for (int j =0; j<board.GetLength(1); j++) {
				board [i, j] = -1;
				spriteBoard [i, j].SetSprite ("square");
			}
		}
	}
}

