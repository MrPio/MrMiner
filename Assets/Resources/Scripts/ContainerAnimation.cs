using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerAnimation : MonoBehaviour
{
    public Animator animator;
    public void Stop()
    {
        animator.enabled = false;
    }
}
