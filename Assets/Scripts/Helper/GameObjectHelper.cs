using System.Collections.Generic;
using UnityEngine;

public static class GameObjectHelper
{
    /// <summary>
    /// Sets the active state of a GameObject and all its children.
    /// </summary>
    /// <param name="gameObject">The GameObject to set active.</param>
    /// <param name="isActive">The active state to set.</param>
    /// <remarks>
    /// This method will recursively set the active state of all child GameObjects.
    /// Be cautious when using this method, as it can affect the entire hierarchy of GameObjects.
    /// </remarks>
    public static void SetActive(IEnumerable<GameObject> gameObjects, bool isActive)
    {
        if (gameObjects == null)
            return;

        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject != null)
            {
                SetActive(gameObject.GetComponentsInChildren<GameObject>(), isActive);
                gameObject.SetActive(isActive);
            }
        }
    }
}
