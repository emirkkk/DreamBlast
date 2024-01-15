using System;
using System.Collections.Generic;
using UnityEngine;
public enum BallColor
{
	RED,
	GREEN,
	BLUE,
	//MAGENTA,
	//YELLOW,
	//ORANGE
}
public class BallController : MonoBehaviour
{
	[SerializeField] SpriteRenderer spriteRenderer;
	private BallColor color;
	public BallColor Color { get => color; set => color = value; }

	public List<BallController> onCollisionList;
	void Start()
	{
		SetColor(GetRandomColor());
	}

	public void Blast()
	{
		Lean.Pool.LeanPool.Spawn(ReferenceHolder.GameManager.blastFX, transform.position, Quaternion.identity);
		Lean.Pool.LeanPool.Despawn(this.gameObject);
	}

	private void SetColor(BallColor ballColor)
	{
		Color = ballColor;
		spriteRenderer.color = StringToColor(ballColor.ToString());
	}
	private BallColor GetRandomColor()
	{
		Array values = Enum.GetValues(typeof(BallColor));
		return (BallColor)values.GetValue(new System.Random().Next(values.Length));
	}
	public Color StringToColor(string color)
	{
		return (Color)typeof(Color).GetProperty(color.ToLowerInvariant()).GetValue(null, null);
	}

	private void OnCollisionStay(Collision collision)
	{
		if (collision == null) return;
		if (collision.gameObject.TryGetComponent(out BallController ball))
		{
			if (ball.Color != Color)
			{
				return;
			}

			if (!onCollisionList.Contains(ball))
			{
				onCollisionList.Add(ball);
			}
		}
	}
	private void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.TryGetComponent(out BallController ball))
		{
			if (onCollisionList.Contains(ball))
			{
				onCollisionList.Remove(ball);
			}
		}
	}
	private void OnDisable()
	{
		onCollisionList.Clear();
	}
}