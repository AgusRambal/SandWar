using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGS.MeshFusionPro
{
    public class BinaryTreeDrawer<TData>
    {
        public void DrawGizmo(IBinaryTreeNode root, Color color)
        {
            Gizmos.color = color;
            DrawNode(root);
        }

        private void DrawNode(IBinaryTreeNode node)
        {
            Gizmos.DrawWireCube(node.Center, node.Size);

            if (node.HasChilds)
            {
                DrawNode(node.GetLeft());
                DrawNode(node.GetRight());
            }
        }
    }
}
