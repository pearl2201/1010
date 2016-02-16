using UnityEngine;
using System.Collections;

public class Config  {

	public static float CELL_SIZE = 0.82f;
	public static float CELL_SCALE_SMALL = 0.5f;
	public static float CELL_SCALE_SELECTION = 0.9f;
	public static float CELL_SCALE_NORMAL = 1f;

	public static float SCREEN_HEIGHT = 16f ;
	public static float SCREEN_WIDTH = 9.6f;

	public static Vector2 ConvertCellPositionWorldToBoard(Vector3 positionW)
	{
		return new Vector2(0,0);
	}

	public static Vector3 ConvertCellPositionBoardToWorld(Vector2 positionB)
	{
		return new Vector3(0,0,0);
	}
	public static int TYPE_BLOCK_O_VUONG_1X1=0;
	public static int TYPE_BLOCK_2_O_DOC = 1;
	public static int TYPE_BLOCK_2_O_NGANG = 2;
	public static int TYPE_BLOCK_3_O_DOC = 3;
	public static int TYPE_BLOCK_3_O_NGANG = 4;
	public static int TYPE_BLOCK_4_O_DOC = 5;
	public static int TYPE_BLOCK_4_O_NGANG = 6;
	public static int TYPE_BLOCK_5_O_DOC = 7;
	public static int TYPE_BLOCK_5_O_NGANG = 8;
	public static int TYPE_BLOCK_O_VUONG_2X2 =9;
	public static int TYPE_BLOCK_O_VUONG_3X3 = 10;

	public static int[,] ARRAY_BLOCK_O_VUONG_1X1= new int[1,1]{{1}};
	public static int[,] ARRAY_BLOCK_2_O_DOC = new int[1,2]{{1,1}};
	public static int[,] ARRAY_BLOCK_2_O_NGANG = new int[2,1]{{1},{1}};
	public static int[,] ARRAY_BLOCK_3_O_DOC = new int[1,3]{{1,1,1}};
	public static int[,] ARRAY_BLOCK_3_O_NGANG = new int[3,1]{{1},{1},{1}};
	public static int[,] ARRAY_BLOCK_4_O_DOC = new int[1,4]{{1,1,1,1}};
	public static int[,] ARRAY_BLOCK_4_O_NGANG = new int[4,1]{{1},{1},{1},{1}};
	public static int[,] ARRAY_BLOCK_5_O_DOC = new int[1,5]{{1,1,1,1,1}};
	public static int[,] ARRAY_BLOCK_5_O_NGANG = new int[5,1]{{1},{1},{1},{1},{1}};
	public static int[,] ARRAY_BLOCK_O_VUONG_2X2 = new int[2,2]{{1,1},{1,1}};
	public static int[,] ARRAY_BLOCK_O_VUONG_3X3 = new int[3,3]{{1,1,1},{1,1,1},{1,1,1}};

	public static int[,] GetArrayBlockFromType(int typeB)
	{
		if (typeB == TYPE_BLOCK_O_VUONG_1X1) {
			return ARRAY_BLOCK_O_VUONG_1X1;
		} else if (typeB == TYPE_BLOCK_2_O_DOC) {
			return ARRAY_BLOCK_2_O_DOC;
		} else if (typeB == TYPE_BLOCK_2_O_NGANG) {
			return ARRAY_BLOCK_2_O_NGANG;
		} else if (typeB == TYPE_BLOCK_3_O_DOC) {
			return ARRAY_BLOCK_3_O_DOC;
		} else if (typeB == TYPE_BLOCK_3_O_NGANG) {
			return ARRAY_BLOCK_3_O_NGANG;
		} else if (typeB == TYPE_BLOCK_4_O_DOC) {
			return ARRAY_BLOCK_4_O_DOC;
		} else if (typeB == TYPE_BLOCK_4_O_NGANG) {
			return ARRAY_BLOCK_5_O_NGANG;
		} else if (typeB == TYPE_BLOCK_5_O_DOC) {
			return ARRAY_BLOCK_5_O_DOC;
		} else if (typeB == TYPE_BLOCK_5_O_NGANG) {
			return ARRAY_BLOCK_5_O_NGANG;
		} else if (typeB == TYPE_BLOCK_O_VUONG_2X2) {
			return ARRAY_BLOCK_O_VUONG_2X2;
		} else if (typeB == TYPE_BLOCK_O_VUONG_3X3) {
			return ARRAY_BLOCK_O_VUONG_3X3;
		} 
		return null;
	}

}
