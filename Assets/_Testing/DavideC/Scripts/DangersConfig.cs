using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DavideCTest
{
    [CreateAssetMenu(fileName = "dangerGenericConfig", menuName = "Create Config/Danger Config")]
    public class DangersConfig : ScriptableObject
    {
        public Danger.DangerTypesEnum DangerType = Danger.DangerTypesEnum.None;
        public bool isMovable =  false;
        public int explosionDamage = 20;
        public GameObject barrelModel;
        
        
    }
}
