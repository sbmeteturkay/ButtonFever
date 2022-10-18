//Authored by saban mete turkay demirkiran
//follow: https://github.com/sbmeteturkay

using UnityEngine;
using System;
namespace MeteTurkay{
	public class ButtonObjectMove : ObjectMove
	{
		public static event Action<ButtonUnit,int> ButtonMove;
        [SerializeField] ButtonUnit buttonUnit;
        int area=0;
        public override void OnMouseUp()
        {
            ButtonMove?.Invoke(buttonUnit,area);
            base.OnMouseUp();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(searchTag))
            {
                area = int.Parse(other.gameObject.name);
            }
        }
    }
}
