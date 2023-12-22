using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK_Bone
{
    [SerializeField] private IK_Bone parentBone;
    [SerializeField] private List<IK_Bone> childBones;
    private Transform transform;

    public IK_Bone ParentBone => parentBone;
    public List<IK_Bone> ChildBones => childBones;
    public Transform Transform => transform;

    public IK_Bone(Animator animator, HumanBodyBones bodyBone, IK_Bone parentBone)
    {
        transform = animator.GetBoneTransform(bodyBone);
        if (parentBone != null)
        {
            this.parentBone = parentBone;
            if (parentBone.childBones == null)
                parentBone.childBones = new List<IK_Bone>();
            parentBone.childBones.Add(this);
        }
    }
    
    public static IK_Bone CreateBody(Animator animator)
    {
        IK_Bone Hips = new IK_Bone(animator, HumanBodyBones.Hips, null);
        IK_Bone LeftUpperLeg = new IK_Bone(animator, HumanBodyBones.LeftUpperLeg, Hips);
        IK_Bone RightUpperLeg = new IK_Bone(animator, HumanBodyBones.RightUpperLeg, Hips);
        IK_Bone LeftLowerLeg = new IK_Bone(animator, HumanBodyBones.LeftLowerLeg, LeftUpperLeg);
        IK_Bone RightLowerLeg = new IK_Bone(animator, HumanBodyBones.RightLowerLeg, RightUpperLeg);
        IK_Bone LeftFoot = new IK_Bone(animator, HumanBodyBones.LeftFoot, LeftLowerLeg);
        IK_Bone RightFoot = new IK_Bone(animator, HumanBodyBones.RightFoot, RightLowerLeg);
        IK_Bone Spine = new IK_Bone(animator, HumanBodyBones.Spine, Hips);
        IK_Bone Chest = new IK_Bone(animator, HumanBodyBones.Chest, Spine);
        IK_Bone UpperChest = new IK_Bone(animator, HumanBodyBones.UpperChest, Chest);
        IK_Bone Neck = new IK_Bone(animator, HumanBodyBones.Neck, UpperChest);
        IK_Bone Head = new IK_Bone(animator, HumanBodyBones.Head, Neck);
        IK_Bone LeftShoulder = new IK_Bone(animator, HumanBodyBones.LeftShoulder, UpperChest);
        IK_Bone RightShoulder = new IK_Bone(animator, HumanBodyBones.RightShoulder, UpperChest);
        IK_Bone LeftUpperArm = new IK_Bone(animator, HumanBodyBones.LeftUpperArm, LeftShoulder);
        IK_Bone RightUpperArm = new IK_Bone(animator, HumanBodyBones.RightUpperArm, RightShoulder);
        IK_Bone LeftLowerArm = new IK_Bone(animator, HumanBodyBones.LeftLowerArm, LeftUpperArm);
        IK_Bone RightLowerArm = new IK_Bone(animator, HumanBodyBones.RightLowerArm, RightUpperArm);
        IK_Bone LeftHand = new IK_Bone(animator, HumanBodyBones.LeftHand, LeftLowerArm);
        IK_Bone RightHand = new IK_Bone(animator, HumanBodyBones.RightHand, RightLowerArm);

        return Hips;
    }

    //da usare solo per debug!!! fa schifo!!
    public static IK_Bone FindBoneFromEnum(Animator animator, HumanBodyBones boneToFind, IK_Bone hips)
    {
        Transform foundTransform = animator.GetBoneTransform(boneToFind);
        return FindBoneFromEnum_Ricorsive(hips, foundTransform);
    }
    private static IK_Bone FindBoneFromEnum_Ricorsive(IK_Bone sorgent, Transform target)
    {
        if(sorgent.childBones != null)
            foreach (IK_Bone bone in sorgent.ChildBones)
            {
                IK_Bone res = FindBoneFromEnum_Ricorsive(bone, target);
                if(res != null)
                    return res;
            }
        if (sorgent.transform == target)
            return sorgent;
        return null;
    }

}