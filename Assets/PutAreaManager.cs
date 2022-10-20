//Authored by saban mete turkay demirkiran
//follow: https://github.com/sbmeteturkay

using UnityEngine;

namespace MeteTurkay{
	public class PutAreaManager : MonoBehaviour
	{
        [System.Serializable]
        class ButtonShape
        {
            public int x = 1;
            public int y = 1;
            ButtonShape(int _x,int _y )
            {
                x = _x;
                y = _y;
            }
        }
        [SerializeField] LayerMask putLayer;
        [SerializeField] float yPosition = 0.2f;
        //Set this from scene
        [SerializeField] ButtonShape[] buttonShapes;
        Camera main;
        private void Start()
        {

            main = Camera.main;   
        }
        private void Update()
        {
            Cast();
        }
        private Vector3 Cast()
        {
            Ray ray = main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, putLayer))
                if (hit.collider != null)
                {
                    print("x: "+hit.collider.gameObject.name+" y: "+hit.collider.gameObject.transform.parent.name);
                    return new Vector3(hit.point.x, yPosition, hit.point.z);
                }
            return new Vector3(0, 0, 0);
        }
        bool CanPutButton(ButtonShape buttonShape) {
            return true;
        }
    }
}
