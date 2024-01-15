using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

	[SerializeField] private GameObject ball;

	public void InitSpawner(int spawnCount)
	{
		int counter = 0;
		StartCoroutine(SpawnCR());

		IEnumerator SpawnCR()
		{
			while (counter < spawnCount)
			{
				SpawnBall();
				counter++;
				yield return new WaitForSeconds(0.05f);
			}
			ReferenceHolder.GameManager.canPlay = true;
		}
	}

	void SpawnBall()
	{
		var newBall = Lean.Pool.LeanPool.Spawn(ball, transform);
		newBall.transform.position = (Vector3.up * (20 + Random.Range(-3f, 3f))) + Vector3.right * Random.Range(-3f, 3f);
		ReferenceHolder.GameManager.currentBallList.Add(newBall.GetComponent<BallController>());
	}
}
