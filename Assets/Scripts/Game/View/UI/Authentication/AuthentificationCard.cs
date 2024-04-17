using Game.ViewModel.UI.Authentication;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VContainer;

namespace Game.View.UI.Authentication
{
    public class AuthentificationCard : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Button button;

        [Inject] private readonly AuthenticationServiceInfo info;

        public readonly UnityEvent<string> OnPressed = new();

        public void Start()
        {
            icon.sprite = info.ServiceIcon;
            button.onClick.AddListener(() =>
            {
                OnPressed?.Invoke(info.ServiceID);
                Debug.LogError($"{info.ServiceID}");
            });
        }
    }
}