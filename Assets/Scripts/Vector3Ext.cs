using UnityEngine;

public static class Vector3Ext
{
    public static float DistanceXZ(this Vector3 from, Vector3 to)
    {
        return Mathf.Sqrt( Mathf.Pow(to.x - from.x,2) + Mathf.Pow( to.z - from.z,2) );
    }
}