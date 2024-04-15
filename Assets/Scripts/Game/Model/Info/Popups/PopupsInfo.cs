using UnityEngine;

[CreateAssetMenu(menuName = "Game/" + nameof(PopupsInfo))]
public class PopupsInfo : ScriptableObject
{
    [SerializeField] private GameObject[] popups;

    public bool TryGet<T>(out T component) where T : Component
    {
        foreach (var popup in popups)
            if (popup.TryGetComponent(out component))
                return true;

        component = null;
        return false;
    }
}