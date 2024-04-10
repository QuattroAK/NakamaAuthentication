using UnityEngine;
using VContainer;
using VContainer.Unity;

public class DataScope : LifetimeScope
{
    [SerializeField] private ScriptableObject[] scriptableObjects;

    protected override void Configure(IContainerBuilder builder)
    {
        foreach (var info in scriptableObjects)
        {
            Debug.LogError($"Start{nameof(DataScope)}");
            builder.RegisterInstance(info).As(info.GetType());
        }
    }
}