using System.Collections.Generic;
using System;
using UnityEngine;

namespace NGS.MeshFusionPro
{
    public abstract class ObjectsCombiner<TCombinedObject, TCombineSource>
        where TCombinedObject : ICombinedObject
        where TCombineSource : ICombineSource
    {
        public IReadOnlyList<TCombinedObject> CombinedObjects
        {
            get
            {
                return _combinedObjects;
            }
        }
        public bool ContainSources
        {
            get
            {
                return _sources.Count > 0;
            }
        }

        public event Action<TCombinedObject> onCombinedObjectCreated;

        private List<TCombinedObject> _combinedObjects;
        private List<TCombineSource> _sources;

        private List<TCombineSource> _sourcesForCombine;


        protected ObjectsCombiner()
        {
            _sources = new List<TCombineSource>();
            _combinedObjects = new List<TCombinedObject>();
            _sourcesForCombine = new List<TCombineSource>();
        }

        public virtual void AddSource(TCombineSource source)
        {
            _sources.Add(source);
        }

        public void AddSources(IEnumerable<TCombineSource> sources)
        {
            foreach (var source in sources)
                AddSource(source);
        }

        public void RemoveSource(TCombineSource source)
        {
            _sources.Remove(source);
        }

        public void Combine()
        {
            if (_sources.Count == 0)
                return;

            CleanEmptyData();
            CombineInternal();

            _sources.Clear();
        }


        private void CleanEmptyData()
        {
            int i = 0;
            while (i < _combinedObjects.Count)
            {
                if (_combinedObjects[i] == null)
                {
                    _combinedObjects.RemoveAt(i);
                    continue;
                }

                i++;
            }

            i = 0;
            while (i < _sources.Count)
            {
                if (_sources[i] == null)
                {
                    _sources.RemoveAt(i);
                    continue;
                }

                i++;
            }
        }

        private void CombineInternal()
        {
            _sourcesForCombine.Clear();

            int objIdx = 0;
            while (objIdx <= _combinedObjects.Count)
            {
                if (_sources.Count == 0)
                    return;

                TCombinedObject root;
                bool created = false;

                if (objIdx == _combinedObjects.Count)
                {
                    try
                    {
                        root = CreateCombinedObject(_sources[0]);

                        _combinedObjects.Add(root);

                        created = true;
                    }
                    catch (Exception ex)
                    {
                        Debug.Log("Unable to create CombinedObject : " + ex.Message + ex.StackTrace);
                        _sources.RemoveAt(0);
                        continue;
                    }
                }
                else
                {
                    root = _combinedObjects[objIdx];
                }

                CombinedObjectMatcher<TCombinedObject, TCombineSource> matcher = GetMatcher();
                matcher.StartMatching(root);

                int srcIdx = 0;
                while (srcIdx < _sources.Count)
                {
                    TCombineSource source = _sources[srcIdx];

                    if (matcher.CanAddSource(source))
                    {
                        _sourcesForCombine.Add(source);
                        matcher.SourceAdded(source);

                        _sources.RemoveAt(srcIdx);
                        continue;
                    }

                    srcIdx++;
                }

                if (_sourcesForCombine.Count > 0)
                {
                    try
                    {
                        CombineSources(root, _sourcesForCombine);
                        _sourcesForCombine.Clear();
                    }
                    catch(Exception ex)
                    {
                        Debug.Log("Unable to combine sources in ObjectsCombiner : " + ex.Message + ex.StackTrace);
                        _sourcesForCombine.Clear();
                        continue;
                    }
                }

                if (created)
                    onCombinedObjectCreated?.Invoke(root);

                objIdx++;
            }
        }


        protected abstract CombinedObjectMatcher<TCombinedObject, TCombineSource> GetMatcher();

        protected abstract TCombinedObject CreateCombinedObject(TCombineSource source);

        protected abstract void CombineSources(TCombinedObject root, IList<TCombineSource> sources);
    }
}
