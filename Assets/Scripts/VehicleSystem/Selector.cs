using UnityEngine;

namespace Selector
{
    public class Selector<T> where T : class
    {
        public T SelectedObject { get; private set; }

        public bool TrySelect(Ray ray)
        {
            RaycastHit hit;
            if (Physics.Raycast(ray,out hit))
            {
                T target = hit.collider.GetComponent<T>();

                if (target != null)
                {
                    SelectedObject = target;
                    return true;
                }
            }

            return false;
        }
    }
}
