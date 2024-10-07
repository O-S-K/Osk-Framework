using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace OSK
{
    public class RewardItemComponent : MonoBehaviour
    {
        public Image Icon;
        public TextMeshProUGUI CountText;

        public void SetData(SpinWheelPopup.RewardItem reward)
        {
            this.Icon.sprite = reward.icon;
            this.CountText.text = reward.count.ToString();
        }

    }
}
