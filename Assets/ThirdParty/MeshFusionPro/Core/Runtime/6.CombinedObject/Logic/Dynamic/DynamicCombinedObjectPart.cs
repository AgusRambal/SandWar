using UnityEngine;

namespace NGS.MeshFusionPro
{
    public abstract class DynamicCombinedObjectPart : ICombinedObjectPart<DynamicCombinedObject>
    {
        ICombinedObject ICombinedObjectPart.Root
        {
            get
            {
                return Root;
            }
        }
        public DynamicCombinedObject Root
        {
            get
            {
                return _root;
            }
        }

        public Bounds LocalBounds
        {
            get
            {
                return _basePart.LocalBounds;
            }
        }
        public Bounds Bounds
        {
            get
            {
                return _basePart.Bounds;
            }
        }

        protected DynamicCombinedObject _root;
        protected CombinedObjectPart _basePart;

        private bool _destroyed;

        public DynamicCombinedObjectPart(DynamicCombinedObject root, CombinedObjectPart basePart,
            Matrix4x4 transformMatrix)
        {
            _root = root;
            _basePart = basePart;
        }

        public abstract void Move(Vector3 position, Quaternion rotation, Vector3 scale);

        public abstract void Move(Matrix4x4 transform);

        public abstract void MoveLocal(Matrix4x4 localTransform);

        public void Destroy()
        {
            if (_destroyed)
            {
                Debug.Log("Part already destroyed");
                return;
            }

            _root.Destroy(this, _basePart);
        }
    }

    public class DynamicCombinedObjectPartInternal : DynamicCombinedObjectPart
    {
        private Matrix4x4 _localTransform;
        private Matrix4x4 _targetLocalTransform;
        private Matrix4x4 _worldToLocalMatrix;
        private bool _inMove;

        public DynamicCombinedObjectPartInternal(DynamicCombinedObject root, CombinedObjectPart basePart, 
            Matrix4x4 transformMatrix) : base(root, basePart, transformMatrix)
        {
            Vector3 localPosition = transformMatrix.GetTranslation() - root.transform.position;

            _localTransform = transformMatrix.SetTranslation(localPosition);
            _worldToLocalMatrix = Root.transform.worldToLocalMatrix;
        }

        public override void Move(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            Move(Matrix4x4.TRS(position, rotation, scale));
        }

        public override void Move(Matrix4x4 transform)
        {
            Matrix4x4 localTransform = _worldToLocalMatrix * transform;

            MoveLocal(localTransform);
        }

        public override void MoveLocal(Matrix4x4 localTransform)
        {
            if (_inMove)
                return;

            _targetLocalTransform = localTransform;
            _root.UpdatePart(this);

            _inMove = true;
        }


        public PartMoveInfo CreateMoveInfo()
        {
            CombinedMeshPart meshPart = _basePart.MeshPart;

            return new PartMoveInfo()
            {
                partIndex = meshPart.Index,
                vertexStart = meshPart.VertexStart,
                vertexCount = meshPart.VertexCount,
                currentTransform = _localTransform,
                targetTransform = _targetLocalTransform
            };
        }

        public void PositionUpdated()
        {
            _localTransform = _targetLocalTransform;
            _inMove = false;
        }
    }
}
