using Game.Model.Services.Authentication;
using Game.ViewModel.UI.Authentication;
using VContainer.Unity;
using UnityEngine;
using VContainer;

namespace Game.View.UI.Authentication
{
    public class AuthenticationPopup : MonoBehaviour
    {
        [SerializeField] private AuthentificationCard cardPrefab;
        [SerializeField] private RectTransform cardsParent;

        [Inject] private readonly IAuthenticationPopupModel authenticationModel;
        [Inject] private readonly IObjectResolver container;

        private IScopedObjectResolver scope;

        private void Start()
        {
            Debug.LogError(authenticationModel.ServicesCount);

            for (int i = 0; i < authenticationModel.AuthenticationsInfo.Count; i++)
            {
                var card = Instantiate(cardPrefab, cardsParent);

                scope = container.CreateScope(builder =>
                {
                    builder.RegisterInstance(authenticationModel.AuthenticationsInfo[i]);
                });

                scope.InjectGameObject(card.gameObject);
                card.Onclick.AddListener(OnClickCardHandler);
            }
        }

        private void OnDestroy()
        {
            scope.Dispose();
        }

        private void OnClickCardHandler(AuthenticationService service)
        {
            authenticationModel.SetAuthenticate(service);
        }
    }
}