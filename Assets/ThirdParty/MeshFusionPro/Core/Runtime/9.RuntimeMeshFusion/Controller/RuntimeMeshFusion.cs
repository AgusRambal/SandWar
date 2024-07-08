using System.Collections.Generic;
using UnityEngine;


namespace NGS.MeshFusionPro
{
    public class RuntimeMeshFusion : MonoBehaviour
    {
        private static List<RuntimeMeshFusion> _Instances;

        public int ControllerIndex
        {
            get
            {
                return _controllerIndex;
            }
            set
            {
                if (!Application.isPlaying)
                    _controllerIndex = value;
            }
        }
        public bool DrawGizmo
        {
            get
            {
                return _drawGizmo;
            }
            set
            {
                _drawGizmo = value;
            }
        }
        public int CellSize
        {
            get
            {
                return _cellSize;
            }
            set
            {
                if (Application.isPlaying)
                    return;

                _cellSize = Mathf.Max(1, value);
            }
        }
        public bool LimitVertices
        {
            get
            {
                return _maxVerticesPerObject <= 65535;
            }
            set
            {
                if (Application.isPlaying)
                    return;

                if (value)
                {
                    _maxVerticesPerObject = 65535;
                }
                else
                {
                    _maxVerticesPerObject = int.MaxValue;
                }
            }
        }
        public MeshType MeshType
        {
            get
            {
                return _meshType;
            }
            set
            {
                if (!Application.isPlaying)
                    _meshType = value;
            }
        }
        public MoveMethod MoveMethod
        {
            get
            {
                return _moveMethod;
            }
            set
            {
                if (!Application.isPlaying)
                    _moveMethod = value;
            }
        }

        [SerializeField, HideInInspector]
        private int _controllerIndex = 0;

        [SerializeField, HideInInspector]
        private bool _drawGizmo;

        [SerializeField, HideInInspector]
        private int _cellSize = 80;

        [SerializeField, HideInInspector]
        private int _maxVerticesPerObject = 65535;

        [SerializeField, HideInInspector]
        private MeshType _meshType = MeshType.Standard;

        [SerializeField, HideInInspector]
        private MoveMethod _moveMethod = MoveMethod.Jobs;

        private CombineTree _combineTree;
        private bool _sourceAdded;

        private BinaryTreeDrawer<ICombineSource> _treeDrawer;


        private void Awake()
        {
            if (_Instances == null)
                _Instances = new List<RuntimeMeshFusion>();

            _Instances.Add(this);

            ICombinedMeshFactory factory = new CombinedMeshFactory(_meshType, CombineMethod.Simple, _moveMethod);

            _combineTree = new CombineTree(factory, _cellSize, _maxVerticesPerObject);
            _treeDrawer = new BinaryTreeDrawer<ICombineSource>();

            Transform parent = new GameObject("CombinedObjects").transform;

            _combineTree.onStaticCombinedObjectCreated += (r) => { r.transform.parent = parent; };
            _combineTree.onDynamicCombinedObjectCreated += (r) => { r.transform.parent = parent; };
            _combineTree.onCombinedLODGroupCreated += (r) => { r.transform.parent = parent; };
        }

        private void Update()
        {
            if (_sourceAdded)
            {
                _combineTree.Combine();
                _sourceAdded = false;
            }
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying || !_drawGizmo)
                return;

            if (_combineTree != null && _combineTree.Root != null)
                _treeDrawer.DrawGizmo(_combineTree.Root, Color.white);
        }

        private void OnDestroy()
        {
            _Instances.Remove(this);
        }


        public static RuntimeMeshFusion FindByIndex(int index)
        {
            for (int i = 0; i < _Instances.Count; i++)
            {
                RuntimeMeshFusion controller = _Instances[i];

                if (controller.ControllerIndex == index)
                    return controller;
            }

            throw new KeyNotFoundException("MeshFusionController with index : " + index + " not found");
        }


        public void AddSource(ICombineSource source)
        {
            _combineTree.Add(source);

            _sourceAdded = true;
        }

        public void RemoveSource(ICombineSource source)
        {
            _combineTree.Remove(source);
        }
    }
}
