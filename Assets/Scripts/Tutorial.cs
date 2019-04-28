using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public List<GameObject> TutorialTexts = new List<GameObject>();

    GameObject _panel;
    int _currentTextNum = 0;

    private void Awake()
    {
        Time.timeScale = 0.0f;
        _panel = TutorialTexts[0].transform.parent.gameObject;
    }

    public void ChangeText()
    {
        TutorialTexts[_currentTextNum].SetActive(false);
        _currentTextNum++;
        if (_currentTextNum > TutorialTexts.Count - 1)
        {
            _panel.SetActive(false);
            Time.timeScale = 1.0f;
        }
        else
        {
            TutorialTexts[_currentTextNum].SetActive(true);
        }
    }
}
