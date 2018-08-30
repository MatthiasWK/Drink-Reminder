using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Variables", menuName = "VariableContainer")]
public class VariableContainer : ScriptableObject {

    public int Rows;
    public int Columns;

    public float AnimationDuration;

    public float MoveAnimationMinDuration;

    public float ExplosionDuration;

    public float WaitBeforePotentialMatchesCheck;
    public float OpacityAnimationFrameDelay;

    public int MinimumMatches;
    public int MinimumMatchesForBonus;

    public int Match3Score;
    public int SubsequentMatchScore;
}
