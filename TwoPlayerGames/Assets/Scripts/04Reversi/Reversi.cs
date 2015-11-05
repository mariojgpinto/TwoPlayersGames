using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Reversi : MonoBehaviour {
	#region VARIABLES
	public GameObject board;

	string prefabPath = "Prefabs/04Reversi/Piece";

	const float boardMargin = .05f;
	const int boardSize = 8;
	float minX;
	float deltaX;
	float minY;
	float deltaY;

	const float pieceRatio = .75f;
	Vector3[,] positions = new Vector3[boardSize,boardSize];
	ReversiPiece[,] pieces = new ReversiPiece[boardSize,boardSize];

	bool isBusy = false;

	PlayersScoresGUI score;

	#endregion

	#region SETUP
	void AdjustBoardSize(){
		float widthMargin = Screen.width * boardMargin;
//		float heightMargin = Screen.height * boardMargin;

		Camera cam = GameObject.Find("Main Camera").GetComponent<Camera>();

		float minWidth = cam.ScreenToWorldPoint(new Vector3(widthMargin, 0,10)).x;
		float maxWidth = cam.ScreenToWorldPoint(new Vector3(Screen.width - widthMargin, 0,10)).x;

		board.transform.localScale = Vector3.one * (maxWidth - minWidth);

		cam.transform.position = cam.transform.position + Vector3.up*.5f;

		Vector3 min = board.GetComponent<Renderer>().bounds.min;
		Vector3 max = board.GetComponent<Renderer>().bounds.max;

		minX = cam.WorldToScreenPoint(min).x;
		minY = cam.WorldToScreenPoint(min).y;
		float maxX = cam.WorldToScreenPoint(max).x;
		float maxY = cam.WorldToScreenPoint(max).y;

		deltaX = (maxX - minX) / boardSize;
		deltaY = (maxY - minY) / boardSize;

		for(int i = 0 ; i < boardSize ; ++i){
			for(int j = 0 ; j < boardSize ; ++j){
				positions[i,j] = cam.ScreenToWorldPoint(new Vector3(minX + (i*deltaX) + deltaX/2,minY + (j*deltaY) + deltaY/2,10));
				positions[i,j].z = -.05f;
			}
		}
	}

	void CreatePieces(){
		Vector3 diff = positions[1,1] - positions[0,0];
		diff *= pieceRatio;
		diff.z = 1;

		GameObject parent = new GameObject();
		parent.name = "Pieces";

		for(int i = 0 ; i < boardSize ; ++i){
			for(int j = 0 ; j < boardSize ; ++j){
				GameObject go = Instantiate(Resources.Load(prefabPath) as GameObject);
				go.transform.position = positions[i,j];
				go.transform.localScale = diff;
				go.transform.parent = parent.transform;
				pieces[i,j] = go.GetComponent<ReversiPiece>();

				pieces[i,j].Init(go.transform.localScale, positions[i,j]);
			}
		}
	}

	#endregion

	#region GAME_LOGIC
	void ProcessTouch(float touchX, float touchY)
	{
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(touchX, touchY, 0));
		RaycastHit hit;
		
		if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.name.StartsWith("Board"))
		{		
//			isBusy = true;

			int x = Mathf.FloorToInt((touchX - minX) / deltaX);
			int y = Mathf.FloorToInt((touchY - minY) / deltaY);

			GameObject.Find("Text_Log").GetComponent<Text>().text = "Touch At: " + x + " , " + y;

			ReversiPiece piece = pieces[x, y];

//			if(!piece.isActive){
//				if(flag){
//					piece.SetWhite();
//				}
//				else{
//					piece.SetBlack();
//				}
//				flag = !flag;
//				piece.AddPiece();
//			}
//			else{
//				piece.FlipPiece();
//			}

			if(!piece.isActive){
				bool isWhite = score.turn == 0;

				if(CheckValidMove(x, y, isWhite)){
					GameObject.Find("Text_Log").GetComponent<Text>().text = isWhite + "Move OK";
					if(isWhite)
						piece.SetWhite();
					else
						piece.SetBlack();

					piece.AddPiece();

					AffectNeighbors(x, y, isWhite);

					UpdateScore();

					score.ChangePlayersTurn();

					if(CheckIfItsOver()){
						//GAME IS OVER
						score.EndGame("Game Over");
					}
				}
				else{
					//MOVE NOT POSSIBLE
					Debug.Log("MOVE NOT POSSIBLE");
					piece.ErrorPiece(score.turn == 0);
					GameObject.Find("Text_Log").GetComponent<Text>().text = isWhite + "Invalid Move";
				}
			}
			else{
				piece.JumpPiece();
			}
		}
	}

	void ResetGame(){
		for(int i = 0 ; i < boardSize ; ++i){
			for(int j = 0 ; j < boardSize ; ++j){
				pieces[i,j].HidePiece();
			}
		}

		pieces[3,3].SetWhite();
		pieces[3,4].SetBlack();
		pieces[4,3].SetBlack();
		pieces[4,4].SetWhite();

		pieces[3,3].AddPiece();
		pieces[3,4].AddPiece();
		pieces[4,3].AddPiece();
		pieces[4,4].AddPiece();

		UpdateScore();
	}

	bool CheckValidMove(int x, int y, bool white){
		int xp1 = x+1;
		int xm1 = x-1;
		int yp1 = y+1;
		int ym1 = y-1;

		//UP
		if(xp1 < boardSize){
			if(pieces[xp1,y].isActive){
				if(pieces[xp1,y].isWhite != white){
					if(CheckValidMoveLine(xp1,y,white,1)) 
						return true;
				}
			}
		}

		//UP RIGHT
		if(xp1 < boardSize && yp1 < boardSize){
			if(pieces[xp1,yp1].isActive){
				if(pieces[xp1,yp1].isWhite != white){
					if(CheckValidMoveLine(xp1,yp1,white,2)) 
						return true;
				}
			}
		}

		//RIGHT
		if(yp1 < boardSize){
			if(pieces[x,yp1].isActive){
				if(pieces[x,yp1].isWhite != white){
					if(CheckValidMoveLine(x,yp1,white,3)) 
						return true;
				}
			}
		}

		//DOWN RIGHT
		if(xm1 >= 0 && yp1 < boardSize){
			if(pieces[xm1,yp1].isActive){
				if(pieces[xm1,yp1].isWhite != white){
					if(CheckValidMoveLine(xm1,yp1,white,4)) 
						return true;
				}
			}
		}

		//DOWN
		if(xm1 >= 0){
			if(pieces[xm1,y].isActive){
				if(pieces[xm1,y].isWhite != white){
					if(CheckValidMoveLine(xm1,y,white,5)) 
						return true;
				}
			}
		}

		//DOWN LEFT
		if(xm1 >= 0 && ym1 >= 0){
			if(pieces[xm1,ym1].isActive){
				if(pieces[xm1,ym1].isWhite != white){
					if(CheckValidMoveLine(xm1,ym1,white,6)) 
						return true;
				}
			}
		}

		//LEFT
		if(ym1 >= 0){
			if(pieces[x,ym1].isActive){
				if(pieces[x,ym1].isWhite != white){
					if(CheckValidMoveLine(x,ym1,white,7)) 
						return true;
				}
			}
		}

		//UP LEFT
		if(xp1 < boardSize && ym1 >= 0){
			if(pieces[xp1,ym1].isActive){
				if(pieces[xp1,ym1].isWhite != white){
					if(CheckValidMoveLine(xp1,ym1,white,8)) 
						return true;
				}
			}
		}

		return false;
	}

	bool CheckValidMoveLine(int x, int y, bool white, int direction){
		int newX = x;
		int newY = y;

		switch(direction){
		case 1: //UP
			newX = x+1;
			break;
		case 2: //UP RIGHT
			newX = x+1;
			newY = y+1;
			break;
		case 3: //RIGHT
			newY = y+1;
			break;
		case 4: //DOWN RIGHT
			newX = x-1;
			newY = y+1;
			break;
		case 5: //DOWN
			newX = x-1;
			break;
		case 6: //DOWN LEFT
			newX = x-1;
			newY = y-1;
			break;
		case 7: //LEFT
			newY = y-1;
			break;
		case 8: //UP LEFT
			newX = x+1;
			newY = y-1;
			break;
		default: break;
		}
				
		if(newX < boardSize && newX >= 0 && newY < boardSize && newY >= 0 ){
			if(pieces[newX,newY].isActive){
				if(pieces[newX,newY].isWhite == white){
					return true;
				}
				else{
					return CheckValidMoveLine(newX, newY, white, direction);
				}
			}
			else{
				return false;
			}
		}
		else{
			return false;
		}
	}

	void AffectNeighbors(int x, int y, bool white){
		if(CheckValidMoveLine(x,y,white,1))
			AffectNeighborsRecursive(x+1,y  ,white, 1);
		if(CheckValidMoveLine(x,y,white,2))
			AffectNeighborsRecursive(x+1,y+1,white, 2);
		if(CheckValidMoveLine(x,y,white,3))
			AffectNeighborsRecursive(x  ,y+1,white, 3);
		if(CheckValidMoveLine(x,y,white,4))
			AffectNeighborsRecursive(x-1,y+1,white, 4);
		if(CheckValidMoveLine(x,y,white,5))
			AffectNeighborsRecursive(x-1,y  ,white, 5);
		if(CheckValidMoveLine(x,y,white,6))
			AffectNeighborsRecursive(x-1,y-1,white, 6);
		if(CheckValidMoveLine(x,y,white,7))
			AffectNeighborsRecursive(x  ,y-1,white, 7);
		if(CheckValidMoveLine(x,y,white,8))
			AffectNeighborsRecursive(x+1,y-1,white, 8);
	}

	void AffectNeighborsRecursive(int x, int y, bool white, int direction){
		if(x < boardSize && x >= 0 && y < boardSize && y >= 0){
			if(pieces[x,y].isActive){
				if(pieces[x,y].isWhite != white){
					pieces[x,y].FlipPiece();

					int newX = x;
					int newY = y;
					
					switch(direction){
					case 1: //UP
						newX = x+1;
						break;
					case 2: //UP RIGHT
						newX = x+1;
						newY = y+1;
						break;
					case 3: //RIGHT
						newY = y+1;
						break;
					case 4: //DOWN RIGHT
						newX = x-1;
						newY = y+1;
						break;
					case 5: //DOWN
						newX = x-1;
						break;
					case 6: //DOWN LEFT
						newX = x-1;
						newY = y-1;
						break;
					case 7: //LEFT
						newY = y-1;
						break;
					case 8: //UP LEFT
						newX = x+1;
						newY = y-1;
						break;
					default: break;
					}


					AffectNeighborsRecursive(newX, newY, white, direction);
				}
			}
		}
	}

	void UpdateScore(){
		int whites = 0;
		int blacks = 0;
		for(int i = 0 ; i < boardSize ; ++i){
			for(int j = 0 ; j < boardSize ; ++j){
				if(pieces[i,j].isActive){
					if(pieces[i,j].isWhite){
						whites++; 
//						Debug.Log("White(" + i + "," + j + ") - " + whites);
					}
					else{
						blacks++;
//						Debug.Log("Black(" + i + "," + j + ") - " + blacks);
//						pieces[i,j].JumpPiece();
					}
				}
			}
		}

		score.SetScoreToPlayer1(whites);
		score.SetScoreToPlayer2(blacks);
	}

	bool CheckIfItsOver(){
		bool isWhite = score.turn == 0;

		for(int i = 0 ; i < boardSize ; ++i){
			for(int j = 0 ; j < boardSize ; ++j){
				if(!pieces[i,j].isActive){
					if(CheckValidMove(i,j,isWhite)){
						return false;
					}
				}
			}
		}

		return true;
	}
	#endregion

	#region BUTTON_CALLBACKS
	public void OnButtonPressed(int id){
		switch(id){
		case 1:
			//RETRY
			ResetGame();
			score.HideEndPanel();
			break;
		case 2:
			Application.LoadLevel("00-MainScene");
			break;
		default: break;
		}
	}
	
	#endregion

	#region UNITY_CALLBACKS
	// Use this for initialization
	void Start () {		
		score = GameObject.Find("CanvasScoreGUI").GetComponent<PlayersScoresGUI>();
		score.buttonRetry.onClick.AddListener(() => OnButtonPressed(1));
		score.buttonExit.onClick.AddListener(() => OnButtonPressed(2));
		score.HideEndPanel();
		score.turn = 1;
		score.ChangePlayersTurn();


		AdjustBoardSize();

		CreatePieces();

		ResetGame();
	}
	
	// Update is called once per frame
	void Update () {
		#if UNITY_EDITOR
		if (Input.GetMouseButtonDown(0))
		{
			if(!isBusy){
				ProcessTouch(Input.mousePosition.x, Input.mousePosition.y);
			}            
		}
		#else
		//if (Input.touchCount > 0/* && Input.touches[0].phase == TouchPhase.Began*/)
		if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
		{
			if(!isBusy){
				ProcessTouch(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
			}
		}
		#endif
	}

	void OnGUI(){
		if(Input.GetKeyDown(KeyCode.Escape)){
			Application.LoadLevel("00-Menu");
		}
	}
	#endregion
}
