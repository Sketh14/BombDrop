#define MESH_GENERATION_TESTING

using UnityEngine;

namespace FrontLineDefense.Global
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
    public class ProceduralTerrain : MonoBehaviour
    {
        [SerializeField] private int xWidth = 100; // Terrain Width 
        [SerializeField] private int zDepth = 100; // Terrain depth

        [SerializeField] private float scale = 20f; // Sacle of the noise

        [SerializeField] private float heightMultiplier = 10f; // Multiplier to control mountain height
        [SerializeField] private string _generatedHash;

#if MESH_GENERATION_TESTING
        [SerializeField] private bool _meshGenerateRequested;
#endif
        // private byte[] _randomBytes;

        private Mesh mesh;
        private Vector2 offset;
        private MeshCollider meshCollider;

        public Vector3[] randomEnemyPositions;             //Debugging
        // private Vector3[] vertices;             //Debugging
        // [SerializeField] private int seed = 0;

        private void Start()
        {
            // GenerateHash();

            // if (seed == 0)
            //     seed = Random.Range(0, int.MaxValue); // Random seed if seed is 0
            // Random.InitState(seed);

            Random.InitState(_generatedHash.GetHashCode());
            offset = new Vector2(Random.Range(0f, 1000f), Random.Range(0f, 1000f));

            mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;
            meshCollider = GetComponent<MeshCollider>();

            CreateTerrain();
        }

#if MESH_GENERATION_TESTING
        private void FixedUpdate()
        {
            if (_meshGenerateRequested)
            {
                CreateTerrain();
                _meshGenerateRequested = false;
            }
        }
#endif

        private void GenerateHash()
        {
            // _randomBytes = new byte[6];

            byte[] _randomBytes = new byte[6];
            // using (var rng = new RNGCryptoServiceProvider())            
            //     rng.GetBytes(bytes);

            System.Random randomByteGenerator = new System.Random(System.DateTime.Now.Day + System.DateTime.Now.Hour
                                                 + System.DateTime.Now.Minute + System.DateTime.Now.Second
                                                 + System.DateTime.Now.Millisecond);

            randomByteGenerator.NextBytes(_randomBytes);

            // and if you need it as a string...
            // _generatedHash = System.BitConverter.ToString(_randomBytes);
            _generatedHash = System.BitConverter.ToString(_randomBytes).Replace("-", "");

            // or maybe...
            // string hash2 = System.BitConverter.ToString(_randomBytes).Replace("-", "").ToLower();
        }


        void CreateTerrain()
        {
            Vector3[] vertices = new Vector3[(xWidth + 1) * (zDepth + 1)];
            int[] triangles = new int[xWidth * zDepth * 6];
            Vector2[] uvs = new Vector2[vertices.Length];

            int enemyPosFilled = 0, fillInterval = 10;
            // Vector2[] 
            randomEnemyPositions = new Vector3[5];

            for (int z = 0, i = 0; z <= zDepth; z++)
            {
                for (int x = 0; x <= xWidth; x++, i++)
                {
                    // Calculate height using Perlin noise and the offset for a different map
                    float y = Mathf.PerlinNoise((x + offset.x) * scale * 0.1f, (z + offset.y) * scale * 0.1f) * heightMultiplier;
                    vertices[i] = new Vector3(x, y, z);
                    uvs[i] = new Vector2((float)x / xWidth, (float)z / zDepth);

                    if (z == 2                                          // In line with the player
                     && enemyPosFilled < 5 && fillInterval >= 5
                     && Random.Range(0f, 1f) <= 0.3f)                   //Can get rid of this part
                    {
                        fillInterval = 0;
                        randomEnemyPositions[enemyPosFilled] = vertices[i];             //We dont need z
                        enemyPosFilled++;
                    }

                    fillInterval++;
                }
            }

            int vert = 0, tris = 0;
            for (int z = 0; z < zDepth; z++)
            {
                for (int x = 0; x < xWidth; x++)
                {
                    triangles[tris + 0] = vert + 0;
                    triangles[tris + 1] = vert + xWidth + 1;
                    triangles[tris + 2] = vert + 1;
                    triangles[tris + 3] = vert + 1;
                    triangles[tris + 4] = vert + xWidth + 1;
                    triangles[tris + 5] = vert + xWidth + 2;

                    vert++;
                    tris += 6;
                }
                vert++;
            }

            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            mesh.RecalculateNormals();

            meshCollider.sharedMesh = null;
            meshCollider.sharedMesh = mesh;

            GameManager.Instance.OnMapGenerated?.Invoke(randomEnemyPositions);
        }
    }
}