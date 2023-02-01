using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AsteroidGameManager : MonoBehaviour
{
	[SerializeField] private TMP_Text scoreUI;
	[SerializeField] private GameObject gameoverUI;

	int score = 0;

	public void AddPoints(int points)
	{
		score += points;
		scoreUI.text = score.ToString();
	}

	public void SetGameOver()
	{
		gameoverUI.SetActive(true);
	}
}
