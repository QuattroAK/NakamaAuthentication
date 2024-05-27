using UnityEngine;
using UnityEngine.Events;

namespace Game.ViewModel.UI.Authentication
{
    public class AuthenticationCardModel : IAuthenticationCardModel
    {
        public Sprite Sprite { get; }
        public string Id { get; }
        public UnityEvent<string> OnPressedEvent { get; } = new();

        public AuthenticationCardModel(Sprite sprite, string id)
        {
            Sprite = sprite;
            Id = id;
        }

        public void OnPressed() =>
            OnPressedEvent?.Invoke(Id);
    }
}