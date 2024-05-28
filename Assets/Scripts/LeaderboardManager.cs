using Dan.Main;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LeaderboardCreatorDemo
{

    public class LeaderboardManager : MonoBehaviour
    {
        private int MaxVal = 2048000000;
        public static LeaderboardManager Instance;
        [SerializeField] private GameObject Entries;
        private List<TextMeshProUGUI> _ranks;
        private List<TextMeshProUGUI> _names;
        private List<TextMeshProUGUI> _scores;
        [SerializeField] TMP_InputField inputName;

        private string publicLeaderboardKey = "c0d06ae9cc9b66f6df8ce01bc9ff2b7adab814a38dd7e7c05b1c690189cee097";

        private static LeaderboardReference LeaderboardRefrence = new LeaderboardReference("c0d06ae9cc9b66f6df8ce01bc9ff2b7adab814a38dd7e7c05b1c690189cee097");

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
                    _scores[i].text = msg[i].Extra;
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

        public void SetLeaderboardEntry(string username, float scoreForOrderingMechanism ,string Time)
        {
            LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, username, (int)scoreForOrderingMechanism, Time, ((msg) =>
            {
                GetLeaderboard();
            }), (msg) =>
            {
                Debug.Log("Leaderboard not updated");
            });
        }

        public void SubmitScore() => SetLeaderboardEntry(inputName.text, 2048000000 - 1000 * Timer.Instance.Recordtime +1f, Timer.Instance.RecordTimeInString);

        public void UpdateScore() => LeaderboardRefrence.GetPersonalEntry((entry) =>
        {
            if (entry.Rank == 0) //no entry uploaded
                SetLeaderboardEntry("guest" + Random.Range(1, 1000).ToString(), 2048000000 - 1000 *  Timer.Instance.Recordtime, Timer.Instance.RecordTimeInString);
            else
                SetLeaderboardEntry(entry.Username, 2048000000 - 1000 * Timer.Instance.Recordtime, Timer.Instance.RecordTimeInString);
        });
    }
}
