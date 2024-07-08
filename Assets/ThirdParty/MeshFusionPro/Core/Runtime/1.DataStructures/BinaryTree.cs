using System;
using System.Collections.Generic;
using UnityEngine;

namespace NGS.MeshFusionPro
{
    public abstract class BinaryTree<TNode, TData>
        where TNode : BinaryTreeNode<TData>
    {
        public IBinaryTreeNode Root
        {
            get
            {
                return RootInternal;
            }
        }
        public int Height { get; private set; }

        protected TNode RootInternal { get; private set; }
        private Dictionary<TData, List<TNode>> _dataToNodes;


        public BinaryTree()
        {
            Height = 0;

            _dataToNodes = new Dictionary<TData, List<TNode>>();
        }

        public void Add(TData data)
        {
            if (Root == null)
            {
                RootInternal = CreateRoot(data);
                Height = 1;
            }

            if (!Includes(RootInternal, data))
                GrowTreeUp(data);

            AddData(RootInternal, data, 1);
        }

        public bool Remove(TData data)
        {
            List<TNode> nodes;

            if (!_dataToNodes.TryGetValue(data, out nodes))
                return false;

            foreach (var node in nodes)
                node.Remove(data);

            return _dataToNodes.Remove(data);
        }


        protected void GrowTreeUp(TData target)
        {
            if (Includes(RootInternal, target))
                return;

            RootInternal = ExpandRoot(RootInternal, target);
            Height++;

            GrowTreeUp(target);
        }

        protected void GrowTreeDown(TNode node, TData target, int depth)
        {
            if (node.HasChilds)
                throw new Exception("GrowTreeDown::" + depth + " node already has childs");

            Bounds nodeBounds = node.Bounds;
            Vector3 offset;
            Vector3 size;

            if (nodeBounds.size.x > nodeBounds.size.y && nodeBounds.size.x > nodeBounds.size.z)
            {
                offset = new Vector3(nodeBounds.size.x / 4, 0, 0);
                size = new Vector3(nodeBounds.size.x / 2, nodeBounds.size.y, nodeBounds.size.z);
            }
            else if (nodeBounds.size.y > nodeBounds.size.x || nodeBounds.size.y > nodeBounds.size.z)
            {
                offset = new Vector3(0, nodeBounds.size.y / 4, 0);
                size = new Vector3(nodeBounds.size.x, nodeBounds.size.y / 2, nodeBounds.size.z);
            }
            else
            {
                offset = new Vector3(0, 0, nodeBounds.size.z / 4);
                size = new Vector3(nodeBounds.size.x, nodeBounds.size.y, nodeBounds.size.z / 2);
            }

            bool isLeaf = (depth == Height);

            TNode left = CreateNode(nodeBounds.center - offset, size, isLeaf);
            TNode right = CreateNode(nodeBounds.center + offset, size, isLeaf);

            node.SetChilds(left, right);

            if (isLeaf)
                return;

            if (Intersects(left, target))
                GrowTreeDown(left, target, depth + 1);

            if (Intersects(right, target))
                GrowTreeDown(right, target, depth + 1);
        }

        protected void AddData(TNode node, TData data, int depth)
        {
            if (node.IsLeaf)
            {
                node.Add(data);

                List<TNode> nodes;

                if (!_dataToNodes.TryGetValue(data, out nodes))
                {
                    nodes = new List<TNode>();

                    _dataToNodes.Add(data, nodes);
                }

                if (!nodes.Contains(node))
                    nodes.Add(node);

                return;
            }

            if (!node.HasChilds)
                GrowTreeDown(node, data, depth + 1);

            TNode left = (TNode)node.Left;
            TNode right = (TNode)node.Right;

            if (Intersects(left, data))
                AddData(left, data, depth + 1);

            if (Intersects(right, data))
                AddData(right, data, depth + 1);
        }

        protected void TreeTraversal(Func<TNode, int, bool> func)
        {
            TreeTraversal(RootInternal, 1, func);
        }

        protected void TreeTraversal(TNode current, int depth, Func<TNode, int, bool> func)
        {
            if (!func(current, depth))
                return;

            if (current.HasChilds)
            {
                TreeTraversal((TNode)current.Left, depth + 1, func);
                TreeTraversal((TNode)current.Right, depth + 1, func);
            }
        }


        protected abstract TNode CreateRoot(TData data);

        protected abstract TNode ExpandRoot(TNode root, TData target);

        protected abstract TNode CreateNode(Vector3 center, Vector3 size, bool isLeaf);

        protected abstract bool Intersects(TNode node, TData data);

        protected abstract bool Includes(TNode node, TData data);
    }

    public interface IBinaryTreeNode
    {
        Vector3 Center { get; }
        Vector3 Size { get; }
        Bounds Bounds { get; }

        bool HasChilds { get; }
        bool IsLeaf { get; }

        public IBinaryTreeNode GetLeft();

        public IBinaryTreeNode GetRight();
    }

    public abstract class BinaryTreeNode<TData> : IBinaryTreeNode
    {
        public BinaryTreeNode<TData> Left { get; private set; }
        public BinaryTreeNode<TData> Right { get; private set; }
        public bool IsLeaf { get; private set; }
        public bool HasChilds
        {
            get { return Left != null; }
        }

        public Vector3 Center { get; private set; }
        public Vector3 Size { get; private set; }
        public Bounds Bounds { get; private set; }


        public BinaryTreeNode(Vector3 center, Vector3 size, bool isLeaf)
        {
            Center = center;
            Size = size;
            Bounds = new Bounds(center, size);
            IsLeaf = isLeaf;
        }

        public IBinaryTreeNode GetLeft()
        {
            return Left;
        }

        public IBinaryTreeNode GetRight()
        {
            return Right;
        }


        public void SetChilds(BinaryTreeNode<TData> left, BinaryTreeNode<TData> right)
        {
            Left = left;
            Right = right;
        }

        public abstract void Add(TData data);

        public abstract void Remove(TData data);
    }
}
