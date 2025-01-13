using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalData : MonoBehaviour
{
    public static GlobalData Instance { private set; get; }

    public Blackboard globalBlackboard;

    private List<Guard> guardsThatSeePlayer;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        globalBlackboard = new Blackboard();
        globalBlackboard.SetVariable(GlobalVariableNames.GUARD_SEES_PLAYER, false);

        guardsThatSeePlayer = new List<Guard>();
    }

    //Not the best way to handle player detection, but it works.
    public void GuardSeesPlayer(Guard guard)
    {
        if(!guardsThatSeePlayer.Contains(guard)) guardsThatSeePlayer.Add(guard);

        globalBlackboard.SetVariable(GlobalVariableNames.GUARD_SEES_PLAYER, true);
    }

    public void GuardLostPlayer(Guard guard)
    {
        if(guardsThatSeePlayer.Contains(guard)) guardsThatSeePlayer.Remove(guard);

        globalBlackboard.SetVariable(GlobalVariableNames.GUARD_SEES_PLAYER, guardsThatSeePlayer.Count > 0);
    }


}
