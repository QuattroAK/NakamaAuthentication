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
        [Header("Prefab")] 
        [SerializeField] private AuthenticationCard cardPrefab;
        [SerializeField] private RectTransform cardsParent;

        [Header("Inputs")] 
        [SerializeField] private InputField inputEmail;
        [SerializeField] private InputField inputPassword;

        [Header("Buttons")] 
        [SerializeField] private Button backButton;
        [SerializeField] private Button enterButton;

        [Header("Text")] 
        [SerializeField] private Text tileText;
        [SerializeField] private Text errorMessage;

        [Header("Panel")] 
        [SerializeField] private Image backgroundImage;

        [Header("Objects")] 
        [SerializeField] private GameObject connectionSuccess;
        [SerializeField] private GameObject connectionError;
        [SerializeField] private GameObject connectionWaiting;

        [Inject] private readonly IAuthenticationPopupModel authenticationModel;
        [Inject] private readonly IObjectResolver container;

        private readonly List<IScopedObjectResolver> scopes = new();

        private string serviceID;

        private void Start()
        {
            authenticationModel.OnChangeState.AddListener(ApplyState);
            authenticationModel.AuthenticationMessageError.AddListener(ShowErrorMessage);
            authenticationModel.Start();
            backButton.onClick.AddListener(OnClickBack);
            enterButton.onClick.AddListener(SetAuthenticate);
            inputEmail.onValueChanged.AddListener(SetInputData);
            inputPassword.onValueChanged.AddListener(SetInputData);

            var cardsInfo = authenticationModel.GetAuthenticationsCardsInfo();

            foreach (var cardInfo in cardsInfo)
            {
                var card = Instantiate(cardPrefab, cardsParent);

                var scope = container
                    .CreateScope(builder => { builder.RegisterInstance(cardInfo); });

                scope.InjectGameObject(card.gameObject);
                scopes.Add(scope);
                card.OnPressed.AddListener(SetAuthenticateId);
            }
        }

        private void SetInputData(string _)
        {
            authenticationModel.SetInputData((inputEmail.text, inputPassword.text));
        }

        private void OnDestroy()
        {
            foreach (var scope in scopes)
                scope.Dispose();
            
            authenticationModel.Dispose();
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
            inputEmail.text = inputPassword.text = serviceID = string.Empty;
            authenticationModel.Return();
        }

        private void ShowErrorMessage(string message) =>
            errorMessage.text = message;

        private void ApplyState(AuthenticationPopupState state)
        {
            cardsParent.gameObject.SetActive(state.Cards);
            inputEmail.gameObject.SetActive(state.InputEmail);
            inputPassword.gameObject.SetActive(state.InputPassword);
            backButton.gameObject.SetActive(state.BackButton);
            enterButton.gameObject.SetActive(state.Enter);
            tileText.gameObject.SetActive(state.TileText);
            backgroundImage.color = state.BackgroundColor;
            connectionError.SetActive(state.ConnectionError);
            connectionSuccess.SetActive(state.ConnectionSuccess);
            connectionWaiting.SetActive(state.ConnectionWaiting);
            errorMessage.gameObject.SetActive(state.AuthenticationError);
        }
    }
}