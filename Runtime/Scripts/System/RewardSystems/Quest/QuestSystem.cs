using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public class QuestSystem : MonoBehaviour, IService
    {
        public static QuestSystem Instance;

        public enum QuestGoals
        {
            COMPLETE_MISSION,
            DESTROY_ENEMY,
            UPGRADE_HERO,
            COLLECT_DAILY_REWARDS
        }

        [System.Serializable]
        public class Quest
        {
            public int index;
            public QuestGoals goal;
            public int maxValue;
            public string description;
            public RewardItem rewards;
        }

        [System.Serializable]
        public class RewardItem
        {
            public enum Type
            {
                COIN,
                GEM,
                BLUE_STONE,
                KEY
            }

            public Type type;
            public Sprite icon;
            public int count;
        }
        public Quest[] quests;

        void Awake()
        {
            Instance = this;
        }

        public int GetQuestValue(int index)
        {
            string key = "quest_value_" + index;
            return PlayerPrefs.GetInt(key, 0);
        }

        public void SetQuestValue(int index, int value)
        {
            string key = "quest_value_" + index;
            PlayerPrefs.SetInt(key, value);
        }

        public bool IsQuestClaimed(int index)
        {
            string key = "quest_claimed_" + index;
            return (PlayerPrefs.GetInt(key, 0) == 1);
        }

        public void ClaimQuest(int index, bool isTrue)
        {
            string key = "quest_claimed_" + index;
            PlayerPrefs.SetInt(key, isTrue?1:0);
        }

        public void OnAchieveQuestGoal(QuestGoals goal)
        {
            foreach(Quest quest in quests)
            {
                if(quest.goal == goal)
                {
                    int currentVal = GetQuestValue(quest.index);
                    currentVal++;
                    currentVal = Mathf.Clamp(currentVal, 0, quest.maxValue);
                    SetQuestValue(quest.index, currentVal);
                }
            }
        }

       
        public void ResetAllDailyQuests()
        {
            //int index = 0;
            foreach (Quest quest in this.quests)
            {
                SetQuestValue(quest.index, 0);
                ClaimQuest(quest.index, false);
            }
        }
    }
}
