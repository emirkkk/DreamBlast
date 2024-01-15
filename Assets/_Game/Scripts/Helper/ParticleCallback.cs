using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCallback : MonoBehaviour
{
	void OnParticleSystemStopped()
	{
		Lean.Pool.LeanPool.Despawn(gameObject);
	}
}
