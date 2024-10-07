using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace OSK
{
    public class QuestPopup : MonoBehaviour
    {
        public QuestItem QuestItemPrefab;
        public DataList DataList;

        public GameObject RewardClaimingPanel;
        public Image RewardClaimingIcon;
        public TextMeshProUGUI RewardClaimingCount;


        private void Start()
        {
            HideRewardClaiming();
        }

        public void Init()
        {
            this.Refresh();
        }
        
        public void Refresh()
        {
            _LoadItemsList();
        }

        private void _LoadItemsList()
        {
            if (DataList != null)
            {
                DataList.Clear();
                var options = new DataListOptions();
                var list = new List<UIBehaviourOptions>();
                
                foreach(QuestSystem.Quest quest in QuestSystem.Instance.quests)
                {
                    //if (!QuestManager.instance.IsQuestClaimed(quest.index))
                    //{
                        UIBehaviourOptions option = new UIBehaviourOptions();
                        option.index = quest.index;
                        list.Add(option);
                    //}
                }

                if (QuestItemPrefab != null)
                {
                    options.prefab = QuestItemPrefab;
                }

                options.list = list;
                DataList.SetData(options);
            }
        }

        public void Close()
        {
            DemoRewardSytem.instance.ShowQuestWindow(false);
            //Destroy(this.gameObject);
        }

        public void ClaimQuest(QuestItem questItem)
        {
            QuestSystem.Quest quest = questItem.GetQuest();
            if(quest != null)
            {
                QuestSystem.Instance.ClaimQuest(quest.index, true);
                StartCoroutine(_ShowRewardClaiming(quest.rewards));
                this.Refresh();
            }
        }

        public void GoQuest(QuestItem questItem)
        {
            QuestSystem.Quest quest = questItem.GetQuest();
            if (quest != null)
            {
                switch (quest.goal)
                {
                    case QuestSystem.QuestGoals.COLLECT_DAILY_REWARDS:
                        Debug.Log("COLLECT_DAILY_REWARDS");
                        Debug.Log("You can add your own logic here!");
                        //LOGIC
                        break;

                    case QuestSystem.QuestGoals.COMPLETE_MISSION:
                        Debug.Log("COMPLETE_MISSION");
                        Debug.Log("You can add your own logic here!");
                        //LOGIC
                        break;
                    case QuestSystem.QuestGoals.DESTROY_ENEMY:
                        Debug.Log("DESTROY_ENEMY");
                        Debug.Log("You can add your own logic here!");
                        //LOGIC
                        break;
                    case QuestSystem.QuestGoals.UPGRADE_HERO:
                        Debug.Log("UPGRADE_HERO");
                        Debug.Log("You can add your own logic here!");
                        //LOGIC
                        break;
                }
            }
        }

        private IEnumerator _ShowRewardClaiming(QuestSystem.RewardItem reward)
        {
            if (RewardClaimingPanel && reward != null)
            {
                RewardClaimingPanel.SetActive(true);

                if (reward != null)
                {
                    RewardClaimingIcon.sprite = reward.icon;
                    RewardClaimingCount.text = "x" + reward.count.ToString();
                }

                //RewardClaimingPanel.GetComponent<Animator>().Play("clip");
            }
            yield return new WaitForSeconds(1f);
            HideRewardClaiming();
            Debug.Log("Reward claimed. You can do your own logic here!");
        }

        public void HideRewardClaiming()
        {
            if (RewardClaimingPanel)
            {
                RewardClaimingPanel.SetActive(false);
            }
        }
    }
}
