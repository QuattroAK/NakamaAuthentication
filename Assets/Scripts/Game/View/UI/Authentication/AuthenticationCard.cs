using Game.ViewModel.UI.Authentication;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Game.View.UI.Authentication
{
    public class AuthenticationCard : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Button button;

        [Inject] private readonly IAuthenticationCardModel model;

        public void Start()
        {
            icon.sprite = model.Sprite;
            button.onClick.AddListener(model.OnPressed);
        }
    }
}