using System.Collections.Generic;
using UnityEngine;

namespace NGS.MeshFusionPro
{
    public class CombinedLODGroupPart : ICombinedObjectPart<CombinedLODGroup>
    {
        ICombinedObject ICombinedObjectPart.Root
        {
            get
            {
                return Root;
            }
        }
        public CombinedLODGroup Root { get; private set; }

        public Bounds LocalBounds
        {
            get
            {
                return _localBounds;
            }
        }
        public Bounds Bounds
        {
            get
            {
                Bounds bounds = _localBounds;

                bounds.center += Root.transform.position;

                return bounds;
            }
        }

        private List<CombinedObjectPart> _baseParts;
        private Bounds _localBounds;


        public CombinedLODGroupPart(CombinedLODGroup root, List<CombinedObjectPart> baseParts)
        {
            Root = root;

            _baseParts = baseParts;

            CalculateLocalBounds();
        }

        public void Destroy()
        {
            Root.Destroy(this, _baseParts);
        }


        private void CalculateLocalBounds()
        {
            _localBounds = _baseParts[0].Bounds;

            for (int i = 1; i < _baseParts.Count; i++)
                _localBounds.Encapsulate(_baseParts[i].Bounds);

            _localBounds.center -= Root.transform.position;
        }
    }
}
