using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	public float moveSpeed = 5f;

	Vector2 move;
	Vector2 aim;

	Rigidbody rb;
	PlayerWeapon weapon;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		weapon = GetComponent<PlayerWeapon>();
	}

	private void FixedUpdate()
	{
		Vector3 moveDir = new Vector3(-move.y, 0f, move.x);
		rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);

		if (aim.magnitude >= .5f)
			transform.rotation = Quaternion.LookRotation(new Vector3(-aim.y, 0f, aim.x)) * Quaternion.Euler(0f, 90f, 0f);
	}

	void OnMove(InputValue value)
	{
		if (GameManager.Instance.waitingForPlayers)
		{
			move = Vector2.zero;
			return;
		}

		move = value.Get<Vector2>();
	}

	void OnAim(InputValue value)
	{
		aim = value.Get<Vector2>();
	}

	void OnFire()
	{
		if (GameManager.Instance.waitingForPlayers)
			return;

		weapon.Shoot();
	}
}
