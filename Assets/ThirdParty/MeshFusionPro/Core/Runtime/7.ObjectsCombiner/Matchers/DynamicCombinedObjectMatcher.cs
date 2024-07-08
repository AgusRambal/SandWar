using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGS.MeshFusionPro
{
    public class DynamicCombinedObjectMatcher :
    CombinedObjectMatcher<DynamicCombinedObject, DynamicCombineSource>
    {
        private RendererSettings _settings;
        private int _vertexCount;
        private int _vertexLimit;

        public DynamicCombinedObjectMatcher(int vertexLimit)
        {
            _vertexLimit = vertexLimit;
        }

        public override void StartMatching(DynamicCombinedObject combinedObject)
        {
            _settings = combinedObject.RendererSettings;
            _vertexCount = combinedObject.VertexCount;
        }

        public override bool CanAddSource(DynamicCombineSource source)
        {
            if (!_settings.Equals(source.Base.RendererSettings))
                return false;

            if ((_vertexCount + source.Base.CombineInfo.vertexCount) > _vertexLimit)
                return false;

            return true;
        }

        public override void SourceAdded(DynamicCombineSource source)
        {
            _vertexCount += source.Base.CombineInfo.vertexCount;
        }
    }
}
