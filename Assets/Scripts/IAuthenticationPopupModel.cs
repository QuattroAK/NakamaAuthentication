using System.Collections.Generic;

public interface IAuthenticationPopupModel
{
    public int ServicesCount { get; }
    IReadOnlyList<AuthenticationInfo> AuthenticationsInfo { get; }
    public AuthentificationCard CardPrefab { get; }
    void SetAuthenticate(AuthenticationService service);
}