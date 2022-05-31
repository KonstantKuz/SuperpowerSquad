using EasyButtons;
using Survivors.Extension;
using UnityEngine;

namespace Survivors.Units.Weapon
{
    // source https://forum.unity.com/threads/how-to-make-a-mesh-for-fov.152317/ 

    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class RangeConeRenderer : MonoBehaviour
    {
        private const int MIN_SEGMENTS_COUNT = 2;
        private const int DEGREES_PER_SEGMENT = 3;
        private const float VERTS_ANGLE_OFFSET = 90f;
        
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

        private void Awake()
        {
            var meshFilter = gameObject.RequireComponent<MeshFilter>();
            var meshRenderer = gameObject.RequireComponent<MeshRenderer>();

            meshRenderer.material = _material;
            _mesh = meshFilter.mesh;
        }

        // test creation
        [Button]
        private void CreateTest()
        {
            Build(10, 130);
        }

        public void Build(float radius, float angle)
        {
            if (radius == _radius || angle == _angle)
            {
                return;
            }
            
            _radius = radius;
            _angle = angle;
            
            CalculateSegments();
            PrepareMeshData();
            CalculateVerts();
            CalculateTriangles();
            SetUvs();
            UpdateMesh();
        }

        private void CalculateSegments()
        {
            _segments = Mathf.Max(MIN_SEGMENTS_COUNT, _angle / DEGREES_PER_SEGMENT);
            _segmentAngle = _angle / _segments;
        }

        private void PrepareMeshData()
        {
            _verts = new Vector3[(int) _segments * 3];
            _normals = new Vector3[(int) _segments * 3];
            _triangles = new int[(int) _segments * 3];
            _uvs = new Vector2[(int) _segments * 3];
            for (int i = 0; i < _verts.Length; i++)
            {
                _verts[i] = new Vector3(0, 0, 0);
                _normals[i] = Vector3.up;
            }
        }

        private void CalculateVerts()
        {
            var currentAngle = VERTS_ANGLE_OFFSET -_angle / 2;
            for (int i = 1; i < _verts.Length; i += 3)
            {
                _verts[i] = GetVertPositionAtAngle(currentAngle);
                currentAngle += _segmentAngle;
                _verts[i + 1] = GetVertPositionAtAngle(currentAngle);
            }
        }

        private Vector3 GetVertPositionAtAngle(float angle)
        {
            var x = Mathf.Cos(Mathf.Deg2Rad * angle) * _radius;
            var z = Mathf.Sin(Mathf.Deg2Rad * angle) * _radius;
            return new Vector3(x, 0, z);
        }

        private void CalculateTriangles()
        {
            for (int i = 0; i < _triangles.Length; i += 3)
            {
                _triangles[i] = 0;
                _triangles[i + 1] = i + 2;
                _triangles[i + 2] = i + 1;
            }
        }

        private void SetUvs()
        {
            for (int i = 0; i < _uvs.Length; i++)
            {
                _uvs[i] = new Vector2(_verts[i].x, _verts[i].z);
            }
        }

        private void UpdateMesh()
        {
            _mesh.Clear();
            _mesh.vertices = _verts;
            _mesh.normals = _normals;
            _mesh.triangles = _triangles;
            _mesh.uv = _uvs;
        }
    }
}