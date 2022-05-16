using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class PlayerWeapon : MonoBehaviour
{
	public GameObject bulletPrefab;
	public Transform firePoint;

	public float bulletSpeed = 10f;

	public float bulletRefillRate = 1f;
	int bulletsReady = 0;
	public GameObject bulletUI01;
	public GameObject bulletUI02;
	public GameObject bulletUI03;

	float nextBulletTime = 0f;

	public void Reset()
	{
		bulletsReady = 1;
		nextBulletTime = Time.time + 1f / bulletRefillRate;
		bulletUI01.SetActive(true);
		bulletUI02.SetActive(false);
		bulletUI03.SetActive(false);
	}

	private void Update()
	{
		if (GameManager.Instance.waitingForPlayers || bulletsReady >= 3)
			return;

		if (Time.time >= nextBulletTime)
		{
			bulletsReady++;
			if (bulletsReady == 1)
			{
				bulletUI01.SetActive(true);
			} else if (bulletsReady == 2)
			{
				bulletUI02.SetActive(true);
			} else if (bulletsReady == 3)
			{
				bulletUI03.SetActive(true);
			}
			nextBulletTime = Time.time + 1f / bulletRefillRate;
		}
	}

	public void Shoot()
	{
		if (bulletsReady > 0)
		{
			GameObject ball = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
			Rigidbody ballRB = ball.GetComponent<Rigidbody>();
			ballRB.AddForce(firePoint.forward * bulletSpeed, ForceMode.VelocityChange);

			if (bulletsReady == 1)
			{
				bulletUI01.SetActive(false);
			} else if (bulletsReady == 2)
			{
				bulletUI02.SetActive(false);
			} else if (bulletsReady == 3)
			{
				bulletUI03.SetActive(false);
			}

			AudioManager.instance.Play("Shoot");
			CameraShaker.Instance.ShakeOnce(1.3f, 1.3f, .05f, .25f);

			bulletsReady--;
			nextBulletTime = Time.time + 1f / bulletRefillRate;
		}
	}

}
