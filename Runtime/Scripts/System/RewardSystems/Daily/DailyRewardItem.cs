using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace OSK
{
    public class DailyRewardItem : MonoBehaviour
    {
        public static DailyRewardItem Instance;

        public Image Icon;
        public TextMeshProUGUI CountText;

        public Image DayPanel;
        public GameObject GreenPanel;
        public GameObject Glow;
        public GameObject TickMark;

        [HideInInspector]
        public int day;

        public void SetData(DailyRewardsPopup.RewardData reward)
        {
            this.Icon.sprite = reward.icon;
            this.CountText.text = reward.count.ToString();
            this.day = reward.day;

            bool isReadyToCollect = RewardSytem.instance.dailyRewardsPopup.IsDailyRewardReadyToCollect(day);
            bool isClaimed = RewardSytem.instance.dailyRewardsPopup.IsDailyRewardClaimed(day);

            GreenPanel.SetActive(isReadyToCollect);
            Glow.SetActive(!isClaimed && Instance == this);

            DayPanel.color = isReadyToCollect && !isClaimed ? Color.green : Color.white;
            TickMark.SetActive(isClaimed);

            if (isReadyToCollect && !isClaimed)
                SetSelected(true);
        }

        public void SetSelected(bool isTrue)
        {
            if (this != Instance && Instance != null)
                Instance.SetSelected(false);
        
            Glow.SetActive(isTrue);

            if(isTrue)
                Instance = this;
        }

        //private bool _IsReadyToCollect()
        //{
        //    int loginDay = GetComponentInParent<DailyRewardsWindow>().GetDaysSinceSignUp();
        //    return (loginDay >= _day);
        //}

        //private bool _IsClaimed()
        //{
        //    return GetComponentInParent<DailyRewardsWindow>().IsDailyRewardClaimed(_day);
        //}

       
        public int GetDay()
        {
            return day;
        }

    }
}