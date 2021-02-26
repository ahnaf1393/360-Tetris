using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreTable : MonoBehaviour
{

    private Transform entryContainer;
    private Transform entryTemplate;
    private Transform entryBackground;
    private void Awake()
    {
        entryContainer = transform.Find("HightScoreEntryContainer");
        entryTemplate = entryContainer.Find("HighScoreEntryTemplate");
        entryBackground = entryTemplate.Find("HighScoreEntryBackground");

        entryTemplate.gameObject.SetActive(false);

        List<ScoreManager.ScoreEntry> scoreList = ScoreManager.getInstance().scores;
        scoreList.Sort();

        float templateHeight = 25f;
        //entryBackground.GetComponent<RectTransform>().sizeDelta = new Vector2(640, templateHeight);
        for (int i = 0; i < scoreList.Count; i++)
        {
            Transform entryTransform = Instantiate(entryTemplate, entryContainer);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -(templateHeight * i));
            entryTransform.gameObject.SetActive(true);
            entryBackground.gameObject.SetActive(i % 2 == 1);

            int rank = i + 1;
            string rankString;

            switch(rank)
            {
                case 1:
                    rankString = rank + "ST";
                    entryTransform.Find("Trophy").GetComponent<Image>().color = new Color32(255, 215, 0, 255);
                    break;

                case 2:
                    rankString = rank + "ND";
                    entryTransform.Find("Trophy").GetComponent<Image>().color = new Color32(196, 202, 206, 255);
                    break;

                case 3:
                    rankString = rank + "RD";
                    entryTransform.Find("Trophy").GetComponent<Image>().color = new Color32(128, 74, 0, 255);
                    break;

                default:
                    rankString = rank + "TH";
                    entryTransform.Find("Trophy").gameObject.SetActive(false);
                    break;
            }

            entryTransform.Find("Position").GetComponent<Text>().text = rankString;

            int score = scoreList[i].score;
            entryTransform.Find("Score").GetComponent<Text>().text = score.ToString();

            string name = scoreList[i].name;
            entryTransform.Find("Name").GetComponent<Text>().text = name;
        }
    }
}
