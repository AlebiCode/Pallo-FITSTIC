using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


namespace AleBorghesi
{

    [RequireComponent(typeof(Animator))]
    public class IKTesting : MonoBehaviour
    {
        protected Animator animator;

        private IK_Bone hipsBone;
        public bool ikUpdate = true;
        [Header("BoneHits")]
        //public HumanBodyBones bone;
        public HumanBodyBones DEBUG_StartBottaBone;
        private IK_Bone bottaBone;
        public Vector3 bottaOffset;

        void Start()
        {
            animator = GetComponent<Animator>();
            hipsBone = IK_Bone.CreateBody(animator);
            bottaBone = hipsBone;
            bottaBone = IK_Bone.FindBoneFromEnum(animator, DEBUG_StartBottaBone, hipsBone);
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (!ikUpdate)
                return;
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            animator.SetIKPosition(AvatarIKGoal.RightHand, Vector3.one);
        }


        void LateUpdate()
        {
            if (bottaOffset.magnitude > 0)
            {
                //SimpleBotta();
                UpdateBottaBonesOffsets();
                //UpdateBottaBonesOffsetsV2(bottaBone, null, bottaOffset);
            }
        }

        private void SimpleBotta()
        {
            bottaBone.Transform.position += bottaOffset;
            bottaOffset = Vector3.MoveTowards(bottaOffset, Vector3.zero, Time.deltaTime);
        }
        private void UpdateBottaBonesOffsets()
        {
            if (bottaBone.ParentBone == null)
            {
                bottaBone.Transform.position += bottaOffset;
            }
            else
            {
                float dist = Vector3.Distance(bottaBone.Transform.position, bottaBone.ParentBone.Transform.position);
                bottaBone.Transform.position += bottaOffset;
                ChainMovementAscending(bottaBone.ParentBone, bottaBone, dist);
            }
            bottaOffset = Vector3.MoveTowards(bottaOffset, Vector3.zero, Time.deltaTime);
        }
        private void ChainMovementAscending(IK_Bone toMove, IK_Bone alreadyMoved, float oldDist)
        {
            float dist = 0;
            if(toMove.ParentBone != null)
                dist = Vector3.Distance(toMove.Transform.position, toMove.ParentBone.Transform.position);
            Vector3 oldPos = toMove.Transform.position;
            toMove.Transform.position = alreadyMoved.Transform.position + (toMove.Transform.position - alreadyMoved.Transform.position).normalized * oldDist;
            foreach (var c in toMove.ChildBones)
            {
                if(c != alreadyMoved)
                    ChainMovementDescending(c, toMove, oldPos);
            }
            if (toMove.ParentBone != null)
                ChainMovementAscending(toMove.ParentBone, toMove, dist);
        }
        private void ChainMovementDescending(IK_Bone toMove, IK_Bone alreadyMoved, Vector3 oldPos)
        {
            toMove.Transform.position = alreadyMoved.Transform.position + (toMove.Transform.position - alreadyMoved.Transform.position).normalized * Vector3.Distance(toMove.Transform.position, oldPos);
        }
        private void UpdateBottaBonesOffsetsV2(IK_Bone toMove, IK_Bone callerBone, Vector3 offset)
        {
            Vector3 oldPos = toMove.Transform.position;
            toMove.Transform.position += offset;
            if (toMove.ParentBone != null && toMove.ParentBone != callerBone)
            {
                float dist = Vector3.Distance(oldPos, toMove.ParentBone.Transform.position);
                Vector3 vec = toMove.ParentBone.Transform.position - toMove.Transform.position;
                UpdateBottaBonesOffsetsV2(toMove.ParentBone, toMove,  toMove.Transform.position + (vec.normalized * dist) - toMove.ParentBone.Transform.position);
            }
            if(toMove.ChildBones != null)
                foreach (var c in toMove.ChildBones)
                {
                    if(c == callerBone)
                        continue;
                    float dist = Vector3.Distance(oldPos, c.Transform.position);
                    Vector3 vec = c.Transform.position - toMove.Transform.position;
                    UpdateBottaBonesOffsetsV2(c, toMove, toMove.Transform.position + (vec.normalized * dist) - c.Transform.position);
                }
            bottaOffset = Vector3.MoveTowards(bottaOffset, Vector3.zero, Time.deltaTime);
        }

        public void SetBotta(Vector3 bottaPoint, Vector3 impulse)
        {
            bottaBone = GetClosestBoneToPoint(hipsBone, bottaPoint).Item1;
            bottaOffset = impulse;
        }
        private (IK_Bone, float) GetClosestBoneToPoint(IK_Bone startingBone, Vector3 point)
        {
            (IK_Bone, float) closest = (startingBone, Vector3.Distance(startingBone.Transform.position, point));
            foreach (var bone in startingBone.ChildBones)
            {
                (IK_Bone, float) newDist = GetClosestBoneToPoint(bone, point);
                if(newDist.Item2 < closest.Item2)
                    closest = newDist;
            }
            return closest;
        }
    }

}