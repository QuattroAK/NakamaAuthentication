using UnityEngine;
using UnityEngine.Events;

namespace Game.ViewModel.UI.Authentication
{
    public interface IAuthenticationCardModel
    {
        public Sprite Sprite { get; }
        public string Id { get; }
        public UnityEvent<string> OnPressedEvent { get; }
        public void OnPressed();
    }
}