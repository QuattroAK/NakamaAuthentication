using System;
using UnityEngine;

[Serializable]
public class AuthenticationInfo
{
    [SerializeField] private AuthenticationService id;
    [SerializeField] private Sprite icon;

    public AuthenticationService ID => id;
    public Sprite Icon => icon;
}