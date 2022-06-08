using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace Survivors.UI.Components
{
    public class BezierFollower : MonoBehaviour
    {
        private const int CUBIC_BEZIER_CURVE_POINT_COUNT = 4;
        
        [SerializeField]
        private float _speed = 0.3f;
        [SerializeField]
        private Transform[] _routes;    
        [SerializeField]
        private Transform _target;

        private int _currentRouteIndex;
        private float _elapsedTime;
        private Coroutine _moveCoroutine;
        private void OnEnable()
        {
            Dispose();
            _moveCoroutine = StartCoroutine(MoveOnLoop());
        }

        private IEnumerator MoveOnLoop()
        {
            while (true) {
                yield return StartCoroutine(MoveOnLoopSegment());
                _currentRouteIndex++;
                if (_currentRouteIndex >= _routes.Length) {
                    _currentRouteIndex = 0;
                }
            }
        }

        private IEnumerator MoveOnLoopSegment()
        {
            Assert.IsTrue(_routes[_currentRouteIndex].transform.childCount >= CUBIC_BEZIER_CURVE_POINT_COUNT);
            var point0 = GetRoutePoint(_currentRouteIndex, 0);
            var point1 = GetRoutePoint(_currentRouteIndex, 1);
            var point2 = GetRoutePoint(_currentRouteIndex, 2);
            var point3 = GetRoutePoint(_currentRouteIndex, 3);

            _elapsedTime = 0f;

            while (_elapsedTime < 1) {
                _elapsedTime += Time.fixedDeltaTime * _speed;
                _target.transform.position = CalculateCubicBezierCurve(point0, point1, point2, point3, _elapsedTime);
                yield return null;
            }
        }
        private Vector3 GetRoutePoint(int routeIndex, int pointIndex) => _routes[routeIndex].GetChild(pointIndex).position;

        private Vector3 CalculateCubicBezierCurve(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float time)
        {
            return Mathf.Pow(1 - time, 3) * p0 + 3 * Mathf.Pow(1 - time, 2) * time * p1 + 3 * (1 - time) * Mathf.Pow(time, 2) * p2
                   + Mathf.Pow(time, 3) * p3;
        }
        private void Dispose()
        {
            if (_moveCoroutine == null) {
                return;
            }
            StopCoroutine(_moveCoroutine);
            _moveCoroutine = null;
        }
        private void OnDisable()
        {
            Dispose();
        }
    }
}