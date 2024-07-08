using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGS.MeshFusionPro
{
    public class StaticCombinedObjectMatcher : CombinedObjectMatcher<CombinedObject, CombineSource>
    {
        private RendererSettings _settings;
        private int _vertexCount;
        private int _vertexLimit;

        public StaticCombinedObjectMatcher(int vertexLimit)
        {
            _vertexLimit = vertexLimit;
        }

        public override void StartMatching(CombinedObject combinedObject)
        {
            _settings = combinedObject.RendererSettings;
            _vertexCount = combinedObject.VertexCount;
        }

        public override bool CanAddSource(CombineSource source)
        {
            if (!_settings.Equals(source.RendererSettings))
                return false;

            if ((_vertexCount + source.CombineInfo.vertexCount) > _vertexLimit)
                return false;

            return true;
        }

        public override void SourceAdded(CombineSource source)
        {
            _vertexCount += source.CombineInfo.vertexCount;
        }
    }
}