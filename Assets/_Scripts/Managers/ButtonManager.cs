//Authored by saban mete turkay demirkiran
//follow: https://github.com/sbmeteturkay

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
namespace MeteTurkay{
	public class ButtonManager : MonoBehaviour
	{
		[System.Serializable]
		public class ButtonBuyArea
        {
			public Transform putLocation;
			public bool hasButton = false;
			public ButtonUnit buttonUnit;
        }
		[SerializeField] ButtonBuyArea[] buttonBuyAreas;
		//this array contains prefabs of every level of fever buttons index 0, level 1 button, index 2 level 4 button, 3 level 16 button
		[SerializeField] GameObject[] buttonLevelObjects;
		[SerializeField] GameObject buttonParents;

		[Header("UI")]
		[SerializeField] Button buyButton;
		public void BuyButton(int level=1,int _areaIndex=-1)
        {
			if (CanBuy())
			{
				int areaIndex = _areaIndex == -1 ? GetAvaibleBuyAreaIndex() : _areaIndex;
				var button = Instantiate(buttonLevelObjects[level-1], buttonParents.transform,true);
				ButtonBuyArea buttonBuyArea = buttonBuyAreas[areaIndex];
				button.transform.localPosition = buttonBuyArea.putLocation.position;
				buttonBuyArea.buttonUnit = button.GetComponent<ButtonUnit>();
				buttonBuyArea.buttonUnit.level = level;
				buttonBuyArea.buttonUnit.buyAreaIndex = areaIndex;
				buttonBuyArea.hasButton = true;
			}

        }
		public void BuyButtonUI()
        {
			BuyButton();
        }
        private void Start()
        {
            ButtonObjectMove.ButtonMove += ButtonObjectMove_ButtonMove; 
        }

        private void ButtonObjectMove_ButtonMove(ButtonUnit arg1, int arg2)
        {
            if (arg2 != -1)
            {
                if (buttonBuyAreas[arg2].hasButton)
                {
					print(CanMerge(buttonBuyAreas[arg2].buttonUnit.level, arg1.level));
                    if (CanMerge(buttonBuyAreas[arg2].buttonUnit.level, arg1.level))
                    {
						buttonBuyAreas[arg1.buyAreaIndex].hasButton = false;
						arg1.ButtonObjectMove.SetStartPosition(buttonBuyAreas[arg2].putLocation.position);
						this.Wait(1, () => { arg1.gameObject.SetActive(false); buttonBuyAreas[arg2].buttonUnit.gameObject.SetActive(false);
							MergeButton(arg1.level, arg2);
						});
					}
                }
                else
                {
					arg1.ButtonObjectMove.SetStartPosition(buttonBuyAreas[arg2].putLocation.position);
					buttonBuyAreas[arg1.buyAreaIndex].hasButton = false;
					arg1.buyAreaIndex = arg2;
					buttonBuyAreas[arg2].hasButton = true;
					buttonBuyAreas[arg2].buttonUnit = arg1;
                }
            }
        }

        private void Update()
        {
            buyButton.interactable = CanBuy();
        }

        private bool CanBuy()
        {
            return GetAvaibleBuyAreaIndex() != -1;
        }

        public void MergeButton(int level,int areaIndex)
        {
			BuyButton(level+1, areaIndex);
        }
		int GetAvaibleBuyAreaIndex()
        {
			for(int i = 0; i < buttonBuyAreas.Length; i++)
            {
				if (!buttonBuyAreas[i].hasButton)
					return i;
			}

			return -1;
		}

		bool CanMerge(int holdingButtonLevel, int targetButtonLevel)
		{
			return holdingButtonLevel == targetButtonLevel && targetButtonLevel< buttonLevelObjects.Length;
		}
	}
}
