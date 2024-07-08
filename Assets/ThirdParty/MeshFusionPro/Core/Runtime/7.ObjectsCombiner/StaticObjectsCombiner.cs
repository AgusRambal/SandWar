using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGS.MeshFusionPro
{
    public class StaticObjectsCombiner : ObjectsCombiner<CombinedObject, CombineSource>
    {
        private ICombinedMeshFactory _factory;
        private StaticCombinedObjectMatcher _matcher;
        private int _vertexLimit;

        public StaticObjectsCombiner(ICombinedMeshFactory factory, int vertexLimit)
        {
            _factory = factory;
            _matcher = new StaticCombinedObjectMatcher(vertexLimit);
            _vertexLimit = vertexLimit;
        }

        public override void AddSource(CombineSource source)
        {
            if (source.CombineInfo.vertexCount >= _vertexLimit)
                return;

            base.AddSource(source);
        }

        protected override void CombineSources(CombinedObject root, IList<CombineSource> sources)
        {
            root.Combine(sources);
        }

        protected override CombinedObject CreateCombinedObject(CombineSource source)
        {
            return CombinedObject.Create(_factory, source.RendererSettings);
        }

        protected override CombinedObjectMatcher<CombinedObject, CombineSource> GetMatcher()
        {
            return _matcher;
        }
    }
}
