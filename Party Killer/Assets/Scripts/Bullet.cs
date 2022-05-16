using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class Bullet : MonoBehaviour
{

	public GameObject explosionPrefab;

	int bounce = 2;

	float lastBounceTime = 0f;

	private void OnCollisionEnter(Collision col)
	{
		if (col.collider.CompareTag("Player"))
		{
			GameManager.Instance.KillPlayer(col.collider.gameObject);
			Explode();
		} else if (col.collider.CompareTag("Bullet"))
		{
			Explode();
		} else
		{
			if (Time.time >= lastBounceTime + .05f)
			{
				if (bounce == 0)
				{
					Explode();
				}
				else
				{
					bounce--;
					lastBounceTime = Time.time;

					AudioManager.instance.Play("Bounce");
				}
			}
		}
		
	}

	void Explode()
	{
		AudioManager.instance.Play("Explode");

		GameObject explode = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
		Destroy(explode, 10f);
		Destroy(gameObject);

		CameraShaker.Instance.ShakeOnce(2f, 2f, .05f, .35f);
	}

}
