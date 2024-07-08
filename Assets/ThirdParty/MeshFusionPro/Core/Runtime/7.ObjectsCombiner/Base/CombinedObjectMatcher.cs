using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGS.MeshFusionPro
{
    public abstract class CombinedObjectMatcher<TCombinedObject, TCombineSource>
        where TCombinedObject : ICombinedObject
        where TCombineSource : ICombineSource
    {
        public abstract void StartMatching(TCombinedObject combinedObject);

        public abstract bool CanAddSource(TCombineSource source);

        public abstract void SourceAdded(TCombineSource source);
    }
}
