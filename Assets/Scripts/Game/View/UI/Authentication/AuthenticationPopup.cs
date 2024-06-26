using Game.ViewModel.UI.Authentication;
using R3;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;
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

        private void Start()
        {
            authenticationModel.State.Subscribe(ApplyState).AddTo(gameObject);
            authenticationModel.AuthenticationMessageError.Subscribe(ShowErrorMessage).AddTo(gameObject);
            authenticationModel.Start();
            backButton.onClick.AddListener(Return);
            enterButton.onClick.AddListener(() => SetAuthenticate(authenticationModel.ServiceId));
            inputEmail.onValueChanged.AddListener(SetInputData);
            inputPassword.onValueChanged.AddListener(SetInputData);

            foreach (var cardModel in authenticationModel.GetAuthenticationsCards())
            {
                authenticationModel.Container
                    .CreateScope(builder => { builder.RegisterInstance(cardModel); })
                    .Instantiate(cardPrefab, cardsParent);

                cardModel.OnPressedEvent.AddListener(OnPressCardHandler);
            }
        }

        private void SetInputData(string _) =>
            authenticationModel.SetInputData((inputEmail.text, inputPassword.text));

        private void OnDestroy()
        {
            authenticationModel.Dispose();
        }

        private void SetAuthenticate(string serviceId) =>
            authenticationModel.SetAuthenticate(serviceId, (inputEmail.text, inputPassword.text));

        private void OnPressCardHandler(string serviceId) =>
            SetAuthenticate(serviceId);

        private void Return()
        {
            inputEmail.text = inputPassword.text = string.Empty;
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