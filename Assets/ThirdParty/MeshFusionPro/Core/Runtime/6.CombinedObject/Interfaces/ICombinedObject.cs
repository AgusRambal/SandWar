using System.Collections.Generic;

namespace NGS.MeshFusionPro
{
    public interface ICombinedObject
    {
        IReadOnlyList<ICombinedObjectPart> Parts { get; }

        void Combine(IEnumerable<ICombineSource> sources);
    }

    public interface ICombinedObject<TCombinedPart, TCombinedSource> : ICombinedObject
        where TCombinedPart : ICombinedObjectPart
        where TCombinedSource : ICombineSource
    {
        new IReadOnlyList<TCombinedPart> Parts { get; }

        void Combine(IEnumerable<TCombinedSource> sources);
    }
}
