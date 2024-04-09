using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class ControllerScope : LifetimeScope
{
   protected override void Configure(IContainerBuilder builder)
   {
      builder.RegisterEntryPoint<ConnectionController>();
   }
}
