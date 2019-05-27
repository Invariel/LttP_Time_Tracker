using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Time_Tracker
{
    public partial class TimeTracker : Form
    {
        private List<TimeRecord> recordedTimes = new List<TimeRecord>();

        public const String COMBO_DONTCARE = "--";
        public static readonly Point POINT_TIME = new Point(148, 16);
        public static readonly Point POINT_ITEMS = new Point(148, 38);
        public static readonly Point POINT_DEATHS = new Point(148, 60);
        public static readonly Point POINT_FAIRIES = new Point(148, 82);

        #region Standard Options
        public static readonly String[] GameStates = { "Standard", "Open", "Inverted" };
        public static readonly String[] SwordStates = { "Randomized", "Uncle Assured", "Swordless" };
        public static readonly String[] Difficulties = { "Easy", "Normal", "Hard", "Expert", "Insane", "Crowd Control" };
        public static readonly String[] Logics = { "No Glitches", "Overworld Glitches", "Major Glitches", "None" };
        public static readonly String[] Goals = { "Defeat Ganon", "All Dungeons", "Master Sword Pedestal", "Triforce Pieces" };
        public static readonly String[] Variations = { "None", "Keysanity", "Retro", "Timed Race", "Timed OHKO", "OHKO", "Coop", "Multiworld" };
        public static readonly String[] Placements = { "Randomized", "Plando" };
        public static readonly string[] Pedestals = { "Yes", "No" };
        #endregion Standard Options

        #region Enemizer
        // Coming later, if necessary.
        #endregion Enemizer

        public TimeTracker()
        {
            InitializeComponent();

            InitializeComboBoxes();

            InitializeStatisticsCallbacks();

            LoadExistingData();
            dg_TimeTable.Refresh();

            UpdateStatisticsPage(null, null);

            btn_AddGame.Click += RecordGame;
        }

        private void InitializeComboBoxes()
        {
            cmb_Difficulty.Items.AddRange(Difficulties);
            cmb_Goal.Items.AddRange(Goals);
            cmb_GameLogic.Items.AddRange(Logics);
            cmb_Placement.Items.AddRange(Placements);
            cmb_GameState.Items.AddRange(GameStates);
            cmb_Swords.Items.AddRange(SwordStates);
            cmb_Variation.Items.AddRange(Variations);

            cmb_StatisticsDifficulty.Items.AddRange(Difficulties);
            cmb_StatisticsGoal.Items.AddRange(Goals);
            cmb_StatisticsLogic.Items.AddRange(Logics);
            cmb_StatisticsPedestal.Items.AddRange(Pedestals);
            cmb_StatisticsPlacement.Items.AddRange(Placements);
            cmb_StatisticsGameState.Items.AddRange(GameStates);
            cmb_StatisticsSwordsState.Items.AddRange(SwordStates);
            cmb_StatisticsVariation.Items.AddRange(Variations);

            cmb_StatisticsDifficulty.Items.Insert(0, COMBO_DONTCARE);
            cmb_StatisticsGoal.Items.Insert(0, COMBO_DONTCARE);
            cmb_StatisticsLogic.Items.Insert(0, COMBO_DONTCARE);
            cmb_StatisticsPedestal.Items.Insert(0, COMBO_DONTCARE);
            cmb_StatisticsPlacement.Items.Insert(0, COMBO_DONTCARE);
            cmb_StatisticsGameState.Items.Insert(0, COMBO_DONTCARE);
            cmb_StatisticsSwordsState.Items.Insert(0, COMBO_DONTCARE);
            cmb_StatisticsVariation.Items.Insert(0, COMBO_DONTCARE);

            cmb_GameState.SelectedIndex = 0;
            cmb_Swords.SelectedIndex = 0;
            cmb_Difficulty.SelectedIndex = 1;
            cmb_GameLogic.SelectedIndex = 0;
            cmb_Goal.SelectedIndex = 0;
            cmb_Variation.SelectedIndex = 0;
            cmb_Placement.SelectedIndex = 0;

            cmb_StatisticsDifficulty.SelectedIndex = 0;
            cmb_StatisticsGoal.SelectedIndex = 0;
            cmb_StatisticsLogic.SelectedIndex = 0;
            cmb_StatisticsPedestal.SelectedIndex = 0;
            cmb_StatisticsPlacement.SelectedIndex = 0;
            cmb_StatisticsGameState.SelectedIndex = 0;
            cmb_StatisticsSwordsState.SelectedIndex = 0;
            cmb_StatisticsVariation.SelectedIndex = 0;
        }

        private void InitializeStatisticsCallbacks()
        {
            cmb_StatisticsDifficulty.SelectionChangeCommitted += UpdateStatisticsPage;
            cmb_StatisticsGoal.SelectionChangeCommitted += UpdateStatisticsPage;
            cmb_StatisticsLogic.SelectionChangeCommitted += UpdateStatisticsPage;
            cmb_StatisticsPedestal.SelectionChangeCommitted += UpdateStatisticsPage;
            cmb_StatisticsPlacement.SelectionChangeCommitted += UpdateStatisticsPage;
            cmb_StatisticsGameState.SelectionChangeCommitted += UpdateStatisticsPage;
            cmb_StatisticsSwordsState.SelectionChangeCommitted += UpdateStatisticsPage;
            cmb_StatisticsVariation.SelectionChangeCommitted += UpdateStatisticsPage;
        }

        private void LoadExistingData()
        {
            if (File.Exists("./gamedetails.txt"))
            {
                using (StreamReader reader = new StreamReader("./gamedetails.txt"))
                {
                    while (!reader.EndOfStream)
                    {
                        String lineOfText = reader.ReadLine();
                        TimeRecord newRecord = new TimeRecord();
                        newRecord.Initialize(lineOfText);

                        if (newRecord.IsValid())
                        {
                            bs_TimeTable.Add(newRecord);
                        }
                    }
                }
            }
        }

        private void RecordGame(object sender, EventArgs e)
        {
            // Validate that the fields are all filled out.  Otherwise, bad things happen.
            if (!ValidateFields(out String validationString))
            {
                MessageBox.Show(validationString, "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                TimeRecord newRecord = new TimeRecord()
                {
                    Deaths = Convert.ToInt32(numud_Deaths.Value),
                    Difficulty = cmb_Difficulty.Text,
                    FairieRevivals = Convert.ToInt32(numud_FairieRevivals.Value),
                    GameState = cmb_GameState.Text,
                    Goal = cmb_Goal.Text,
                    Items = Convert.ToInt32(numud_ItemCount.Value),
                    Logic = cmb_GameLogic.Text,
                    Pedestal = chk_Pedestal.Checked,
                    Placement = cmb_Placement.Text,
                    SwordState = cmb_Swords.Text,
                    TimeTaken = General.TimeSpanFromString(mtxt_FinalTime.Text),
                    Variation = cmb_Variation.Text,
                };

                // Copy the contents of the game that was stored to the flat file that is used for recording the games.
                using (StreamWriter writer = new StreamWriter("./gamedetails.txt", true))
                {
                    writer.WriteLine(newRecord.CSVOutput());
                }

                // And update the table.
                bs_TimeTable.Add(newRecord);
                dg_TimeTable.Refresh();

                mtxt_FinalTime.Text = "00:00:00.00";
                numud_ItemCount.Value = 0;
                numud_Deaths.Value = 0;
                numud_FairieRevivals.Value = 0;
            }
        }

        private Boolean ValidateFields(out String errorString)
        {
            bool retval = true;
            errorString = "";

            // Make sure that a time has been set.
            TimeSpan timeSpan = General.TimeSpanFromString(mtxt_FinalTime.Text);

            if (timeSpan.TotalMilliseconds == 0)
            {
                retval = false;
                errorString = "Somehow, I think this seed took time to complete";
            }

            // Make sure that more than zero items were collected.
            if (numud_ItemCount.Value == 0)
            {
                retval = false;
                errorString = $"{errorString}{(String.IsNullOrEmpty(errorString) ? "" : " and ")}I doubt you completed the seed with no items.  Nice try";
            }

            if (!String.IsNullOrEmpty(errorString))
            {
                errorString += ".";
            }

            return retval;
        }

        private void UpdateStatisticsPage(object sender, EventArgs e)
        {
            // First, filter the records.
            List<TimeRecord> filteredRecords = bs_TimeTable.List.OfType<TimeRecord>().Where(
                record =>
                    (cmb_StatisticsGameState.Text == COMBO_DONTCARE || record.GameState == cmb_StatisticsGameState.Text) &&
                    (cmb_StatisticsSwordsState.Text == COMBO_DONTCARE || record.SwordState == cmb_StatisticsSwordsState.Text) &&
                    (cmb_StatisticsDifficulty.Text == COMBO_DONTCARE || record.Difficulty == cmb_StatisticsDifficulty.Text) &&
                    (cmb_StatisticsPlacement.Text == COMBO_DONTCARE || record.Placement == cmb_StatisticsPlacement.Text) &&
                    (cmb_StatisticsLogic.Text == COMBO_DONTCARE || record.Logic == cmb_StatisticsLogic.Text) &&
                    (cmb_StatisticsGoal.Text == COMBO_DONTCARE || record.Goal == cmb_StatisticsGoal.Text) &&
                    (cmb_StatisticsVariation.Text == COMBO_DONTCARE || record.Variation == cmb_StatisticsVariation.Text) &&
                    (cmb_StatisticsPedestal.Text == COMBO_DONTCARE || (record.Pedestal && cmb_StatisticsPedestal.Text == "Yes") || (!record.Pedestal && cmb_StatisticsPedestal.Text == "No"))
                ).ToList<TimeRecord>();

            if (filteredRecords.Count == 0)
            {
                lbl_PersonalBestDeaths.Text = "0";
                lbl_PersonalBestFairieRevivals.Text = "0";
                lbl_PersonalBestItems.Text = "0/216";
                lbl_PersonalBestTime.Text = "00:00:00.00";

                lbl_AveragesDeaths.Text = "0";
                lbl_AveragesFairieRevivals.Text = "0";
                lbl_AveragesItems.Text = "0/216";
                lbl_AveragesTime.Text = "00:00:00.00";

                lbl_TotalsDeaths.Text = "0";
                lbl_TotalsFairieRevivals.Text = "0";
                lbl_TotalsItems.Text = "0";
                lbl_TotalsTime.Text = "00:00:00.00";

                lbl_RecordCount.Text = "Showing data for 0 records.";
            }
            else
            {
                // Sort the elements by time taken.
                filteredRecords.Sort((record1, record2) => record1.TimeTaken.CompareTo(record2.TimeTaken));

                // Then figure out the personal best (lowest time)
                lbl_PersonalBestDeaths.Text = $"{filteredRecords[0].Deaths}";
                lbl_PersonalBestFairieRevivals.Text = $"{filteredRecords[0].FairieRevivals}";
                lbl_PersonalBestItems.Text = $"{filteredRecords[0].Items}/216";
                lbl_PersonalBestTime.Text = $"{General.StringFromTimeSpan(filteredRecords[0].TimeTaken)}";

                // Then calculate averages and totals.
                TimeSpan totalTime = new TimeSpan(0);
                int totalDeaths = 0;
                int totalFairies = 0;
                int totalItems = 0;

                foreach (TimeRecord timeRecord in filteredRecords)
                {
                    totalTime = totalTime.Add(timeRecord.TimeTaken);
                    totalDeaths += timeRecord.Deaths;
                    totalFairies += timeRecord.FairieRevivals;
                    totalItems += timeRecord.Items;
                }

                lbl_TotalsDeaths.Text = $"{totalDeaths}";
                lbl_TotalsFairieRevivals.Text = $"{totalFairies}";
                lbl_TotalsItems.Text = $"{totalItems}";
                lbl_TotalsTime.Text = $"{General.StringFromTimeSpan(totalTime)}";

                int recordCount = filteredRecords.Count;
                TimeSpan averageTime = new TimeSpan(0, 0, 0, 0, Convert.ToInt32(totalTime.TotalMilliseconds / recordCount));

                lbl_AveragesDeaths.Text = $"{totalDeaths / recordCount}";
                lbl_AveragesFairieRevivals.Text = $"{totalFairies / recordCount}";
                lbl_AveragesItems.Text = $"{totalItems / recordCount}/216";
                lbl_AveragesTime.Text = $"{General.StringFromTimeSpan(averageTime)}";

                lbl_RecordCount.Text = $"Showing data for {filteredRecords.Count} game{(filteredRecords.Count == 1 ? "" : "s")}.";
            }

            UpdatePosition(lbl_PersonalBestTime, POINT_TIME);
            UpdatePosition(lbl_PersonalBestItems, POINT_ITEMS);
            UpdatePosition(lbl_PersonalBestDeaths, POINT_DEATHS);
            UpdatePosition(lbl_PersonalBestFairieRevivals, POINT_FAIRIES);

            UpdatePosition(lbl_AveragesTime, POINT_TIME);
            UpdatePosition(lbl_AveragesItems, POINT_ITEMS);
            UpdatePosition(lbl_AveragesDeaths, POINT_DEATHS);
            UpdatePosition(lbl_AveragesFairieRevivals, POINT_FAIRIES);

            UpdatePosition(lbl_TotalsTime, POINT_TIME);
            UpdatePosition(lbl_TotalsItems, POINT_ITEMS);
            UpdatePosition(lbl_TotalsDeaths, POINT_DEATHS);
            UpdatePosition(lbl_TotalsFairieRevivals, POINT_FAIRIES);
        }

        private void UpdatePosition(Label label, Point right)
        {
            label.Location = new Point(right.X - label.Size.Width, right.Y);
        }
    }
}
