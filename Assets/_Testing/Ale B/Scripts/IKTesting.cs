using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AleBorghesi
{

    [RequireComponent(typeof(Animator))]
    public class IKTesting : MonoBehaviour
    {
        protected Animator animator;

        private Transform spine, rightHand, rightArm;

        private IK_Bone hipsBone;
        [Header("BoneHits")]
        public bool updateBotta;
        public bool boneOffSetActive;
        public HumanBodyBones bone;
        public Vector3 offset;
        private IK_Bone targetBone;
        private Vector3 originalPos;

        void Start()
        {
            animator = GetComponent<Animator>();
            hipsBone = IK_Bone.CreateBody(animator);
            targetBone = IK_Bone.FindBoneFromEnum(animator, bone, hipsBone);
            originalPos = targetBone.Transform.localPosition;
        }

        public bool ikUpdate = true;
        private void OnAnimatorIK(int layerIndex)
        {
            if (!ikUpdate)
                return;
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            animator.SetIKPosition(AvatarIKGoal.RightHand, Vector3.one);
        }


        float time;
        void LateUpdate()
        {
            if (updateBotta)
            {

                updateBotta = false;
            }
            if (boneOffSetActive)
            {
                //Recuts();
                Recuts();
            }
        }


        private void Recuts()
        {
            if (targetBone.ParentBone == null)
            {
                targetBone.Transform.position += offset;
                return;
            }
            
            float dist = Vector3.Distance(targetBone.Transform.position, targetBone.ParentBone.Transform.position);
            targetBone.Transform.position += offset;
            Brubbo(targetBone.ParentBone, targetBone, dist);
            offset = Vector3.MoveTowards(offset, Vector3.zero, Time.deltaTime);
        }
        private void Brubbo(IK_Bone toMove, IK_Bone alreadyMoved, float oldDist)
        {
            float dist = 0;
            if(toMove.ParentBone != null)
                dist = Vector3.Distance(toMove.Transform.position, toMove.ParentBone.Transform.position);
            Vector3 oldPos = toMove.Transform.position;
            toMove.Transform.position = alreadyMoved.Transform.position + (toMove.Transform.position - alreadyMoved.Transform.position).normalized * oldDist;
            foreach (var c in toMove.ChildBones)
            {
                if(c != alreadyMoved)
                    DescendingBrubbo(c, toMove, oldPos);
            }
            if (toMove.ParentBone != null)
                Brubbo(toMove.ParentBone, toMove, dist);
        }
        private void DescendingBrubbo(IK_Bone toMove, IK_Bone alreadyMoved, Vector3 oldPos)
        {
            toMove.Transform.position = alreadyMoved.Transform.position + (toMove.Transform.position - alreadyMoved.Transform.position).normalized * Vector3.Distance(toMove.Transform.position, oldPos);
        }

    }

}