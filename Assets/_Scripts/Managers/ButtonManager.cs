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

        private void ButtonObjectMove_ButtonMove(ButtonUnit buttonUnit, int buttonAreaIndex)
        {
			print(buttonBuyAreas[buttonAreaIndex].buttonUnit != buttonUnit);
            if (buttonAreaIndex != -1&& buttonBuyAreas[buttonAreaIndex].buttonUnit!=buttonUnit)
            {
                if (buttonBuyAreas[buttonAreaIndex].hasButton)
                {
					print(CanMerge(buttonBuyAreas[buttonAreaIndex].buttonUnit.level, buttonUnit.level));
                    if (CanMerge(buttonBuyAreas[buttonAreaIndex].buttonUnit.level, buttonUnit.level))
                    {
						buttonBuyAreas[buttonUnit.buyAreaIndex].hasButton = false;
						buttonBuyAreas[buttonUnit.buyAreaIndex].buttonUnit = null;
						buttonUnit.ButtonObjectMove.SetStartPosition(buttonBuyAreas[buttonAreaIndex].putLocation.position);

						this.Wait(1, () => {
							buttonUnit.gameObject.SetActive(false); 
							buttonBuyAreas[buttonAreaIndex].buttonUnit.gameObject.SetActive(false);
							MergeButton(buttonUnit.level, buttonAreaIndex);
						});
					}
                }
                else
                {
					buttonUnit.ButtonObjectMove.SetStartPosition(buttonBuyAreas[buttonAreaIndex].putLocation.position);
					buttonBuyAreas[buttonUnit.buyAreaIndex].hasButton = false;
					buttonBuyAreas[buttonUnit.buyAreaIndex].buttonUnit = null;
					buttonUnit.buyAreaIndex = buttonAreaIndex;
					buttonBuyAreas[buttonAreaIndex].hasButton = true;
					buttonBuyAreas[buttonAreaIndex].buttonUnit = buttonUnit;
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
