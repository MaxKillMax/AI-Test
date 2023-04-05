using UnityEngine;
using UnityEngine.AI;

namespace AiTest.World
{
    [RequireComponent(typeof(EventCollector)), RequireComponent(typeof(Collider)), RequireComponent(typeof(MeshRenderer)), RequireComponent(typeof(NavMeshObstacle))]
    public class Door : MonoBehaviour
    {
        [SerializeField] private Material _openMaterial;
        [SerializeField] private Material _closeMaterial;

        private Collider _collider;
        private NavMeshObstacle _obstacle;
        private MeshRenderer _meshRenderer;

        private bool _isOpen = false;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _obstacle = GetComponent<NavMeshObstacle>();
            _meshRenderer = GetComponent<MeshRenderer>();
            GetComponent<EventCollector>().OnDowned += Switch;
        }

        private void Switch()
        {
            _isOpen = !_isOpen;
            _collider.isTrigger = _isOpen;
            _obstacle.enabled = !_isOpen;    
            _meshRenderer.material = _isOpen ? _openMaterial : _closeMaterial;
        }
    }
}
