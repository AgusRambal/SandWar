using System.Collections.Generic;
using UnityEngine;

namespace NGS.MeshFusionPro
{
    public class MeshDataListsSTD : CombinedMeshDataInternal
    {
        public List<Vector3> Vertices { get { return _vertices; } }
        public List<Vector3> Normals { get { return _normals; } }
        public List<Vector4> Tangents { get { return _tangents; } }
        public List<Bounds> PartsBounds { get { return _partsBounds; } }
        public List<Bounds> PartsBoundsLocal { get { return _partsBoundsLocal; } }
        public Bounds Bounds { get; set; }

        private CombinedMesh _root;
        private List<Vector3> _vertices;
        private List<Vector3> _normals;
        private List<Vector4> _tangents;
        private List<Bounds> _partsBounds;
        private List<Bounds> _partsBoundsLocal;


        public override Bounds GetBounds()
        {
            return Bounds;
        }

        public override Bounds GetBounds(CombinedMeshPart part)
        {
            return _partsBounds[part.Index];
        }

        public void ApplyDataToMesh()
        {
            Mesh mesh = _root.Mesh;

            mesh.SetVertices(_vertices);
            mesh.SetNormals(_normals);
            mesh.SetTangents(_tangents);

            mesh.bounds = Bounds;
        }


        protected override void OnInitialized()
        {
            _root = Root;
            _vertices = new List<Vector3>();
            _normals = new List<Vector3>();
            _tangents = new List<Vector4>();
            _partsBounds = new List<Bounds>();
            _partsBoundsLocal = new List<Bounds>();
        }

        protected override void OnAddPart(CombinedMeshPart part, Mesh mesh, Matrix4x4 transform)
        {
            Bounds localBounds = mesh.bounds;
            Bounds bounds = localBounds.Transform(transform);

            _partsBounds.Add(bounds);
            _partsBoundsLocal.Add(localBounds);
        }

        protected override void OnRemovePart(CombinedMeshPart part)
        {
            _partsBounds.RemoveAt(part.Index);
            _partsBoundsLocal.RemoveAt(part.Index);
        }

        protected override void OnMeshUpdated()
        {
            Mesh mesh = _root.Mesh;

            mesh.GetVertices(_vertices);
            mesh.GetNormals(_normals);
            mesh.GetTangents(_tangents);

            Bounds = mesh.bounds;
        }
    }
}
