using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class RollerAcademy : Academy
{
    [Header("This Project")]
    public float MapDifficulty = 0f;

    public override void AcademyReset()
    {
        base.AcademyReset();
        MapDifficulty = resetParameters["map_difficulty"];

    }
}
