using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;

public class EditorTools : EditorWindow
{
    
    [MenuItem("EditorTools/SpawnPlayer")]
    public static void SpawnPlayer()
    {
        MainLogic.instance.SpawnPlayer();
    }

}
