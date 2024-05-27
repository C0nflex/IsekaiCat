using Dan.Main;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LeaderboardCreatorDemo
{

    public class LeaderboardManager : MonoBehaviour
    {
        public static LeaderboardManager Instance;
        [SerializeField] private GameObject Entries;
        private List<TextMeshProUGUI> _ranks;
        private List<TextMeshProUGUI> _names;
        private List<TextMeshProUGUI> _scores;
        [SerializeField] TMP_InputField inputName;

        private string publicLeaderboardKey = "a499e40388e331aac9a7333bd7913def6add44409682ce7fce075df1cb360628";

        private static LeaderboardReference LeaderboardRefrence = new LeaderboardReference("a499e40388e331aac9a7333bd7913def6add44409682ce7fce075df1cb360628");

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _ranks = new List<TextMeshProUGUI>(Entries.transform.childCount);
            _names = new List<TextMeshProUGUI>(Entries.transform.childCount);
            _scores = new List<TextMeshProUGUI>(Entries.transform.childCount);
            for (int i = 0; i < Entries.transform.childCount; i++)
            {
                _ranks.Add(Entries.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>());
                _names.Add(Entries.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>());
                _scores.Add(Entries.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>());
            }
            GetLeaderboard();
        }

        public void GetLeaderboard()
        {
            LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, ((msg) =>
            {
                int leaderboardlength = Mathf.Min(msg.Length, _names.Count);
                for (int i = 0; i < leaderboardlength; i++)
                {
                    _ranks[i].text = msg[i].Rank.ToString() + ".";
                    _names[i].text = msg[i].Username;
                    _scores[i].text = msg[i].Score.ToString();
                }
                for (int i = leaderboardlength; i < _names.Count; i++)
                {
                    _ranks[i].text = (i + 1).ToString() + ".";
                }
            }), (msg) =>
            {
                Debug.Log("Leaderboard cannot be found");
            });
        }

        public void SetLeaderboardEntry(string username, int score)
        {
            LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, username, score, ((msg) =>
            {
                GetLeaderboard();
            }), (msg) =>
            {
                Debug.Log("Leaderboard not updated");
            });
        }

        public void SubmitScore() => SetLeaderboardEntry(inputName.text, 0);

        public void UpdateScore() => LeaderboardRefrence.GetPersonalEntry((entry) =>
        {
            if (entry.Rank == 0) //no entry uploaded
                SetLeaderboardEntry("guest" + Random.Range(1, 1000).ToString(), 0);
            else
                SetLeaderboardEntry(entry.Username, 0);
        });
    }
}
