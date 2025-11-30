using NyanQueue.Core.ScreenSystem.Screens;
using UnityEngine;

namespace NyanQueue.Core.ScreenSystem.Containers
{
    public class ScreenContainer : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        
        public int SortingOrder => _canvas.sortingOrder;
        
        public void Setup(int sortingOrder) => _canvas.sortingOrder = sortingOrder;

        public void AddScreen(AbstractScreen screen)
        {
            screen.gameObject.transform.SetParent(transform);
            screen.gameObject.transform.localPosition = Vector3.zero;
        }
    }
}