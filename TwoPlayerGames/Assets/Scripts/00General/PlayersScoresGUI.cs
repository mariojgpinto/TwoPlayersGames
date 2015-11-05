using UnityEngine;
using UnityEngine.UI;

public class PlayersScoresGUI : MonoBehaviour {
	#region VARIABLES
	float[] playersScore = new float[]{0,0};

	public Text[] textScores = new Text[2];
	public Text[] playerLabel_Text = new Text[2];
	
	public GameObject endGame_Panel;
	public Text endGame_Text;

	public Button buttonRetry;
	public Button buttonExit;

	public enum ScoreType{
		INT,
		FLOAT,
		STRING
	};

	public int turn = 1;

	public ScoreType typeScore = ScoreType.INT;
	#endregion

	#region SCORE_MANAGEMENT
	public void AddScore(int ammount){
		playersScore[turn] += ammount;
		UpdatePlayersGUI();
	}

	public void AddScoreToPlayer1(int ammount){
		playersScore[0] += ammount;
		UpdatePlayersGUI();
	}

	public void AddScoreToPlayer2(int ammount){
		playersScore[1] += ammount;
		UpdatePlayersGUI();
	}

	public void SetScoreToPlayer1(int ammount){
		playersScore[0] = ammount;
		UpdatePlayersGUI();
	}
	
	public void SetScoreToPlayer2(int ammount){
		playersScore[1] = ammount;
		UpdatePlayersGUI();
	}

	public void ResetScores(){
		playersScore[0] = 0;
		playersScore[1] = 0;
		UpdatePlayersGUI();
	}
	#endregion

	#region ACCESS
	public float GetPlayerScore(int player){
		return playersScore[player];
	}

	public float GetPlayer1Score(){
		return playersScore[0];
	}

	public float GetPlayer2Score(){
		return playersScore[1];
	}
	#endregion

	#region GUI_MANAGEMENT
	public void ChangePlayersTurn(){
		playerLabel_Text[turn].fontStyle = FontStyle.Normal;
		textScores[turn].fontStyle = FontStyle.Normal;
		
		turn = Mathf.Abs(turn-1);
		
		playerLabel_Text[turn].fontStyle = FontStyle.Bold;
		textScores[turn].fontStyle = FontStyle.Bold;
	}

	public void EndGame(string text = ""){
		int score1 = (int)playersScore[0];
		int score2 = (int)playersScore[1];
		
		if(score1 > score2){
			endGame_Text.text = "Player 1 Wins!";
		} 
		else
		if(score1 < score2){
			endGame_Text.text = "Player 2 Wins!";
		}else{
			endGame_Text.text = "It's a Tie!";
		}

		if(text != ""){
			endGame_Text.text = text + "\n" + endGame_Text.text;
		}

		endGame_Panel.SetActive(true);
	}

	public void HideEndPanel(){
		endGame_Panel.SetActive(false);
	}


	#endregion

	#region GUI_UPDATE
	public void UpdatePlayersGUI(){
		switch(typeScore){
		case ScoreType.INT:
			textScores[0].text = "" + (int)playersScore[0];
			textScores[1].text = "" +  (int)playersScore[1];
			break;
		case ScoreType.FLOAT:
			textScores[0].text = "" +  playersScore[0].ToString("0.00");
			textScores[1].text = "" +  playersScore[1].ToString("0.00");
			break;		
		case ScoreType.STRING:		
		default:
			textScores[0].text = "" +  playersScore[0];
			textScores[1].text = "" +  playersScore[1];
			break;
		}

	}
	#endregion
}
