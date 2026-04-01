using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class ChunkGen : MonoBehaviour
{
    public int chunkSize;
    public int mapLength;
    public GameObject chunkPrefab;
    [SerializeField]
    int seed;
    [SerializeField] Transform player;
    public float spawnRadius;
    List<GameObject> chunks;
    Vector3 previousPos;
    Dictionary<Vector2Int, bool> loadedChunks;
    void Awake()
    {
        if(chunks == null) chunks = new();

        int childCount = transform.childCount;
        for(int i = 0; i < childCount; i++)
        {
            chunks.Add(transform.GetChild(i).gameObject);
        }

        loadedChunks ??= new();
    }
    void Update()
    {
        // Destroy chunks that are too far.
        // DestroyTooFar();
        // Spawn chunks that aren't close enough.
        // SpawnNewChunks();
        Generate();
        DestroyTooFar();
        previousPos = player.position;
    }

    void SpawnNewChunks()
    {
        Vector3Int previousChunk = posToChunkPos(previousPos);
        Vector3Int playerChunk = posToChunkPos(player.position);
        Debug.DrawLine(player.position, playerChunk, Color.blue);
        Debug.DrawLine(player.position, previousChunk, Color.red);

        // If player is in new chunk.
        if(previousChunk != playerChunk)
        {
            Vector3Int toGenDirection = playerChunk - previousChunk;
            toGenDirection /= 10;
            Debug.Log("New chunk! " + toGenDirection);
            Vector3 playerRelativeToGridPos = transform.position - player.position;
            Vector3Int playerGridPos = posToChunkPos(playerRelativeToGridPos);
            playerGridPos /= -10; 
            Vector3Int toGen = playerGridPos + toGenDirection * (mapLength/2);
            // toGen *= mapLength;
            GenerateChunkRow(toGen.x, toGen.z);
            // TODO ONLY SPAWN WHEN CHUNKS ARE UNLOADED
            
            // Get offset
            // 
            
            // SpawnChunk(toGenDirection.x, toGenDirection.z);
            
        }


        // Draw ray to current chunk.
    }

    public void GenerateChunkRow(int x, int z)
    {
        for(int i = 0; i < mapLength; i++)
        {
            // Spawn from left to right
            SpawnChunk(x - mapLength/2 + i, z);
            SpawnChunk(x, z - mapLength/2 + i);
            // spawn from bottom to top


            // Vector3Int playerPos = new Vector3Int((int) player.position.x, 0, (int) player.position.z);
            // Vector3Int spawnPos = playerPos + new Vector3Int(x * chunkSize, 0, i * chunkSize);

        }
    }
    public Vector3Int posToChunkPos(Vector3 pos, out Vector2Int output)
    {
        Vector3Int gridPos = posToChunkPos(pos);
        output = new (gridPos.x, gridPos.z);
        return gridPos;
    }
    public Vector3Int posToChunkPos(Vector3 pos)
    {
        // convert world pos to chunk pos. 
        pos.y = 0;
        pos /= 10f;
        Vector3Int chunkPos = new Vector3Int((int) pos.x, 0, (int) pos.z) * 10;
        return chunkPos;
    }

    bool IsChunkLoaded(int x, int z)
    {
        bool output = false;
        bool doesExist = loadedChunks.TryGetValue(new Vector2Int(x, z), out output);
        if(!doesExist) return false;
        return output;
    }

    void SpawnChunk(int x, int z)
    {
        loadedChunks ??= new();
        chunks ??= new();
        Debug.Log("Trying to spawn chunk");
        if(IsChunkLoaded(x, z))
        {
            Debug.LogWarning("Chunk already loaded!");
            return;
        }
        Debug.Log("Spawning");
        
        loadedChunks ??= new();    
        Vector3 offset = new(x, 0, z);
        offset *= chunkSize;
        GameObject generated = Instantiate(chunkPrefab,transform.position + offset, Quaternion.identity, transform);
        IChunk chunk = generated.GetComponent<IChunk>();
        Vector2 offsetPos = new(offset.x + transform.position.x, offset.z + transform.position.z);
        Vector2Int offsetCasted = Vector2Int.CeilToInt(offsetPos);
        chunk.Init(offsetCasted, seed);
        chunk.Generate();
        if(chunks == null) chunks = new();
        chunks.Add(generated);
        loadedChunks[new Vector2Int(x, z)] = true;
    }

    void DestroyTooFar()
    {
        int chunksCount = chunks.ToArray().Length;
        GameObject[] chunksArr = chunks.ToArray();
        for(int i = 0; i < chunksCount; i++)
        {
            // TODO calc only 2d distance
            // calc is short for calculate btw
            GameObject chunk = chunksArr[i];
            
            Vector3 toChunk = chunk.transform.position - player.transform.position;
            toChunk.y = 0;
            // Debug.DrawRay(player.position, toChunk);
            float maximumDistance = (mapLength * chunkSize * 4f);

            if(toChunk.sqrMagnitude > maximumDistance * maximumDistance)
            {
                // chunk out of bounds, destroying
                UnloadChunk(chunk);
            }
        }
    }

    void UnloadChunk(GameObject chunk)
    {
        // convert to grid pos int x, int z
        Vector2Int gridPos;
        posToChunkPos(chunk.transform.position, out gridPos);

        // update list
        chunks.Remove(chunk);    
        // update dict
        loadedChunks.Remove(gridPos);
        loadedChunks.Add(gridPos, false);
        // destroy
        Destroy(chunk);

    }
    public void Generate()
    {
        seed = Random.Range(0, 100000);
        // DestroyAllChildren();
        float startTime = Time.time;
        
        for(int x = 0; x < mapLength; x++)
        {
            for(int z = 0; z < mapLength; z++)
            {
                SpawnChunk(x, z);
            }
        }

        float elapsed = Time.fixedUnscaledTime - startTime;
        float ms = elapsed * 1000f;
        Debug.Log("Time to gen: " + (Time.deltaTime * 1000f) );
    }

    public void DestroyAllChildren()
    {
        GameObject[] children = new GameObject[transform.childCount];
        for(int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i).gameObject;
        }
        if(Application.isPlaying)
        {
            for(int i = 0; i < children.Length; i++)
            {
                Destroy(children[i]);
            }
        }
        else if(Application.isEditor)
        {
            for(int i = 0; i < children.Length; i++)
            {
                DestroyImmediate(children[i]);
            }
        }

        chunks ??= new();
        loadedChunks ??= new();

        chunks.Clear();
        loadedChunks.Clear();
        
    }

    
}
