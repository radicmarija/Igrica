﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] List<AudioClip> _audioClips;

    public char Letter = 'a';

    int _correctAnswers = 5;
    int _correctClicks;

    public static GameController Instance { get; private set; }

    AudioSource _audioSource;

    void Awake() 
    {
        Instance = this;
        _audioSource = GetComponent<AudioSource>();
    }
    

     void OnEnable()
    {
        GenerateBoard();
        UpdateDisplayLetters();
    }

     void GenerateBoard()
    {
        var clickables = FindObjectsOfType<ClickableLetter>();
        
        List<char> charsList = new List<char>();

        for (int i = 0; i < _correctAnswers; i++)
            charsList.Add(Letter);

        for(int i = _correctAnswers; i < clickables.Length; i++)
        {
            var chosenLetter = ChooseInvalidRandomLetter();
            charsList.Add((char)chosenLetter);
        }

        charsList = charsList
            .OrderBy(t => Random.Range(0, 10000))
            .ToList();

        for(int i = 0; i < clickables.Length; i++)
        {
            clickables[i].SetLetter(charsList[i]);
        }
        FindObjectOfType<RemainingCounterText>().SetRemaining(_correctAnswers - _correctClicks);
    }

    internal void HandleCorrecrLetterClick(bool upperCase)
    {
        var clip = _audioClips.FirstOrDefault(t => t.name == Letter.ToString() + Letter.ToString());
        if (upperCase)
        {
            clip = _audioClips.FirstOrDefault(t => t.name == Letter.ToString());
        }
        _audioSource.PlayOneShot(clip);

        _correctClicks++;
        FindObjectOfType<RemainingCounterText>().SetRemaining(_correctAnswers - _correctClicks);
        if(_correctClicks >= _correctAnswers)
        {
            Letter++;
            UpdateDisplayLetters();
            _correctClicks = 0;
            GenerateBoard();
        }
    }

    private void UpdateDisplayLetters()
    {
        foreach (var displayletter in FindObjectsOfType<DisplayLetter>())
        {
            displayletter.SetLetter(Letter);
        }
    }

    private object ChooseInvalidRandomLetter()
    {
        int a = Random.Range(0, 26);
        var randomLetter = (char)('a' + a);

        while (randomLetter == Letter)
        {
            a = Random.Range(0, 26);
            randomLetter = (char)('a' + a);
        }
        return randomLetter;
    }

}