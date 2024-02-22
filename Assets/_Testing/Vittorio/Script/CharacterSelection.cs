using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using vittorio;
using static UnityEngine.InputSystem.InputAction;

public class CharacterSelection : MonoBehaviour, IMultiplayerInteractiveMenu {

    [SerializeField]List<CharacterPortaitSmall> mPortaitSmallList;
    //da aggiungere i portrait grandi
    Dictionary<UIPlayer, CharacterSO> selectedCharacter;
    private void Awake() {
        
    }

    private void OnEnable() {
        MenuManager.Instance.activeMenu = this;
    }
    public void Move(UIPlayer player,CallbackContext ctx) {
        
    }

    public void Select(UIPlayer player) {
        player.selectedCharacter = selectedCharacter[player];
        
    }
}
