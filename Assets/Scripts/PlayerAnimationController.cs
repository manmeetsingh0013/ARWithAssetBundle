using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{

    Animator animator;

    AnimatorClipInfo[] info;

    AnimatorStateInfo allStates = new AnimatorStateInfo();

    [SerializeField] AnimationClip jumpClip;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        allStates = animator.GetNextAnimatorStateInfo(0);

        info = animator.GetNextAnimatorClipInfo(0);

    }

    void Start()
    {
        

    }
    void Update()
    {
        //Press the space bar to tell the Animator to trigger the Jump Animation
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Jump");
        }

        //When entering the Jump state in the Animator, output the message in the console
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("state"))
        {
            allStates = animator.GetNextAnimatorStateInfo(0);

            
            
        }
    }
}