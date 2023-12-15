using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]

public class IKControl : MonoBehaviour
{
    protected Animator animator;

    public bool ikActive = false;

    [Header("Legs IK")]
    public bool checkGround = false;
    public float step = 0.2f;
    [Range(0, 1)]
    public float feetRotWeight = 1;
    public Transform RightFoot, LeftFoot;
    [SerializeField] private LayerMask layerMask;

    [Header("Hands IK")]
    public Transform HandObj = null;
    [Range(0, 1)]
    public float handWeight = 1;
    public bool bothHands = false;

    [Header("Look IK")]
    public Transform lookObj = null;
    [Range(0,1)]
    public float lookWeight = 1;

    void Start()
    {
        animator = GetComponent<Animator>();

        RightPos = RightFoot.position;
        LeftPos = LeftFoot.position;
    }

    //a callback for calculating IK
    void OnAnimatorIK()
    {
        if (animator)
        {
            if (ikActive)
            {
                // look IK
                if (lookObj != null)
                {
                    animator.SetLookAtWeight(lookWeight);
                    animator.SetLookAtPosition(lookObj.position);
                }

                // Hand IK
                if (HandObj != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, handWeight);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, handWeight);
                    animator.SetIKPosition(AvatarIKGoal.RightHand, HandObj.position);
                    animator.SetIKRotation(AvatarIKGoal.RightHand, HandObj.rotation);

                    if (bothHands)
                    {
                        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, handWeight);
                        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, handWeight);
                        animator.SetIKPosition(AvatarIKGoal.LeftHand, HandObj.position);
                        animator.SetIKRotation(AvatarIKGoal.LeftHand, HandObj.rotation);
                    }
                }

                //Leg Ground IK
                if (checkGround)
                {
                    Vector3 offset = Vector3.up * step;

                    //L foot
                    Debug.DrawRay(LeftPos + offset, Vector3.down * 4, Color.red);
                    if (Physics.Raycast(LeftPos + offset, Vector3.down, out RaycastHit rayL, 10, layerMask))
                    {
                        if (Mathf.Abs(LeftPos.y - rayL.point.y) < step)
                        {
                            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
                            animator.SetIKPosition(AvatarIKGoal.LeftFoot, rayL.point);

                            animator.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(transform.forward, rayL.normal));
                            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, feetRotWeight);
                        }
                        else animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0);
                    }                   


                    //R foot
                    Debug.DrawRay(RightPos, Vector3.down *4, Color.blue);
                    if (Physics.Raycast(RightPos + offset, Vector3.down, out RaycastHit rayR, 10, layerMask))
                    {
                        if (Mathf.Abs(RightPos.y - rayR.point.y) < step)
                        {
                            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
                            animator.SetIKPosition(AvatarIKGoal.RightFoot, rayR.point);

                            animator.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(transform.forward, rayR.normal));
                            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, feetRotWeight);
                        }
                        else animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0);
                    }
                }
            }

            //if the IK is not active
            else
            {
                //look
                animator.SetLookAtWeight(0);
               
                //hands
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);

                //feet
                animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 0);
                animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 0);
            }
        }

    }


    private Vector3 RightPos, LeftPos;
    private void Update()
    {
        RightPos = RightFoot.position;
        LeftPos = LeftFoot.position;
    }
}