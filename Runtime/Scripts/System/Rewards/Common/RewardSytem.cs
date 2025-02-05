using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace OSK
{
    public class RewardSytem : MonoBehaviour
    {
        public static RewardSytem instance;

        public GameObject MainMenu;
        public DailyRewardsPopup dailyRewardsPopup;
        public QuestPopup questPopup;
        public SpinWheelPopup SpinWheelWindow;


        void Awake()
        {
            instance = this;

            ShowDailyRewardsWindow(false);
            ShowQuestWindow(false);
            ShowSpinWheelWindow(false);
            ShowMainMenu(true);
        }

        public void OnClickDailyRewardsButton()
        {
            ShowDailyRewardsWindow(true);
            ShowQuestWindow(false);
            ShowSpinWheelWindow(false);
            ShowMainMenu(false);
        }

        public void OnClickQuestButton()
        {
            ShowQuestWindow(true);
            ShowDailyRewardsWindow(false);
            ShowSpinWheelWindow(false);
            ShowMainMenu(false);
        }

        public void OnClickSpinwheelButton()
        {
            ShowQuestWindow(false);
            ShowDailyRewardsWindow(false);
            ShowSpinWheelWindow(true);
            ShowMainMenu(false);
        }


        public void ShowMainMenu(bool isTrue)
        {
            if (MainMenu)
            {
                MainMenu.gameObject.SetActive(isTrue);
            }
        }



        //DAILY REWARDS OPTIONS
        public void ShowDailyRewardsWindow(bool isTrue)
        {
            if (dailyRewardsPopup)
            {
                dailyRewardsPopup.gameObject.SetActive(isTrue);

                if (isTrue)
                    dailyRewardsPopup.Init();
                else
                    ShowMainMenu(true);
            }
        }

        public void OnClickResetDailyRewardsButton()
        {
            PlayerPrefs.DeleteAll();
            dailyRewardsPopup.Init();
        }

        public void OnClickNextButton()
        {
            int currentDay = dailyRewardsPopup.GetDaysSinceSignUp();
            var signTime = DateTime.Now - new TimeSpan((currentDay + 1) * 24, 0, 0);
            PlayerPrefs.SetString("sign_up_time", signTime.ToString());
            dailyRewardsPopup.Init();
        }


        //QUESTS OPTIONS
        public void ShowQuestWindow(bool isTrue)
        {
            if (questPopup)
            {
                questPopup.gameObject.SetActive(isTrue);

                if (isTrue)
                    questPopup.Init();
                else
                    ShowMainMenu(true);
            }
        }

        public void OnClickFinishMission()
        {
            QuestSystem.Instance.OnAchieveQuestGoal(QuestSystem.QuestGoals.COMPLETE_MISSION);
        }

        public void OnClickUpgradeHero()
        {
            QuestSystem.Instance.OnAchieveQuestGoal(QuestSystem.QuestGoals.UPGRADE_HERO);
        }

        public void OnClickKillEnemy()
        {
            QuestSystem.Instance.OnAchieveQuestGoal(QuestSystem.QuestGoals.DESTROY_ENEMY);
        }

        public void OnClickCollectDailyRewards()
        {
            ShowDailyRewardsWindow(true);
        }

        public void OnClickResetQuest()
        {
            QuestSystem.Instance.ResetAllDailyQuests();
        }

        //SPINWHEEL OPTIONS
        public void ShowSpinWheelWindow(bool isTrue)
        {
            if (SpinWheelWindow)
            {
                SpinWheelWindow.gameObject.SetActive(isTrue);

                if (isTrue)
                    SpinWheelWindow.Init();
                else
                    ShowMainMenu(true);
            }
        }
    }
}