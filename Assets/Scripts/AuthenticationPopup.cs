using UnityEngine;
using VContainer;

public class AuthenticationPopup : MonoBehaviour
{
    [Inject] private readonly IAuthenticationPopupModel authenticationModel;

    private void Start()
    {
        Debug.LogError(authenticationModel.ServicesCount);
    }
}