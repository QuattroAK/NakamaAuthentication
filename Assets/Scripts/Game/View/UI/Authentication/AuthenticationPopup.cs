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

            for (var i = 0; i < servicesInfo.Count; i++)
            {
                var card = Instantiate(cardPrefab, cardsParent);

                var scope = container
                    .CreateScope(builder => { builder.RegisterInstance(servicesInfo[i]); });

                scope.InjectGameObject(card.gameObject);
                scopes.Add(scope);
                card.OnPressed.AddListener(OnClickCardHandler);
            }
        }

        private void OnDestroy()
        {
            foreach (var scope in scopes)
                scope.Dispose();
        }

        private void OnClickCardHandler(string service)
        {
            authenticationModel.SetAuthenticate(service);
        }
    }
}