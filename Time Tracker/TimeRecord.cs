using System;
using System.Linq;

namespace Time_Tracker
{
    class TimeRecord
    {
        const int ELEMENTS_TIME = 0;
        const int ELEMENTS_ITEMS = 1;
        const int ELEMENTS_DEATHS = 2;
        const int ELEMENTS_FAIRIEREVIVALS = 3;
        const int ELEMENTS_GAMESTATE = 4;
        const int ELEMENTS_SWORDSTATE = 5;
        const int ELEMENTS_DIFFICULTY = 6;
        const int ELEMENTS_LOGIC = 7;
        const int ELEMENTS_GOAL = 8;
        const int ELEMENTS_VARIATION = 9;
        const int ELEMENTS_PLACEMENT = 10;
        const int ELEMENTS_PEDESTAL = 11;

        private TimeSpan timeTaken;
        private int items;
        private int deaths;
        private int fairieRevivals;
        private String gameState;
        private String swordState;
        private String difficulty;
        private String logic;
        private String goal;
        private String variation;
        private String placement;
        private Boolean pedestal;

        public TimeSpan TimeTaken
        {
            get { return timeTaken; }
            set { if (value.TotalMilliseconds > 0) timeTaken = value; }
        }

        public int Items
        {
            get { return items; }
            set { if (value > 0) items = value; }
        }

        public int Deaths
        {
            get { return deaths; }
            set { deaths = value; }
        }

        public int FairieRevivals
        {
            get { return fairieRevivals; }
            set { fairieRevivals = value; }
        }

        public String GameState
        {
            get { return gameState; }
            set { if (CheckArrayValue(TimeTracker.GameStates, value)) gameState = value; }
        }

        public String SwordState
        {
            get { return swordState; }
            set { if (CheckArrayValue(TimeTracker.SwordStates, value)) swordState = value; }
        }

        public String Difficulty
        {
            get { return difficulty; }
            set { if (CheckArrayValue(TimeTracker.Difficulties, value)) difficulty = value; }
        }

        public String Logic
        {
            get { return logic; }
            set { if (CheckArrayValue(TimeTracker.Logics, value)) logic = value; }
        }

        public String Goal
        {
            get { return goal; }
            set { if (CheckArrayValue(TimeTracker.Goals, value)) goal = value; }
        }

        public String Variation
        {
            get { return variation; }
            set { if (CheckArrayValue(TimeTracker.Variations, value)) variation = value; }
        }

        public String Placement
        {
            get { return placement; }
            set { if (CheckArrayValue(TimeTracker.Placements, value)) placement = value; }
        }

        public Boolean Pedestal
        {
            get { return pedestal; }
            set { pedestal = value; }
        }

        private Boolean CheckArrayValue(String[] array, String value)
        {
            return (array.ToList<String>().Contains(value));
        }

        public String CSVOutput ()
        {
            return $"{General.StringFromTimeSpan(timeTaken)}," +
                   $"{items}," +
                   $"{deaths}," +
                   $"{fairieRevivals}," +
                   $"{gameState}," +
                   $"{swordState}," +
                   $"{difficulty}," +
                   $"{logic}," +
                   $"{goal}," +
                   $"{variation}," +
                   $"{placement}," +
                   $"{pedestal}";
        }
            
        internal void Initialize(string lineOfText)
        {
            String[] elements = lineOfText.Split(',');

            TimeTaken = General.TimeSpanFromString(elements[ELEMENTS_TIME]);

            Int32.TryParse(elements[ELEMENTS_ITEMS], out int recordedItems);
            Items = recordedItems;

            Int32.TryParse(elements[ELEMENTS_DEATHS], out deaths);
            Int32.TryParse(elements[ELEMENTS_FAIRIEREVIVALS], out fairieRevivals);
            GameState = elements[ELEMENTS_GAMESTATE];
            SwordState = elements[ELEMENTS_SWORDSTATE];
            Difficulty = elements[ELEMENTS_DIFFICULTY];
            Logic = elements[ELEMENTS_LOGIC];
            Goal = elements[ELEMENTS_GOAL];
            Variation = elements[ELEMENTS_VARIATION];
            Placement = elements[ELEMENTS_PLACEMENT];
            Boolean.TryParse(elements[ELEMENTS_PEDESTAL], out bool pedestal);
            Pedestal = pedestal;
        }

        public Boolean IsValid()
        {
            return timeTaken.Milliseconds > 0 &&
                   Items > 0 &&
                   CheckArrayValue(TimeTracker.GameStates, GameState) &&
                   CheckArrayValue(TimeTracker.SwordStates, SwordState) &&
                   CheckArrayValue(TimeTracker.Difficulties, Difficulty) &&
                   CheckArrayValue(TimeTracker.Logics, Logic) &&
                   CheckArrayValue(TimeTracker.Goals, Goal) &&
                   CheckArrayValue(TimeTracker.Variations, Variation) &&
                   CheckArrayValue(TimeTracker.Placements, Placement);
        }
    }
}
