using System.Collections.Generic;
using Game.ViewModel.UI.Authentication;
using VContainer.Unity;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using Text = TMPro.TextMeshProUGUI;
using InputField = TMPro.TMP_InputField;

namespace Game.View.UI.Authentication
{
    public class AuthenticationPopup : MonoBehaviour
    {
        [Header("Prefab")] [SerializeField] private AuthenticationCard cardPrefab;
        [SerializeField] private RectTransform cardsParent;

        [Header("Inputs")] [SerializeField] private InputField inputEmail;
        [SerializeField] private InputField inputPassword;

        [Header("Buttons")] [SerializeField] private Button backButton;
        [SerializeField] private Button enterButton;

        [Header("Text")] [SerializeField] private Text tileText;

        [Header("Panel")] [SerializeField] private Image backgroundImage;

        [Inject] private readonly IAuthenticationPopupModel authenticationModel;
        [Inject] private readonly IObjectResolver container;

        private readonly List<IScopedObjectResolver> scopes = new();

        private string serviceID;

        private void Start()
        {
            authenticationModel.OnChangeState.AddListener(ApplyState);
            backButton.onClick.AddListener(OnClickBack);
            enterButton.onClick.AddListener(SetAuthenticate);
            inputEmail.onValueChanged.AddListener(ValidateInputData);
            inputPassword.onValueChanged.AddListener(ValidateInputData);
            OnClickBack();

            var servicesInfo = authenticationModel.GetAuthenticationsServiceInfos();

            foreach (var serviceInfo in servicesInfo)
            {
                var card = Instantiate(cardPrefab, cardsParent);

                var scope = container
                    .CreateScope(builder => { builder.RegisterInstance(serviceInfo); });

                scope.InjectGameObject(card.gameObject);
                scopes.Add(scope);
                card.OnPressed.AddListener(SetAuthenticateId);
            }
        }

        private void ValidateInputData(string _)
        {
            authenticationModel.ValidateInputData((inputEmail.text, inputPassword.text));
        }

        private void OnDestroy()
        {
            foreach (var scope in scopes)
                scope.Dispose();
        }

        private void SetAuthenticate()
        {
            authenticationModel.SetAuthenticate(serviceID, (inputEmail.text, inputPassword.text));
        }

        private void SetAuthenticateId(string serviceID)
        {
            this.serviceID = serviceID;
            SetAuthenticate();
        }

        private void OnClickBack()
        {
            authenticationModel.OnBack();
            inputEmail.text = inputPassword.text = serviceID = string.Empty;
        }

        private void ApplyState(AuthenticationPopupState state)
        {
            cardsParent.gameObject.SetActive(state.Cards);
            inputEmail.gameObject.SetActive(state.InputEmail);
            inputPassword.gameObject.SetActive(state.InputPassword);
            backButton.gameObject.SetActive(state.BackButton);
            enterButton.gameObject.SetActive(state.Enter);
            tileText.gameObject.SetActive(state.TileText);
            backgroundImage.color = state.BackgroundColor;
        }
    }
}