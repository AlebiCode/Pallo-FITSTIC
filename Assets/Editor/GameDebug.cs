using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;
using DavideCTest;

public class EditorTools : EditorWindow
{
    
    [MenuItem("EditorTools/SpawnPlayer")]
    public static void SpawnPlayer()
    {
        MainLogic.instance.SpawnPlayer();
    }

}
