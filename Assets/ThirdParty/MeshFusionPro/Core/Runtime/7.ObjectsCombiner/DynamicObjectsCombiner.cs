using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGS.MeshFusionPro
{
    public class DynamicObjectsCombiner : ObjectsCombiner<DynamicCombinedObject, DynamicCombineSource>
    {
        private ICombinedMeshFactory _factory;
        private DynamicCombinedObjectMatcher _matcher;
        private int _vertexLimit;

        public DynamicObjectsCombiner(ICombinedMeshFactory factory, int vertexLimit)
        {
            _factory = factory;
            _matcher = new DynamicCombinedObjectMatcher(vertexLimit);
            _vertexLimit = vertexLimit;
        }

        public override void AddSource(DynamicCombineSource source)
        {
            if (source.Base.CombineInfo.vertexCount >= _vertexLimit)
                return;

            base.AddSource(source);
        }

        protected override void CombineSources(DynamicCombinedObject root, 
            IList<DynamicCombineSource> sources)
        {
            root.Combine(sources);
        }

        protected override DynamicCombinedObject CreateCombinedObject(DynamicCombineSource source)
        {
            return DynamicCombinedObject.Create(_factory, source.Base.RendererSettings);
        }

        protected override CombinedObjectMatcher<DynamicCombinedObject, DynamicCombineSource> GetMatcher()
        {
            return _matcher;
        }
    }
}
