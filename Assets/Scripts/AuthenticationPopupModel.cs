using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Nakama;
using UnityEngine;

public class AuthenticationPopupModel : IAuthenticationPopupModel
{
    private readonly AuthenticationServices authenticationServices;
    private readonly AuthenticationsInfo authenticationsInfo;
    private readonly IClient client;

    public int ServicesCount => authenticationServices.AuthenticationsCount;
    public AuthentificationCard CardPrefab => authenticationsInfo.Cart;

    public IReadOnlyList<AuthenticationInfo> AuthenticationsInfo =>
        authenticationsInfo.AuthenticationInfos;

    public AuthenticationPopupModel(AuthenticationServices authenticationServices,
        AuthenticationsInfo authenticationsInfo, IClient client)
    {
        Debug.LogError($"<color=yellow>Invoke ctor {nameof(AuthenticationPopupModel)}</color>");
        this.authenticationServices = authenticationServices;
        this.authenticationsInfo = authenticationsInfo;
        this.client = client;
    }

    public void SetAuthenticate(AuthenticationService serviceID)
    {
        authenticationServices.Authenticate(serviceID, client).Forget();
    }
}