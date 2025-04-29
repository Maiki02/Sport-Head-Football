using Unity.VisualScripting;
using UnityEngine;

public class GameSoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundMusicSource;
    [SerializeField] private AudioSource ballBounceSource;
    [SerializeField] private AudioSource whistleStartSource;
    [SerializeField] private AudioSource whistleEndSource;
    [SerializeField] private AudioSource goalSource;

    [SerializeField] private AudioSource jumpSource;


    public void PlayBackgroundMusic(){
        backgroundMusicSource.Play();
    }
    public void StopBackgroundMusic(){
        backgroundMusicSource.Stop();
    }

    public void PlayBallBounce(){
        ballBounceSource.PlayOneShot(ballBounceSource.clip);
    }
    public void PlayWhistleStart(){
        whistleStartSource.PlayOneShot(whistleStartSource.clip);
    }
    public void PlayWhistleEnd(){
        whistleEndSource.PlayOneShot(whistleEndSource.clip);
    }
    public void PlayGoalSound(){
        goalSource.PlayOneShot(goalSource.clip);
    }

    public void PlayJumpSound(){
        jumpSource.PlayOneShot(jumpSource.clip);
    }

}