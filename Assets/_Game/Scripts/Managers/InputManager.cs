using UnityEngine;

public class InputManager : MonoBehaviour
{
	float counter = 0;
	void Update()
	{
		if (!ReferenceHolder.GameManager.canPlay)
		{
			return;
		}

		if (Input.GetMouseButtonDown(0))
		{
			OnFingerDown(Input.mousePosition);
			counter = 0;
		}
		else
		{
			counter += Time.deltaTime;
			if (counter > 3)
			{
				ReferenceHolder.GameManager.CheckGameLock();
				counter = 0;
			}
		}
	}

	private void OnFingerDown(Vector3 worldPoint)
	{
		Ray ray = Camera.main.ScreenPointToRay(worldPoint);
		RaycastHit hit;
		Collider targetCollider = null;
		if (Physics.Raycast(ray, out hit))
		{
			targetCollider = hit.collider;
		}
		if (targetCollider == null) return;

		var selectedBall = targetCollider.gameObject.GetComponent<BallController>();
		if (selectedBall == null) return;

		ReferenceHolder.GameManager.OnBallSelected(selectedBall);
	}


}
