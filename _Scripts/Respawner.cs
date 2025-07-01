using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class Respawner : MonoBehaviour
{
    [SerializeField] private BoxCollider respawnArea;
    [SerializeField] private float respawnY = 1f;
    [SerializeField] private string[] objectTagsToSpawn; // Ahora usamos tags en lugar de prefabs
    [SerializeField] private float spawnInterval = 2f;
    [Header("Meteorite Configuration")]
    [SerializeField] private List<MeteoriteStats> availableMeteoriteTypes;
    private void Start()
    {
        StartCoroutine(SpawnObjectsRoutine());
    }

    private IEnumerator SpawnObjectsRoutine()
    {
        while (true)
        {
            string tagToSpawn = objectTagsToSpawn[Random.Range(0, objectTagsToSpawn.Length)];
            GameObject newObj = ObjectPoolManager.Instance.GetObjectFromPool(tagToSpawn);

            if (newObj != null)
            {
                if (tagToSpawn == "Meteorite")
                {
                    MeteoriteModel model = newObj.GetComponent<MeteoriteModel>();
                    if (model != null && availableMeteoriteTypes != null && availableMeteoriteTypes.Count > 0)
                    {
                        int randomIndex = Random.Range(0, availableMeteoriteTypes.Count);
                        model.meteoriteStats = availableMeteoriteTypes[randomIndex];
                    }

                }
                    RespawnAtRandomX(newObj);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (respawnArea == null) return;

        if (other.CompareTag("Coin") || other.CompareTag("Meteorite"))
        {
            ObjectPoolManager.Instance.ReturnObjectToPool(other.gameObject, other.tag);
            RespawnAtRandomX(other.gameObject);
        }
    }

    public void RespawnAtRandomX(GameObject obj)
    {
        Vector3 center = respawnArea.bounds.center;
        float halfWidth = respawnArea.bounds.extents.x;

        float randomX = Random.Range(center.x - halfWidth, center.x + halfWidth);
        Vector3 respawnPos = new Vector3(randomX, respawnY, 0);

        obj.transform.position = respawnPos;

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        obj.SetActive(true);
    }
}
