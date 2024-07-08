using System;
using UnityEngine;

namespace NGS.MeshFusionPro
{
    public interface ICombineSource
    {
        Vector3 Position { get; }
        Bounds Bounds { get; }

        event Action<ICombinedObject, ICombinedObjectPart> onCombined;
        event Action<ICombinedObject, string> onCombineError;
        event Action<ICombinedObject> onCombineFailed;
    }

    public interface ICombineSource<TCombinedObject, TCombinedPart> : ICombineSource
        where TCombinedObject : ICombinedObject
        where TCombinedPart : ICombinedObjectPart
    {
        event Action<TCombinedObject, TCombinedPart> onCombinedTyped;
        event Action<TCombinedObject, string> onCombineErrorTyped;
        event Action<TCombinedObject> onCombineFailedTyped;
    }
}
