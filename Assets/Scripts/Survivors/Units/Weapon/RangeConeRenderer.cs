using System;
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
        private const float DIRECTION_CORRECTION_ANGLE = 90f;
        
        [SerializeField] private Material _material;

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
            Build(130, 10);
        }

        public void Build(float angle, float radius)
        {
            CalculateSegments(angle);
            PrepareMeshData();
            CalculateVerts(angle, radius);
            CalculateTriangles();
            SetUvs();
            UpdateMesh();
        }

        private void CalculateSegments(float angle)
        {
            _segments = Mathf.Max(MIN_SEGMENTS_COUNT, angle / DEGREES_PER_SEGMENT);
            _segmentAngle = angle / _segments;
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

        private void CalculateVerts(float angle, float radius)
        {
            var currentAngle = DIRECTION_CORRECTION_ANGLE - angle / 2;
            for (int i = 1; i < _verts.Length; i += 3)
            {
                _verts[i] = GetVertPositionAtAngle(currentAngle, radius);
                currentAngle += _segmentAngle;
                _verts[i + 1] = GetVertPositionAtAngle(currentAngle, radius);
            }
        }

        private Vector3 GetVertPositionAtAngle(float angle, float radius)
        {
            var x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            var z = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
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