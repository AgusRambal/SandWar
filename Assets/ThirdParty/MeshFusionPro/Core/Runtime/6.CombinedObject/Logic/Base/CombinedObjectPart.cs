using UnityEngine;

namespace NGS.MeshFusionPro
{
    public class CombinedObjectPart : ICombinedObjectPart<CombinedObject>
    {
        ICombinedObject ICombinedObjectPart.Root { get { return Root; } }
        public CombinedObject Root { get; private set; }
        public CombinedMeshPart MeshPart { get; private set; }

        public Bounds LocalBounds
        {
            get
            {
                return Root.GetLocalBounds(this);
            }
        }
        public Bounds Bounds
        {
            get
            {
                return Root.GetBounds(this);
            }
        }

        private bool _destroyed;

        public CombinedObjectPart(CombinedObject root, CombinedMeshPart meshPart)
        {
            Root = root;
            MeshPart = meshPart;
        }

        public void Destroy()
        {
            if (_destroyed)
            {
                Debug.Log("CombinedPart already destroyed");
                return;
            }

            Root.Destroy(this);

            _destroyed = true;
        }
    }
}
