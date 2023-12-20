using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;
using DavideCTest;

public class EditorTools : EditorWindow
{
    
    [MenuItem("EditorTools/SpawnPlayer")]
    public static void SpawnPlayerDebug()
    {
        MainLogic.instance.SpawnPlayer();
    }

    [MenuItem("EditorTools/SpawnPallo")]
    public static void SpawnPalloDebug()
    {
        MainLogic.instance.SpawnPallo();
    }

}
