using UnityEngine;

public class SelectAnimator : MonoBehaviour
{
    [SerializeField] private bool useGymAnims = false;
    [SerializeField] private Animator animator;
    [SerializeField] private int animationValue;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        if (!useGymAnims)
            return;

        animator.SetInteger("Random", animationValue);
    }
}
