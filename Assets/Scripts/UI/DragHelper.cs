using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.UI
{
    public class DragHelper : MonoBehaviour
    {
        [SerializeField]
        private VisualElement dragObject;
        private MouseDownEvent downEvent;


        public void StartDrag(VisualElement vis)
        {
            dragObject = vis;
            dragObject.style.position = Position.Absolute;
        }

        public void Drag(VisualElement vis, MouseMoveEvent m)
        {
            if (dragObject == null)
                dragObject = vis;

            dragObject.style.position = Position.Absolute;
            dragObject.transform.position = m.mousePosition;
        }


        private void Update()
        {
            //if (dragObject != null)
            //{
            //    if (!Input.GetMouseButton(0))
            //        dragObject = null;
            //    else
            //    {
            //        Debug.Log("Position");
            //        dragObject.transform.position = downEvent.mousePosition;
                    
            //    }
            //} 
                
            
        }


    }
}
