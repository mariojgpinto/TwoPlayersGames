using UnityEngine;
using System.Collections;

public class PlayersScores {
	#region VARIABLES
	float[] playersScore = new float[]{0,0};
	#endregion

	#region SCORE_MANAGEMENT
	public void AddScore(int player, int ammount){
		playersScore[player] += ammount;
	}

	public void AddScoreToPlayer1(int ammount){
		playersScore[0] += ammount;
	}

	public void AddScoreToPlayer2(int ammount){
		playersScore[1] += ammount;
	}

	public void ResetScores(){
		playersScore[0] = 0;
		playersScore[1] = 0;
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
}
