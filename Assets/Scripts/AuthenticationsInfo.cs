using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Game/" + nameof(AuthenticationsInfo))]
public class AuthenticationsInfo : ScriptableObject
{
    [SerializeField] private AuthenticationInfo[] authenticationInfos;
    [SerializeField] private AuthentificationCard authentificationCard;

    public IReadOnlyList<AuthenticationInfo> AuthenticationInfos => authenticationInfos;
    public AuthentificationCard Card => authentificationCard;

    public bool TryGet(AuthenticationService authenticationService, out AuthenticationInfo info) =>
        (info = authenticationInfos.FirstOrDefault(info => info.ID == authenticationService)) != null;
}