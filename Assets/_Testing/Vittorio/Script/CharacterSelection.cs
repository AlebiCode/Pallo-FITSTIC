using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using vittorio;
using static UnityEngine.InputSystem.InputAction;


/// <summary>
/// handles the input on the Character Selection Menu
/// </summary>
public class CharacterSelection : MonoBehaviour, IMultiplayerInteractiveMenu {

    [SerializeField] List<CharacterPortraitSmall> characterPreviewSmallList;
    [SerializeField] List<CharacterPortraitSelected> characterSelectedList;
    Dictionary<UIPlayer, CharacterPortraitSmall> selectedPreviewDictionary;
   
    private void Awake() {
        
    }

    private void OnEnable() {
        MenuManager.Instance.activeMenu = this;

        characterSelectedList.ForEach(go => go.gameObject.SetActive(false));
        for (int i = 0; i <= MenuManager.Instance.uiPlayers.Count; i++) {
            characterSelectedList[i].gameObject.SetActive(true);
            selectedPreviewDictionary[MenuManager.Instance.uiPlayers[i]] = characterPreviewSmallList[0];
            characterPreviewSmallList[0].playerIcons[i].SetActive(true);
        }


    }
    public void Move(UIPlayer player,CallbackContext ctx) {

        if(ctx.ReadValue<Vector2>().x > 0.5 &&
            characterPreviewSmallList.IndexOf(selectedPreviewDictionary[player]) < characterPreviewSmallList.Count) {

            selectedPreviewDictionary[player].DisablePlayerIcon(player);
            selectedPreviewDictionary[player] = characterPreviewSmallList[characterPreviewSmallList.IndexOf(selectedPreviewDictionary[player])+1];
            selectedPreviewDictionary[player].EnablePlayerIcon(player);
            return;
        }

        if(ctx.ReadValue<Vector2>().x < -0.5 
            && characterPreviewSmallList.IndexOf(selectedPreviewDictionary[player]) >0) {

            selectedPreviewDictionary[player].DisablePlayerIcon(player);
            selectedPreviewDictionary[player] = characterPreviewSmallList[characterPreviewSmallList.IndexOf(selectedPreviewDictionary[player]) - 1];
            selectedPreviewDictionary[player].EnablePlayerIcon(player);
            return;

        }

        if (ctx.ReadValue<Vector2>().y > 0.5
            && characterPreviewSmallList.IndexOf(selectedPreviewDictionary[player]) <= characterPreviewSmallList.Count/2) {

            selectedPreviewDictionary[player].DisablePlayerIcon(player);
            selectedPreviewDictionary[player] = characterPreviewSmallList[characterPreviewSmallList.IndexOf(selectedPreviewDictionary[player]) + (characterPreviewSmallList.Count / 2)];
            selectedPreviewDictionary[player].EnablePlayerIcon(player);
            return;

        }

        if (ctx.ReadValue<Vector2>().y < -0.5
            && characterPreviewSmallList.IndexOf(selectedPreviewDictionary[player]) >= characterPreviewSmallList.Count / 2) {

            selectedPreviewDictionary[player].DisablePlayerIcon(player);
            selectedPreviewDictionary[player] = characterPreviewSmallList[characterPreviewSmallList.IndexOf(selectedPreviewDictionary[player]) - (characterPreviewSmallList.Count / 2)];
            selectedPreviewDictionary[player].EnablePlayerIcon(player);
            return;

        }


    }

    public void Select(UIPlayer player) {

        if( player.PlayerNum == 0 && MenuManager.Instance.uiPlayers.All( player => player.selectedCharacter != null )) {
         //GO TO NEXT PANEL & return
        }
        player.selectedCharacter = selectedPreviewDictionary[player].character;
        characterSelectedList[player.PlayerNum].SetSprite(player.selectedCharacter.largePreviewImage); 
        
    }
}
