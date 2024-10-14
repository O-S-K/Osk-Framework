using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OSK
{
    public class LeaderboardSystem : MonoBehaviour
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
            DisplayLeaderboard();
        }

        private void SortLeaderboard()
        {
            leaderboard.Sort((a, b) => b.score.CompareTo(a.score));
        }

        private void DisplayLeaderboard()
        {
            foreach (Transform child in leaderboardPanel)
            {
                Destroy(child.gameObject);
            }

            foreach (var entry in leaderboard)
            {
                GameObject entryGO = Instantiate(entryPrefab, leaderboardPanel);
                Text entryText = entryGO.GetComponent<Text>();
                entryText.text = $"{entry.playerName}: {entry.score}";
            }
        }
    }
}