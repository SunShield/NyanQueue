using UnityEngine;
using Screen = NyanQueue.Core.ScreenSystem.Screens.Screen;

namespace NyanQueue.Core.ScreenSystem.Containers
{
    public class ScreenContainer : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        
        public int SortingOrder => _canvas.sortingOrder;
        
        public void Setup(int sortingOrder) => _canvas.sortingOrder = sortingOrder;

        public void AddScreen(Screen screen)
        {
            screen.gameObject.transform.SetParent(transform);
            screen.gameObject.transform.localPosition = Vector3.zero;
        }
    }
}