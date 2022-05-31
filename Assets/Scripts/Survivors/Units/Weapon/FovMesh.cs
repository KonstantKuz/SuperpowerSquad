using Survivors.Extension;
using UnityEngine;

namespace Survivors.Units.Weapon
{
// https://forum.unity.com/threads/how-to-make-a-mesh-for-fov.152317/ 
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class FovMesh : MonoBehaviour
    {
        [SerializeField] private Material _material;
        private float _radius;
        private float _angle;

        private float _segments;
        private float _segmentAngle;
        private float _actualAngle;

        private Mesh _mesh;
        private Vector3[] _verts;
        private Vector3[] _normals;
        private int[] _triangles;
        private Vector2[] _uvs;

        void Start()
        {
            var MeshF = gameObject.RequireComponent<MeshFilter>();
            var MeshR = gameObject.RequireComponent<MeshRenderer>();

            MeshR.material = _material;
            _mesh = MeshF.mesh;

            //BUILD THE MESH
            buildMesh();
        }

        void buildMesh()
        {
            // Grab the Mesh off the gameObject
            //myMesh = gameObject.GetComponent<MeshFilter>().mesh;

            //Clear the mesh
            _mesh.Clear();

            // Calculate actual pythagorean angle
            _actualAngle = 90.0f - _angle;

            _segments = Mathf.Max(2, _angle / 2);
            // Segment Angle
            _segmentAngle = _angle * 2 / _segments;

            // Initialise the array lengths
            _verts = new Vector3[(int) _segments * 3];
            _normals = new Vector3[(int) _segments * 3];
            _triangles = new int[(int) _segments * 3];
            _uvs = new Vector2[(int) _segments * 3];

            // Initialise the Array to origin Points
            for (int i = 0; i < _verts.Length; i++)
            {
                _verts[i] = new Vector3(0, 0, 0);
                _normals[i] = Vector3.up;
            }

            // Create a dummy angle
            float a = _actualAngle;

            // Create the Vertices
            for (int i = 1; i < _verts.Length; i += 3)
            {
                _verts[i] = new Vector3(Mathf.Cos(Mathf.Deg2Rad * a) * _radius, // x
                    0, // y
                    Mathf.Sin(Mathf.Deg2Rad * a) * _radius); // z

                a += _segmentAngle;
                print(a);

                _verts[i + 1] = new Vector3(Mathf.Cos(Mathf.Deg2Rad * a) * _radius, // x
                    0, // y
                    Mathf.Sin(Mathf.Deg2Rad * a) * _radius); // z          
            }

            // Create Triangle
            for (int i = 0; i < _triangles.Length; i += 3)
            {
                _triangles[i] = 0;
                _triangles[i + 1] = i + 2;
                _triangles[i + 2] = i + 1;
            }

            // Generate planar UV Coordinates
            for (int i = 0; i < _uvs.Length; i++)
            {
                _uvs[i] = new Vector2(_verts[i].x, _verts[i].z);
            }

            // Put all these back on the mesh
            _mesh.vertices = _verts;
            _mesh.normals = _normals;
            _mesh.triangles = _triangles;
            _mesh.uv = _uvs;
        }
    }
}