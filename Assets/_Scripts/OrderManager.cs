using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager instance;
    public GameObject orderPrefab;
    public float spawnRange = 20f;
    public FractionalBrownianMotion noiseGen;
    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }

    void Start()
    {
        SpawnOrder();
    }

    public void SpawnOrder()
    {
        Vector2 random = Random.insideUnitCircle * spawnRange;
        Vector2 flatPos2 = new(transform.position.x, transform.position.z);
        Vector3 spawnPos = new(random.x, noiseGen.GetNoiseAtPos(flatPos2+ random), random.y);
        Vector3 flatPos = new(transform.position.x, 0f, transform.position.z);
        GameObject order = Instantiate(orderPrefab, flatPos + spawnPos, Quaternion.identity);
        Compass.instance.UpdateObjectiveTransform(order.transform);

    }
    public void CompleteOrder()
    {
        SpawnOrder();
    }
}