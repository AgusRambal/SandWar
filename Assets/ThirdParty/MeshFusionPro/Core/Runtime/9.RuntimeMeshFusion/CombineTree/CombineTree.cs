using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NGS.MeshFusionPro
{
    public class CombineTree : BinaryTree<CombineTreeNode, ICombineSource>
    {
        public event Action<CombinedObject> onStaticCombinedObjectCreated;
        public event Action<DynamicCombinedObject> onDynamicCombinedObjectCreated;
        public event Action<CombinedLODGroup> onCombinedLODGroupCreated;

        public float LeafSize 
        {
            get 
            { 
                return _leafSize;
            } 
        }
        public int VertexLimit 
        {
            get 
            { 
                return _vertexLimit; 
            }
        }
        public ICombinedMeshFactory CombinedMeshFactory
        {
            get
            {
                return _factory;
            }
        }

        private ICombinedMeshFactory _factory;
        private float _leafSize;
        private int _vertexLimit;


        public CombineTree(ICombinedMeshFactory factory, float leafSize, int vertexLimit)
        {
            _factory = factory;
            _leafSize = leafSize;
            _vertexLimit = vertexLimit;
        }

        public void Combine()
        {
            TreeTraversal((node, depth) => 
            {
                node.Combine(); 
                return true; 
            });
        }


        protected override CombineTreeNode CreateRoot(ICombineSource source)
        {
            return CreateNode(source.Position, Vector3.one * _leafSize, true);
        }

        protected override CombineTreeNode CreateNode(Vector3 center, Vector3 size, bool isLeaf)
        {
            CombineTreeNode node = new CombineTreeNode(this, center, size, isLeaf);

            node.onStaticCombinedObjectCreated += onStaticCombinedObjectCreated;
            node.onDynamicCombinedObjectCreated += onDynamicCombinedObjectCreated;
            node.onCombinedLODGroupCreated += onCombinedLODGroupCreated;

            return node;
        }

        protected override CombineTreeNode ExpandRoot(CombineTreeNode root, ICombineSource target)
        {
            Bounds rootBounds = root.Bounds;
            Bounds targetBounds = target.Bounds;

            Vector3 parentCenter = Vector3.zero;
            Vector3 parentSize = Vector3.zero;

            Vector3 childCenter = Vector3.zero;

            bool rootIsLeft = false;

            for (int i = 0; i < 3; i++)
            {
                if (targetBounds.min[i] < rootBounds.min[i])
                {
                    parentSize = rootBounds.size;
                    parentSize[i] *= 2;

                    parentCenter = rootBounds.center;
                    parentCenter[i] -= rootBounds.size[i] / 2;

                    childCenter = rootBounds.center;
                    childCenter[i] -= rootBounds.size[i];

                    break;
                }

                if (targetBounds.max[i] > rootBounds.max[i])
                {
                    parentSize = rootBounds.size;
                    parentSize[i] *= 2;

                    parentCenter = rootBounds.center;
                    parentCenter[i] += rootBounds.size[i] / 2;

                    childCenter = rootBounds.center;
                    childCenter[i] += rootBounds.size[i];

                    rootIsLeft = true;

                    break;
                }
            }

            CombineTreeNode parent = CreateNode(parentCenter, parentSize, false);
            CombineTreeNode child = CreateNode(childCenter, rootBounds.size, root.IsLeaf);

            if (rootIsLeft)
                parent.SetChilds(RootInternal, child);
            else
                parent.SetChilds(child, RootInternal);

            return parent;
        }

        protected override bool Includes(CombineTreeNode node, ICombineSource source)
        {
            Bounds nodeBounds = node.Bounds;
            Bounds sourceBounds = source.Bounds;

            return nodeBounds.Contains(sourceBounds);
        }

        protected override bool Intersects(CombineTreeNode node, ICombineSource source)
        {
            return node.Bounds.Contains(source.Position);
        }
    }

    public class CombineTreeNode : BinaryTreeNode<ICombineSource>
    {
        public event Action<CombinedObject> onStaticCombinedObjectCreated;
        public event Action<DynamicCombinedObject> onDynamicCombinedObjectCreated;
        public event Action<CombinedLODGroup> onCombinedLODGroupCreated;

        private CombineTree _tree;
        private UniversalObjectsCombiner _combiner;

        public CombineTreeNode(CombineTree tree, Vector3 center, Vector3 size, bool isLeaf) : base(center, size, isLeaf)
        {
            _tree = tree;
        }

        public override void Add(ICombineSource source)
        {
            if (_combiner == null)
            {
                _combiner = new UniversalObjectsCombiner(_tree.CombinedMeshFactory, _tree.VertexLimit);

                _combiner.onStaticCombinedObjectCreated += onStaticCombinedObjectCreated;
                _combiner.onDynamicCombinedObjectCreated += onDynamicCombinedObjectCreated;
                _combiner.onCombinedLODGroupCreated += onCombinedLODGroupCreated;
            }

            _combiner.AddSource(source);
        }

        public override void Remove(ICombineSource source)
        {
            _combiner.RemoveSource(source);
        }

        public void Combine()
        {
            _combiner?.Combine();
        }
    }
}
