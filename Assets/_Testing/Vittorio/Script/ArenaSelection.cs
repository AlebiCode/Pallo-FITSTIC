using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using vittorio;

public class ArenaSelection : MonoBehaviour, IMultiplayerInteractiveMenu
{
    [SerializeField] ArenaPreview mainPreview;
    [SerializeField] List<ArenaPortrait> arenaPortraitList;
    ArenaPortrait selectedArena;


    private void OnEnable() {
        MenuManager.Instance.activeMenu = this;
        selectedArena = arenaPortraitList[0];
        selectedArena.SelectButton();
        mainPreview.SetPreview(arenaPortraitList[0].Arena);
    }

    public void Move(UIPlayer player, InputAction.CallbackContext ctx) {
       
        if(player.PlayerNum != 0) {
            return;
        }

        if (ctx.ReadValue<Vector2>().x > 0.5f && arenaPortraitList.IndexOf(selectedArena) < arenaPortraitList.Count) {
            selectedArena = arenaPortraitList[arenaPortraitList.IndexOf(selectedArena) + 1];
            selectedArena.SelectButton();
            mainPreview.SetPreview(selectedArena.Arena);

        }

        if(ctx.ReadValue<Vector2>().x < -0.5f && arenaPortraitList.IndexOf(selectedArena) > 0) {
            selectedArena = arenaPortraitList[arenaPortraitList.IndexOf(selectedArena) - 1];
            selectedArena.SelectButton();
            mainPreview.SetPreview(selectedArena.Arena);
        }

    }

    public void Select(UIPlayer player) {

        SceneManager.LoadScene(selectedArena.Arena.sceneNumber);

    }


   



}
