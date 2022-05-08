using UnityEngine;

public class ContainerAnimation : MonoBehaviour
{
    public Animator animator;
    public SwitchMode switchMode;

    public void End()
    {
        if (switchMode.mode)
            animator.enabled = false;
    }
    public void Begin()
    {
        if (!switchMode.mode)
            animator.enabled = false;
    }
}