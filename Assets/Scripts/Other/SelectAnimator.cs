using UnityEngine;

public class SelectAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private int animationValue;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        animator.SetInteger("Random", animationValue);
    }
}
