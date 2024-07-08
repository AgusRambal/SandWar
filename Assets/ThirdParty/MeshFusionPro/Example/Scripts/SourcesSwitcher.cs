using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NGS.MeshFusionPro.Example
{
    public class SourcesSwitcher : MonoBehaviour
    {
        [SerializeField]
        private Text _sourcesEnabledText;

        [SerializeField]
        private Text _sourcesDisabledText;

        private bool _sourcesEnabled = true;


        private void LateUpdate()
        {
            if (SourcesList.UpdatedDirty)
            {
                ToggleSources(_sourcesEnabled);

                SourcesList.UpdatedDirty = false;
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                _sourcesEnabled = !_sourcesEnabled;

                ToggleSources(_sourcesEnabled);
            }
        }

        private void ToggleSources(bool enabled)
        {
            foreach (var source in SourcesList.Sources)
            {
                if (source)
                    ToggleSource(source, enabled);
            }

            foreach (var go in SourcesList.CombinedObjects)
                go.enabled = !enabled;

            _sourcesEnabledText.enabled = enabled;
            _sourcesDisabledText.enabled = !enabled;
        }

        private void ToggleSource(MeshFusionSource source, bool enabled)
        {
            if (source is LODMeshFusionSource)
            {
                source.GetComponent<LODGroup>().enabled = enabled;

                foreach (var renderer in source.GetComponentsInChildren<MeshRenderer>())
                    renderer.enabled = enabled;
            }
            else
            {
                source.GetComponent<MeshRenderer>().enabled = enabled;
            }
        }
    }
}
