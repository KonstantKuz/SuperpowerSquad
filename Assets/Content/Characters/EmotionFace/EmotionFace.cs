using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionFace : MonoBehaviour
{
    [SerializeField] private GameObject angry;
    [SerializeField] private GameObject damaged;
    [SerializeField] private GameObject defeated;

    private Dictionary<Emotion, GameObject> _emotions;

    private void Start()
    {
        SetupDictionary();
    }

    private void SetupDictionary()
    {
        _emotions = new Dictionary<Emotion, GameObject>
        {
            {Emotion.Angry, angry},
            {Emotion.Damaged, damaged}, 
            {Emotion.Defeated, defeated}
        };
    }

    public void SetEmotion(Emotion value)
    {
        foreach (var emotion in _emotions.Values)
        {
            emotion.SetActive(false);
        }
        _emotions[value].SetActive(true);
    }
}

public enum Emotion
{
    Angry,
    Damaged,
    Defeated,
}
