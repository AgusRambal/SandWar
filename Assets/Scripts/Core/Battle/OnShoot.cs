using UnityEngine;

public class OnShoot : MonoBehaviour
{
    [SerializeField] private MarineObject marine;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform spawnPoint;

    public void Shoot()
    {
        var bulletInstantiated = Instantiate(bulletPrefab, spawnPoint);
        bulletInstantiated.transform.localPosition = Vector3.zero;
        bulletInstantiated.GetComponent<Bullet>().target = marine.target.transform;
    }
}
