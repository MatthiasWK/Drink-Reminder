using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Contains global variables used for game settings
    public static class Variables
    {
        public static int GameSize = 0;

        public static int Rows = 6;
        public static int Columns = 6;

        public static int NumShapes = 5;

        public static int ShapeTheme = 0;

        public static int Background = 0;

        public static bool CustomBackgrounds = false;
                      
        public static bool BackgroundsChanged = true;
                      
        public static bool CustomShapes = false;
                     
        public static bool ShapesChanged = true;

        public static string BackgroundPath = "Backgrounds";

        public static bool TriggerStop = false;
        public static bool TriggerGo = false;

        public static readonly float AnimationDuration =  0.3f;

        public static readonly float MoveAnimationMinDuration = 0.6f;

        public static readonly float ExplosionDuration = 0.4f;

        public static readonly float WaitBeforePotentialMatchesCheck = 2f;
        public static readonly float OpacityAnimationFrameDelay = 0.05f;

        public static readonly int MinimumMatches = 3;
        public static readonly int MinimumMatchesForBonus = 4;

        public static readonly int Match3Score = 60;
        public static readonly int SubsequentMatchScore = 1000;
        public static readonly int WinScore = 10000;

        public static float MinColor = 0.10f;
        public static float MaxColor = 0.50f;

        public static int GameMode = 0;

        public static bool IsPlaying = false;
    }



