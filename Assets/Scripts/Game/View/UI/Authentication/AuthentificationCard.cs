using Game.Model.Info;
using Game.Model.Services.Authentication;
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

        [Inject] private readonly AuthenticationInfo info;

        public readonly UnityEvent<AuthenticationService> Onclick = new();

        public void Start()
        {
            icon.sprite = info.Icon;
            button.onClick.AddListener(() =>
            {
                Onclick?.Invoke(info.ID);
                Debug.LogError($"{info.ID}");
            });
        }
    }
}