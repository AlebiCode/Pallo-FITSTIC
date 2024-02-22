using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public interface IMultiplayerInteractiveMenu 
{
    public void Move(UIPlayer player, CallbackContext ctx);
    public void Select(UIPlayer player);
}
