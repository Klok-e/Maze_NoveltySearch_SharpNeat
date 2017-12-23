using UnityEngine;

namespace Scripts.Agent.Abstract
{
    [RequireComponent(typeof(MoveController))]
    public abstract class AAgentController : MonoBehaviour
    {
        [SerializeField] protected ScriptableObjects.Containers.FloatReference _movingSpeed;
        [SerializeField] protected ScriptableObjects.Containers.FloatReference _rotationSpeed;

        public void Tick()
        {
            DoAction();
        }

        protected abstract void DoAction();

        public abstract void Reset(Vector3 defaultPos);
    }
}
