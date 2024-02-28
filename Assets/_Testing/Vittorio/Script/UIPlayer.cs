using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using vittorio;
using static UnityEngine.InputSystem.InputAction;

public class UIPlayer : MonoBehaviour
{
    
    public PlayerInput playerInput;
    public CharacterSO selectedCharacter;
    public ArenaSO selectedArena;
    public int PlayerNum { get { return MenuManager.Instance.uiPlayers.IndexOf(this); } }


    private void Awake() {
        playerInput = GetComponent<PlayerInput>();
        DontDestroyOnLoad(gameObject);
        
    }
    private void Start() {
        MenuManager.Instance.AddPlayer(this);
    }

    private void OnDisable() {
        MenuManager.Instance.RemovePlayer(this);
    }


    public void OnMove(CallbackContext ctx) {
        MenuManager.Instance.activeMenu.Move(this, ctx);

    }

    public void OnSelect(CallbackContext ctx) {
        MenuManager.Instance.activeMenu.Select(this);
    }

}
