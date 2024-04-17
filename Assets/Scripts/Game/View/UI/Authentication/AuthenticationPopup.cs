using System.Collections.Generic;
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

        private readonly List<IScopedObjectResolver> scopes = new();

        private void Start()
        {
            var servicesInfo = authenticationModel.GetAuthenticationsServiceInfos();

            foreach (var serviceInfo in servicesInfo)
            {
                var card = Instantiate(cardPrefab, cardsParent);

                var scope = container
                    .CreateScope(builder => { builder.RegisterInstance(serviceInfo); });

                scope.InjectGameObject(card.gameObject);
                scopes.Add(scope);
                card.OnPressed.AddListener(SetAuthenticate);
            }
        }

        private void OnDestroy()
        {
            foreach (var scope in scopes)
                scope.Dispose();
        }

        private void SetAuthenticate(string serviceID)
        {
            authenticationModel.SetAuthenticate(serviceID);
        }
    }
}