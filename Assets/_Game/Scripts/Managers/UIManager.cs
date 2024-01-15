using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI scoreText;
	[SerializeField] TextMeshProUGUI levelText;
	[SerializeField] Transform viewRoot;

	public List<View> m_Views;

	void Start()
	{
		m_Views = viewRoot.GetComponentsInChildren<View>(true).ToList();
	}
	public void Show<T>() where T : View
	{
		foreach (var view in m_Views)
		{
			if (view.GetType() == typeof(T))
			{
				Show(view);
				break;
			}
		}
	}
	public void Show(View view)
	{
		view.Show();
	}
	public void Hide<T>(bool keepInHistory = true) where T : View
	{
		foreach (var view in m_Views)
		{
			if (view is T)
			{

				Hide(view);
				break;
			}
		}
	}
	public void Hide(View view)
	{
		view.Hide();
	}


	public void UpdateScoreText(int _score)
	{
		scoreText.text = "Score:\n" + _score;
	}

	public void UpdateLevelText(int _level)
	{
		levelText.text = "Lv." + (_level + 1);
	}

	public void NextLevelButtonClicked()
	{
		Hide<EndGameView>();
		ReferenceHolder.GameManager.LoadNextLevel();
	}
	public void StartButtonClicked()
	{
		Hide<MainView>();
		ReferenceHolder.GameManager.LoadLevel();
	}
}
