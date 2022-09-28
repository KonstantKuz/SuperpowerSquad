using System;
using EasyButtons;
using Feofun.Extension;
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
        
        [SerializeField] private Material _material;

        private Mesh _mesh;
        
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
            var segmentsCount = CalculateSegments(angle);

            _mesh.Clear();
            _mesh.vertices = BuildVertices(segmentsCount, angle, radius);
            _mesh.triangles = BuildTriangles(segmentsCount);
            _mesh.normals = BuildNormals(segmentsCount);
            _mesh.uv = BuildUvs(_mesh.vertices);
        }

        private int CalculateSegments(float angle)
        {
            return (int) Mathf.Max(MIN_SEGMENTS_COUNT, angle / DEGREES_PER_SEGMENT);
        }

        private Vector3[] BuildVertices(int segmentsCount, float angle, float radius)
        {
            var vertices = new Vector3[segmentsCount * 3];
            var segmentAngle = angle / segmentsCount;
            var currentAngle = -angle / 2;
            for (int i = vertices.Length - 1; i >= 0; i -= 3)
            {
                vertices[i] = GetVertPositionAtAngle(currentAngle, radius);
                currentAngle += segmentAngle;
                vertices[i - 1] = GetVertPositionAtAngle(currentAngle, radius);
            }
            return vertices;
        }

        private Vector3 GetVertPositionAtAngle(float angle, float radius)
        {
            var x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            var z = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            return new Vector3(x, 0, z);
        }

        private int[] BuildTriangles(int segmentsCount)
        {
            var triangles = new int[segmentsCount * 3];
            for (int i = 0; i < triangles.Length; i += 3)
            {
                triangles[i] = 0;
                triangles[i + 1] = i + 2;
                triangles[i + 2] = i + 1;
            }
            return triangles;
        }

        private Vector3[] BuildNormals(int segmentsCount)
        {
            var normals = new Vector3[segmentsCount * 3];
            for (int i = 0; i < normals.Length; i++)
            {
                normals[i] = Vector3.up;
            }
            return normals;
        }

        private Vector2[] BuildUvs(Vector3[] vertices)
        {
            var uvs = new Vector2[vertices.Length];
            for (int i = 0; i < uvs.Length; i++)
            {
                uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
            }
            return uvs;
        }
    }
}