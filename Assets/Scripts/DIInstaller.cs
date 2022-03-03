using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DIInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<GameManager>().FromComponentInHierarchy().AsSingle();

    }
}
