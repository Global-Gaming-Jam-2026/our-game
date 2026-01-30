using UnityEngine;

public static class GamePhysics
{
    /// <summary>
    /// Checks if a given object's layer (as defined in the editor) is included in a given LayerMask
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="layerMask"></param>
    /// <returns></returns>
    public static bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return ((layerMask.value & (1 << obj.layer)) > 0);
    }

    public static Vector2 InitialVelocityFromTarget(Vector2 startPos, Vector2 endPos, float time, Vector2 acceleration)
    {
        return (2 * endPos - 2 * startPos - acceleration * time * time) / (2 * time);
    }
}
