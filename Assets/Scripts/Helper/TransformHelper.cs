using UnityEngine;

public class TransformHelper
{
    public static void SetTransformation(Transform sourceTransformation, Transform targetTransformation, params TransformationType[] types)
    {
        if (sourceTransformation == null || targetTransformation == null)
        {
            Debug.LogError("Source or target transformation null.");
            return;
        }

        if (types == null || types.Length == 0)
        {
            sourceTransformation.localPosition = targetTransformation.localPosition;
            sourceTransformation.localRotation = targetTransformation.localRotation;
            sourceTransformation.localScale = targetTransformation.localScale;
        }
        else
        {
            foreach (var type in types)
            {
                switch (type)
                {
                    case TransformationType.Position:
                        sourceTransformation.localPosition = targetTransformation.localPosition;
                        break;
                    case TransformationType.Rotation:
                        sourceTransformation.localRotation = targetTransformation.localRotation;
                        break;
                    case TransformationType.Scale:
                        sourceTransformation.localScale = targetTransformation.localScale;
                        break;
                }
            }
        }
    }

    public static void ResetTransformation(Transform target)
    {
        if (target == null)
        {
            Debug.LogError("Target transform is null.");
            return;
        }

        target.localPosition = Vector3.zero;
        target.localRotation = Quaternion.identity;
        target.localScale = Vector3.one;
    }

    public enum TransformationType
    {
        Position,
        Rotation,
        Scale
    }
}