using Game.ViewModel.UI.Authentication;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VContainer;

namespace Game.View.UI.Authentication
{
    public class AuthenticationCard : MonoBehaviour
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
                Debug.LogError($"On Click  - {info.ServiceID}");
                OnPressed?.Invoke(info.ServiceID);
            });
        }
    }
}