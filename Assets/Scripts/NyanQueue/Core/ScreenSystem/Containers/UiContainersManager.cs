using System.Collections.Generic;
using UnityEngine;

namespace NyanQueue.Core.ScreenSystem.Containers
{
    public class UiContainersManager : MonoBehaviour
    {
        [SerializeField] private Transform _containersRoot;
        [SerializeField] private ScreenContainer _containerPrefab;
        
        private Dictionary<int, ScreenContainer> _containers = new();

        public ScreenContainer GetContainer(int order)
        {
            if (!_containers.ContainsKey(order))
            {
                var containerGo = Instantiate(_containerPrefab, _containersRoot);
                var containerComponent = containerGo.GetComponent<ScreenContainer>();
                containerComponent.Setup(order);
                _containers.Add(order, containerComponent);
            }
            
            return _containers[order];
        }
    }
}