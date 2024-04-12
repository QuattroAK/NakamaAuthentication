using System.Collections.Generic;
using UnityEngine;

public class AuthenticationServices
{
    private readonly IReadOnlyList<IAuthenticationService> authentications;
    public int AuthenticationsCount => authentications.Count;

    public AuthenticationServices(IReadOnlyList<IAuthenticationService> authentications)
    {
        Debug.LogError($"<color=yellow>Invoke ctor {nameof(AuthenticationServices)}</color>");
        this.authentications = authentications;
    }
}