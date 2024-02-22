using UnityEngine.InputSystem.Users;

public class MenuPlayer 
{
    public InputUser inputUser;
    public CharacterSO chosenCharacter;
    public ArenaSO chosenArena;

    public MenuPlayer(InputUser inputUser) {
        this.inputUser = inputUser;
    }


}
