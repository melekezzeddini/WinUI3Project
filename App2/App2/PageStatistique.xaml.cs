using System;
using System.Collections.Generic;
using Microsoft.UI.Xaml.Controls;

namespace App2
{
    public sealed partial class PageStatistique : Page
    {
        public int TotalMembers { get; set; }
        public int TotalActivities { get; set; }
        public List<string> MembersPerActivity { get; set; } // Changed to List<string>
        public List<string> AverageRatings { get; set; }     // Changed to List<string>
        public int TotalSessions { get; set; }
        public int TotalParticipants { get; set; }
        public List<string> AverageSessionsPerActivity { get; set; } // Changed to List<string>

        public PageStatistique()
        {
            this.InitializeComponent();
            LoadStatistics();
        }

        private void LoadStatistics()
        {
            var (totalMembers, totalActivities, membersPerActivity, averageRatings, totalSessions, totalParticipants, averageSessionsPerActivity) =
                Singleton.Instance.GetAllStatistics();

            TotalMembers = totalMembers;
            TotalActivities = totalActivities;
            MembersPerActivity = membersPerActivity; // Directly assign the list
            AverageRatings = averageRatings;         // Directly assign the list
            TotalSessions = totalSessions;
            TotalParticipants = totalParticipants;
            AverageSessionsPerActivity = averageSessionsPerActivity; // Directly assign the list
        }
    }
}
