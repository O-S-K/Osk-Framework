using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OSK
{
    public class LeaderboardSystem : MonoBehaviour, IService
    {
        public List<LeaderboardEntry> leaderboard = new List<LeaderboardEntry>();
        public Transform leaderboardPanel;
        public GameObject entryPrefab;

        public void AddEntry(string playerName, int score)
        {
            LeaderboardEntry newEntry = new LeaderboardEntry
            {
                playerName = playerName,
                score = score
            };
            leaderboard.Add(newEntry);
            SortLeaderboard();
            DrawDisplay();
            //DisplayLeaderboard();
        }

        private void SortLeaderboard()
        {
            leaderboard.Sort((a, b) => b.score.CompareTo(a.score));
        }

        public void DisplayLeaderboard()
        {
            foreach (Transform child in leaderboardPanel)
            {
                GameObject.Destroy(child.gameObject);
            }

            foreach (var entry in leaderboard)
            {
                GameObject entryGO = GameObject.Instantiate(entryPrefab, leaderboardPanel);
                Text entryText = entryGO.GetComponent<Text>();
                entryText.text = $"{entry.playerName}: {entry.score}";
            }
        }

        public void DrawDisplay()
        {
            for (int i = 0; i < leaderboard.Count; i++)
            {
                Logg.Log($"Name {leaderboard[i].playerName} | {leaderboard[i].score}");
            }
        }
    }
}