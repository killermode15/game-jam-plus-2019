using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using VRTK;

public class GameSequence : MonoBehaviour
{
    public enum Sequence { START, GAME, END}

    public CheckpointManager CheckpointManager;
    public RadialSpawner RadialSpawner;
    public ScoreManager ScoreManager;
    public GameObject StartMenu;
    public TextMeshProUGUI EndScoreTxt;
    
    public Sequence CurrentSequence = Sequence.START;
    public bool Begin;  

    private bool reset;

    public bool GameOver()
    {
        return (CheckpointManager.AvailableTargets().Count <= 0);
    }
    
    private void Update()
    {
        Debug.Log(GameOver());
        UpdateSequence();
    }

    private void UpdateSequence()
    {
        switch (CurrentSequence)
        {
            case Sequence.START:    UpdateStartSequence();  break;
            case Sequence.GAME:     UpdateGameSequence();   break;
            case Sequence.END:      UpdateEndSequence();    break;
        }
    }

    private void UpdateStartSequence()
    {
        ScoreManager.Restart();
        ScoreManager.SetScore(0);
        RadialSpawner.CleanUpScene();

        StartMenu.SetActive(true);
        if (ScoreManager.GetScore >= 0)
        {
            EndScoreTxt.gameObject.SetActive(true);
        }

        if (Begin)
        {
            StartMenu.SetActive(false);
            EndScoreTxt.gameObject.SetActive(false);
            CurrentSequence = Sequence.GAME;
        }
    }

    private void UpdateGameSequence()
    {
        //start ze spawning
        RadialSpawner.StartSpawning();

        if (GameOver())
        {
            CurrentSequence = Sequence.END;
            reset = true;
        }
    }

    private void UpdateEndSequence()
    {
        //reset EVERYTHING
        if (reset)
        {
            RadialSpawner.StopSpawn();
            RadialSpawner.CleanUpScene();
            EndScoreTxt.text = "Killed " + ScoreManager.GetScore + " raiders before inevitable invasion";
            CheckpointManager.Restart();

            Begin = false;
            reset = false;

            CurrentSequence = Sequence.START;
        }
    }

    private IEnumerator EndSequence()
    {
        //show end score
        RadialSpawner.CleanUpScene();
        RadialSpawner.StopSpawn();
        EndScoreTxt.text = "Killed " + ScoreManager.GetScore + " raiders before inevitable invasion";
        CheckpointManager.Restart();

        yield return new WaitForEndOfFrame();
        reset = false;
    }
}
