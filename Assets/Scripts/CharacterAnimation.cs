using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    public static Action<bool> OnMovementChange;

    public void TriggerOnMovementChange()
    {
        OnMovementChange?.Invoke(true);
    }

}
