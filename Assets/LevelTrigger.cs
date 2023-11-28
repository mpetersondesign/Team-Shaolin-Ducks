using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TriggerType { Start, End }
public enum GameLevel { Jason, Matthew, Matt }
public class LevelTrigger : MonoBehaviour
{
    public TriggerType Type;
    public GameLevel Level;
    public LevelTrigger StartTrigger;
    public LevelTrigger EndTrigger;
    public bool LevelStarted = false;
    public Vector3 PlayerReturnPos;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !LevelStarted && Type == TriggerType.Start)
        {
            LevelStarted = true;
            EndTrigger.LevelStarted = true;
            FindObjectOfType<GameSystems>().StartTimer(Level);
            FindObjectOfType<GameSystems>().AreaChange("Jason's Level");
        }

        if (collision.tag == "Player" && LevelStarted && Type == TriggerType.End)
        {
            FindObjectOfType<GameSystems>().EndTimer(Level);
            StartTrigger.LevelStarted = false;
            FindObjectOfType<SceneFader>().FadeOut();
            FindObjectOfType<PlayerController>().CanMove = false;
            Invoke("ReturnPlayer", 1.5f);
        }
    }

    public void ReturnPlayer()
    {
        FindObjectOfType<SceneFader>().FadeIn();
            FindObjectOfType<GameSystems>().AreaChange("Hub Area");
        FindObjectOfType<PlayerController>().transform.position = PlayerReturnPos;
        FindObjectOfType<PlayerController>().CanMove = true;
    }

    private void OnDrawGizmos()
    {
        if (PlayerReturnPos != null)
            Gizmos.DrawSphere(PlayerReturnPos, 1f);
    }
}
