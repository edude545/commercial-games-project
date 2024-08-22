using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnomalyFixerTarget
{

    public bool CanBeInteractedWith();

    public void OnInteract();

}
