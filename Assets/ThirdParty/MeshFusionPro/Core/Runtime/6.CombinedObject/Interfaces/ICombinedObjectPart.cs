using UnityEngine;

namespace NGS.MeshFusionPro
{
    public interface ICombinedObjectPart
    {
        ICombinedObject Root { get; }

        Bounds Bounds { get; }
        Bounds LocalBounds { get; }

        void Destroy();
    }

    public interface ICombinedObjectPart<TCombinedObject> : ICombinedObjectPart
        where TCombinedObject : ICombinedObject
    {
        new TCombinedObject Root { get; }
    }
}
