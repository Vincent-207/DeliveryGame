using System.Collections;
using System.Xml.Serialization;
using UnityEngine;

public class ChunkGen : MonoBehaviour
{
    public int chunkSize;
    public int mapLength;
    public GameObject chunkPrefab;
    [SerializeField]
    int seed;
    public void Generate()
    {
        DestroyChildren();
        seed = Random.Range(0, 100000);
        float starTime = Time.time;
        
        for(int x = 0; x < mapLength; x++)
        {
            for(int z = 0; z < mapLength; z++)
            {
                Vector3 offset = new(x, 0, z);
                offset *= chunkSize;
                IChunk chunk = Instantiate(chunkPrefab,transform.position + offset, Quaternion.identity, transform).GetComponent<IChunk>();
                Vector2 offsetPos = new(offset.x + transform.position.x, offset.z + transform.position.z);
                chunk.Init(offsetPos, seed);
                chunk.Generate();
            }
        }

        float elapsed = Time.fixedUnscaledTime - starTime;
        float ms = elapsed * 1000f;
        Debug.Log("Time to gen: " + (Time.deltaTime * 1000f) );
    }

    void DestroyChildren()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    
}
