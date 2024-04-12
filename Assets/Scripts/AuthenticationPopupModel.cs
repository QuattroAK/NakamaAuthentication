using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthenticationPopupModel : IAuthenticationPopupModel
{
    private readonly AuthenticationServices authenticationServices;
    public int ServicesCount => authenticationServices.AuthenticationsCount;

    public AuthenticationPopupModel(AuthenticationServices authenticationServices)
    {
        Debug.LogError($"<color=yellow>Invoke ctor {nameof(AuthenticationPopupModel)}</color>");
        this.authenticationServices = authenticationServices;
        Debug.LogError($"<color=green>Invoke ctor {authenticationServices.AuthenticationsCount}</color>");
    }
}