using System;
using System.Collections.Generic;

namespace NGS.MeshFusionPro
{
    public class UniversalObjectsCombiner
    {
        public event Action<CombinedObject> onStaticCombinedObjectCreated;
        public event Action<DynamicCombinedObject> onDynamicCombinedObjectCreated;
        public event Action<CombinedLODGroup> onCombinedLODGroupCreated;

        private StaticObjectsCombiner _staticCombiner;
        private DynamicObjectsCombiner _dynamicCombiner;
        private LODGroupsCombiner _lodCombiner;

        public UniversalObjectsCombiner(ICombinedMeshFactory factory, int vertexLimit)
        {
            _staticCombiner = new StaticObjectsCombiner(factory, vertexLimit);
            _dynamicCombiner = new DynamicObjectsCombiner(factory, vertexLimit);
            _lodCombiner = new LODGroupsCombiner(factory, vertexLimit);

            _staticCombiner.onCombinedObjectCreated += (r) => { onStaticCombinedObjectCreated?.Invoke(r); };
            _dynamicCombiner.onCombinedObjectCreated += (r) => { onDynamicCombinedObjectCreated?.Invoke(r); };
            _lodCombiner.onCombinedObjectCreated += (r) => { onCombinedLODGroupCreated?.Invoke(r); };
        }

        public void AddSource(ICombineSource source)
        {
            if (source is CombineSource s)
            {
                _staticCombiner.AddSource(s);
            }
            else if (source is DynamicCombineSource d)
            {
                _dynamicCombiner.AddSource(d);
            }
            else if (source is LODGroupCombineSource l)
            {
                _lodCombiner.AddSource(l);
            }
            else
                throw new NotImplementedException("Unknown Combine Source");
        }

        public void AddSources(IEnumerable<ICombineSource> sources)
        {
            foreach (var source in sources)
                AddSource(source);
        }

        public void RemoveSource(ICombineSource source)
        {
            if (source is CombineSource s)
            {
                _staticCombiner.RemoveSource(s);
            }
            else if (source is DynamicCombineSource d)
            {
                _dynamicCombiner.RemoveSource(d);
            }
            else if (source is LODGroupCombineSource l)
            {
                _lodCombiner.RemoveSource(l);
            }
            else
                throw new NotImplementedException("Unknown Combine Source");
        }

        public void Combine()
        {
            if (_staticCombiner.ContainSources)
                _staticCombiner.Combine();

            if (_dynamicCombiner.ContainSources)
                _dynamicCombiner.Combine();

            if (_lodCombiner.ContainSources)
                _lodCombiner.Combine();
        }
    }
}
