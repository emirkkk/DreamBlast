using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ReferenceHolder
{
	public static GameManager GameManager;
}
public class GameManager : MonoBehaviour
{
	public SpawnManager spawnManager;
	public UIManager UIManager;
	public List<BallController> currentBallList = new List<BallController>();
	public GameObject blastFX;
	public bool canPlay = false;


	private Level currentLevel;
	private const int MIN_MATCH = 3;
	private const int SCORE_MULTIPLIER = 50;
	private int playerLevelIndex;
	private int currentScore = 0;

	public int CurrentScore
	{
		get => currentScore;
		set
		{
			currentScore = value;
			UIManager.UpdateScoreText(currentScore);
		}
	}
	private void Awake()
	{
		ReferenceHolder.GameManager = this;
	}

	public void InitGame()
	{
		playerLevelIndex = PlayerPrefs.GetInt("PlayerLevel", 0);
		currentLevel = LevelListData.Instance.levelList[playerLevelIndex % LevelListData.Instance.levelList.Count];
		UIManager.UpdateLevelText(playerLevelIndex);
		CurrentScore = 0;
		ClearCurrentBalls();
		canPlay = false;
		spawnManager.InitSpawner(currentLevel.ballCount);
	}

	public void OnBallSelected(BallController selectedBall)
	{
		canPlay = false;
		List<BallController> neighbourBalls = SearchBallGroup(selectedBall);

		if (neighbourBalls.Count < MIN_MATCH)
		{
			canPlay = true;
			return;
		}

		CurrentScore += ((int)selectedBall.Color + 1) * SCORE_MULTIPLIER;
		foreach (var ball in neighbourBalls)
		{
			ball.Blast();
			currentBallList.Remove(ball);
		}

		if (currentBallList.Count == 0)
		{
			UIManager.Show<EndGameView>();
			return;
		}
		canPlay = true;
	}
	public void CheckGameLock()
	{
		if (!IsGameLocked())
		{
			Debug.Log("Game locked!");
			canPlay = false;
			UIManager.Show<EndGameView>();
		}
	}
	private List<BallController> SearchBallGroup(BallController selectedBall)
	{
		List<BallController> neighbourBalls = new List<BallController> { selectedBall };
		foreach (var neighbor in selectedBall.onCollisionList)
		{
			CheckNeighborsRecursive(neighbor, neighbourBalls);
		}

		return neighbourBalls;
	}

	private bool IsGameLocked()
	{
		foreach (var b in currentBallList)
		{
			List<BallController> neighbourBalls = SearchBallGroup(b);
			if (neighbourBalls.Count >= MIN_MATCH)
			{
				return true;
			}
		}
		return false;
	}

	private void CheckNeighborsRecursive(BallController currentBall, List<BallController> neighbourBalls)
	{
		if (neighbourBalls.Contains(currentBall))
		{
			return;
		}
		neighbourBalls.Add(currentBall);
		foreach (var neighbor in currentBall.onCollisionList)
		{
			CheckNeighborsRecursive(neighbor, neighbourBalls);
		}
	}


	public void LoadNextLevel()
	{
		int currentLevel = PlayerPrefs.GetInt("PlayerLevel");
		currentLevel++;
		PlayerPrefs.SetInt("PlayerLevel", currentLevel);
		PlayerPrefs.Save();
		LoadLevel();
	}

	public void LoadLevel()
	{
		InitGame();
	}

	private void ClearCurrentBalls()
	{
		if (currentBallList.Count > 0)
		{
			foreach (BallController b in currentBallList)
			{
				Lean.Pool.LeanPool.Despawn(b);
			}
		}
		currentBallList.Clear();
	}
}
