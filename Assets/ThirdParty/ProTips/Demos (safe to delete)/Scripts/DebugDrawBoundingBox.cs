using UnityEngine;

namespace ModelShark
{
    public class DebugDrawBoundingBox : MonoBehaviour
    {
        void OnDrawGizmos()
        {
            // Draw mesh renderer bounds
            Renderer rend = gameObject.GetComponent<Renderer>();
            if (rend != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(transform.position, rend.bounds.size);
            }

            // Draw skinned mesh renderer bounds
            SkinnedMeshRenderer skinRend = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
            if (skinRend != null)
            {
                Transform rootBone = skinRend.rootBone;
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireCube(rootBone.position, skinRend.bounds.size);
            }

            // Draw collider bounds
            Collider coll = gameObject.GetComponent<Collider>();
            if (coll != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(transform.position, coll.bounds.size);
            }
        }
    }
}