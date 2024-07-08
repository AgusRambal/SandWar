using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace NGS.MeshFusionPro
{
    public enum SourceCombineStatus { NotCombined, AssignedToController, CombinedPartially, Combined, FailedToCombine }
    public enum CombineErrorStrategy { UndoCombining, Ignore }
    public enum AfterCombineAction { DisableComponents, DestroyGameObject, DoNothing }

    [DisallowMultipleComponent]
    public abstract class MeshFusionSource : MonoBehaviour
    {
        [field: SerializeField]
        public SourceCombineStatus CombineStatus { get; private set; }

        [field: SerializeField]
        public int ControllerIndex { get; set; }

        [field: SerializeField, HideInInspector]
        public bool CombineAtStart { get; set; } = true;

        [field: SerializeField, HideInInspector]
        public CombineErrorStrategy CombineErrorStrategy { get; set; }

        [field: SerializeField]
        public AfterCombineAction AfterCombineAction { get; set; }

        [field: SerializeField]
        public bool IsIncompatible { get; private set; }

        [field: SerializeField]
        public string IncompatibilityReason { get; private set; }

        [field: SerializeField]
        public bool HasCombineErrors { get; private set; }

        [field: SerializeField]
        public string CombineErrors { get; private set; }

        public UnityEvent<MeshFusionSource, IEnumerable<ICombinedObjectPart>> onCombineFinishedUnityEvent;
        public event Action<MeshFusionSource, IEnumerable<ICombinedObjectPart>> onCombineFinished;

        private RuntimeMeshFusion _controller;
        private int _combinedSourcesCount;
        private int _failedSourcesCount;
        private int _totalSourcesCount;


        private void Reset()
        {
            CheckCompatibility();
        }

        private void Start()
        {
            if (CombineAtStart)
                AssignToController();
        }

        private void OnDestroy()
        {
            if (CombineStatus == SourceCombineStatus.AssignedToController)
                UnassignFromController();
            
            ClearSourcesAndUnsubscribe();
        }


        public abstract bool TryGetBounds(ref Bounds bounds);

        public bool CheckCompatibility()
        {
            try
            {
                bool isCompatible = CheckCompatibilityAndGetComponents(out string reason);

                IsIncompatible = !isCompatible;
                IncompatibilityReason = reason;

                return isCompatible;
            }
            catch (Exception ex)
            {
                IsIncompatible = true;
                IncompatibilityReason += string.Format("\n{0}{1}", ex.Message, ex.StackTrace);

                return false;
            }
        }

        public bool AssignToController()
        {
            try
            {
                if (CombineStatus != SourceCombineStatus.NotCombined)
                    return false;

                if (!CheckCompatibility())
                    return false;

                _controller = RuntimeMeshFusion.FindByIndex(ControllerIndex);

                if (_controller == null)
                {
                    throw new NullReferenceException("RuntimeMeshFusion with index " +
                        + ControllerIndex + " not found");
                }

                CreateSourcesAndSubscribe();

                HasCombineErrors = false;
                CombineErrors = "";

                _combinedSourcesCount = 0;
                _failedSourcesCount = 0;
                _totalSourcesCount = 0;

                foreach (var source in GetCombineSources())
                {
                    _totalSourcesCount++;
                    _controller.AddSource(source);
                }

                CombineStatus = SourceCombineStatus.AssignedToController;

                return true;
            }
            catch(Exception ex)
            {
                HasCombineErrors = true;
                CombineErrors += string.Format("\n{0}\n{1}", ex.Message, ex.StackTrace);

                UnassignFromController();

                CombineStatus = SourceCombineStatus.FailedToCombine;

                return false;
            }
        }

        public void UndoCombine()
        {
            try
            {
                foreach (var part in GetCombinedParts())
                    part.Destroy();

                foreach (var source in GetCombineSources())
                    source.onCombined += (root, part) => part.Destroy();

                UnassignFromController();
                ClearSourcesAndUnsubscribe();
                ClearParts();

                _combinedSourcesCount = 0;
                _failedSourcesCount = 0;

                ToggleComponents(true);

                CombineStatus = SourceCombineStatus.NotCombined;
            }
            catch (Exception ex)
            {
                HasCombineErrors = true;

                CombineErrors += string.Format("\n{0}\n{1}", ex.Message, ex.StackTrace);

                CombineStatus = SourceCombineStatus.FailedToCombine;
            }
        }


        private void CreateSourcesAndSubscribe()
        {
            CreateSources();

            foreach (var source in GetCombineSources())
            {
                source.onCombined += OnSourceCombinedHandler;
                source.onCombineError += OnCombineErrorHandler;
                source.onCombineFailed += OnFailedCombineSourceHandler;
            }
        }

        private void ClearSourcesAndUnsubscribe()
        {
            foreach (var source in GetCombineSources())
            {
                source.onCombined -= OnSourceCombinedHandler;
                source.onCombineError -= OnCombineErrorHandler;
                source.onCombineFailed -= OnFailedCombineSourceHandler;
            }

            ClearSources();
        }

        private void UnassignFromController()
        {
            if (_controller != null)
            {
                foreach (var source in GetCombineSources())
                    _controller.RemoveSource(source);
            }

            _controller = null;
        }


        private void OnSourceCombinedHandler(ICombinedObject root, ICombinedObjectPart part)
        {
            CombineStatus = SourceCombineStatus.CombinedPartially;

            _combinedSourcesCount++;

            OnSourceCombinedInternal(root, part);

            if ((_combinedSourcesCount + _failedSourcesCount) == _totalSourcesCount)
                OnCombineFinishedHandler();
        }

        private void OnCombineErrorHandler(ICombinedObject root, string errorMessage)
        {
            HasCombineErrors = true;
            CombineErrors += errorMessage + "\n";

            OnCombineErrorInternal(root, errorMessage);

            if (CombineErrorStrategy == CombineErrorStrategy.UndoCombining)
                UndoCombine();
        }

        private void OnFailedCombineSourceHandler(ICombinedObject root)
        {
            _failedSourcesCount++;

            OnFailedCombineSourceInternal(root);

            if ((_combinedSourcesCount + _failedSourcesCount) == _totalSourcesCount)
                OnCombineFinishedHandler();
        }

        private void OnCombineFinishedHandler()
        {
            ClearSourcesAndUnsubscribe();

            if (_combinedSourcesCount == _totalSourcesCount)
            {
                CombineStatus = SourceCombineStatus.Combined;
            }
            else if (_failedSourcesCount == _totalSourcesCount)
            {
                CombineStatus = SourceCombineStatus.FailedToCombine;
                return;
            }

            OnCombineFinishedInternal();

            IEnumerable<ICombinedObjectPart> parts = GetCombinedParts();

            onCombineFinished?.Invoke(this, parts);
            onCombineFinishedUnityEvent?.Invoke(this, parts);

            if (AfterCombineAction == AfterCombineAction.DisableComponents)
            {
                ToggleComponents(false);
            }
            else if (AfterCombineAction == AfterCombineAction.DestroyGameObject)
            {
                Destroy(gameObject);
            }
        }


        protected virtual void OnSourceCombinedInternal(ICombinedObject root, ICombinedObjectPart part)
        {

        }

        protected virtual void OnCombineErrorInternal(ICombinedObject root, string errorMessage)
        {

        }

        protected virtual void OnFailedCombineSourceInternal(ICombinedObject root)
        {

        }

        protected virtual void OnCombineFinishedInternal()
        {

        }


        protected bool CanCreateCombineSource(
            GameObject go,
            ref string incompatibilityReason, 
            ref MeshRenderer renderer,
            ref MeshFilter filter,
            ref Mesh mesh)
        {
            if (renderer == null)
                renderer = go.GetComponent<MeshRenderer>();

            if (renderer == null)
            {
                incompatibilityReason = "MeshRenderer not found";
                return false;
            }

            if (renderer.isPartOfStaticBatch)
            {
                incompatibilityReason = "MeshRenderer is PartOfStaticBatching" +
                    "\nDisable Static Batching";
                return false;
            }

            if (filter == null)
                filter = go.GetComponent<MeshFilter>();

            if (filter == null)
            {
                incompatibilityReason = "MeshFilter not found";
                return false;
            }

            mesh = filter.sharedMesh;

            if (mesh == null)
            {
                incompatibilityReason = "Mesh not found";
                return false;
            }

            if (!mesh.isReadable)
            {
                incompatibilityReason = "Mesh is not readable. Enable Read/Write in import settings";
                return false;
            }

            if (mesh.subMeshCount != renderer.sharedMaterials.Length)
            {
                incompatibilityReason = "Submesh count and Materials count isn't equal";
                return false;
            }

            incompatibilityReason = "";
            return true;
        }

        protected abstract bool CheckCompatibilityAndGetComponents(out string incompatibilityReason);

        protected abstract void CreateSources();

        protected abstract void ClearSources();

        protected abstract void ClearParts();

        protected abstract IEnumerable<ICombineSource> GetCombineSources();

        protected abstract IEnumerable<ICombinedObjectPart> GetCombinedParts();

        protected abstract void ToggleComponents(bool enabled);
    }
}
