using VContainer;
using UnityEngine;
using VContainer.Unity;

namespace Game.Infrastructure.Controllers
{
    public class PopupsController
    {
        private readonly PopupsInfo popupsInfo;
        private readonly IObjectResolver container;

        public PopupsController(PopupsInfo popupsInfo, IObjectResolver container)
        {
            this.popupsInfo = popupsInfo;
            this.container = container;
        }

        public TComponent Show<TComponent, RObject1>(params object[] arguments)
            where TComponent : Component
        {
            if (!popupsInfo.TryGet(out TComponent component))
            {
                Debug.LogError($"Prefab not found {nameof(TComponent)}");
                return null;
            }

            var gameObject = Object.Instantiate(component.gameObject);

            var scope = container
                .CreateScope(builder =>
                {
                    builder.Register<RObject1>(Lifetime.Scoped).AsSelf().AsImplementedInterfaces();
                    foreach (var argument in arguments)
                        builder.RegisterInstance(argument).AsSelf().AsImplementedInterfaces();
                });

            scope.InjectGameObject(gameObject);

            return component;
        }
    }
}