using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public enum MapIndex
{
    Staging = 0
}

public class MapLoader : NetworkSceneManagerBase
{
    protected override IEnumerator SwitchScene(SceneRef prevScene, SceneRef newScene, FinishedLoadingDelegate finished)
    {
        Debug.Log($"Switching Scene from {prevScene} to {newScene}");
        yield return null;
    }
}
