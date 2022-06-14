using UnityEngine.AI;

namespace Survivors.Units.Enemy
{
    public static class NavMeshInitializer
    {
        private const float AVOIDANCE_PREDICTION_TIME = 0.5f;
        private const int PATHFINDING_ITERATIONS_PER_FRAME = 500;

        public static void Init()
        {
            NavMesh.avoidancePredictionTime = AVOIDANCE_PREDICTION_TIME;
            NavMesh.pathfindingIterationsPerFrame = PATHFINDING_ITERATIONS_PER_FRAME;
        }
    }
}