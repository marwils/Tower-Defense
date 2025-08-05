using System.Collections.Generic;
using UnityEngine;

public static class GameObjectHelper
{
    /// <summary>
    /// Sets the active state of a GameObject and all its children.
    /// </summary>
    /// <param name="gameObjects">The GameObjects to set active.</param>
    /// <param name="isActive">The active state to set.</param>
    /// <remarks>
    /// This method will recursively set the active state of all child GameObjects.
    /// It is useful for enabling or disabling entire hierarchies of GameObjects in Unity.
    /// Be cautious when using this method, as it can affect the entire hierarchy of GameObjects.
    /// </remarks>
    public static void SetActive(IEnumerable<GameObject> gameObjects, bool isActive = true)
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
