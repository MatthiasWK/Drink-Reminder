using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


    public static class Constants
    {
        public static int Rows = 6;
        public static int Columns = 6;

        public static int NumShapes = 5;

        public static int ShapeTheme = 0;

        public static int Background = 0;

        public static Boolean CustomBackgrounds = false;

        public static Boolean BackgroundsChanged = true;

        public static Boolean CustomShapes = false;

        public static Boolean ShapesChanged = true;

        public static string BackgroundPath = "Backgrounds";

        public static float ReminderTime = 30.0f;
        public static float TimeLeft;

        public static readonly float AnimationDuration =  0.2f;

        public static readonly float MoveAnimationMinDuration = 0.05f;

        public static readonly float ExplosionDuration = 0.3f;

        public static readonly float WaitBeforePotentialMatchesCheck = 2f;
        public static readonly float OpacityAnimationFrameDelay = 0.05f;

        public static readonly int MinimumMatches = 3;
        public static readonly int MinimumMatchesForBonus = 4;

        public static readonly int Match3Score = 60;
        public static readonly int SubsequentMatchScore = 1000;
        public static readonly int WinScore = 5000;

        public static float MinColor = 0.25f;
        public static float MaxColor = 0.75f;

    }



