using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
	public float speed;

	Vector3 dir;

	private void Start()
	{
		dir = Random.onUnitSphere;
	}

	// Update is called once per frame
	void Update()
    {
		
		transform.Rotate(dir * Time.deltaTime * speed);
    }
}
