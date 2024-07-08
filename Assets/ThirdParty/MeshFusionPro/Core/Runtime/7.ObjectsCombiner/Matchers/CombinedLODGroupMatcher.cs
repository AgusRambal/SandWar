using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGS.MeshFusionPro
{
    public class CombinedLODGroupMatcher : CombinedObjectMatcher<CombinedLODGroup, LODGroupCombineSource>
    {
        private LODGroupSettings _settings;

        public override void StartMatching(CombinedLODGroup combinedObject)
        {
            _settings = combinedObject.Settings;
        }

        public override bool CanAddSource(LODGroupCombineSource source)
        {
            return source.Settings.IsEqual(_settings, 0.01f, 0.01f);
        }

        public override void SourceAdded(LODGroupCombineSource source)
        {

        }
    }
}
