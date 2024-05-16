using System.Collections.Generic;
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

        [Inject] private readonly KeyValuePair<string, Sprite> info;

        public readonly UnityEvent<string> OnPressed = new();

        public void Start()
        {
            icon.sprite = info.Value;
            button.onClick.AddListener(() => OnPressed?.Invoke(info.Key));
        }
    }
}