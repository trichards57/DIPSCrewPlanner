using System.Collections.Generic;

namespace DIPSCrewPlanner.Model
{
    internal enum DipsContext
    {
        WMR,
        SWR
    }

    internal class HubDetails
    {
        private static List<HubDetails> DefaultDetails = new List<HubDetails>
        {
            new HubDetails
            {
                DisplayName= "Worcester",
                DipsName = "Worcestershire Hub",
                DipsContext = DipsContext.WMR,
                VehicleCount = 2
            },
            new HubDetails
            {
                DisplayName= "Coventry (PTS)",
                DipsName = "Coventry Hub",
                DipsContext = DipsContext.WMR,
                VehicleCount = 2
            },
            new HubDetails
            {
                DisplayName= "Union Park",
                DipsName = "Birmingham Hub",
                DipsContext = DipsContext.WMR,
                VehicleCount = 5
            },
            new HubDetails
            {
                DisplayName= "Rugeley",
                DipsName = "Rugeley Hub",
                DipsContext = DipsContext.WMR,
                VehicleCount = 2
            },
            new HubDetails
            {
                DisplayName= "Truro",
                DipsName = "Truro Hub",
                DipsContext = DipsContext.SWR,
                VehicleCount = 2
            },
            new HubDetails
            {
                DisplayName= "Exeter",
                DipsName = "Exeter Hub",
                DipsContext = DipsContext.SWR,
                VehicleCount = 2
            },
            new HubDetails
            {
                DisplayName= "Bristol",
                DipsName = "Bristol Hub",
                DipsContext = DipsContext.SWR,
                VehicleCount = 6
            },
            new HubDetails
            {
                DisplayName= "Poole",
                DipsName = "Poole Hub",
                DipsContext = DipsContext.SWR,
                VehicleCount = 3
            },
            new HubDetails
            {
                DisplayName= "Staverton",
                DipsName = "Staverton Hub",
                DipsContext = DipsContext.SWR,
                VehicleCount = 2
            }
        };

        public DipsContext DipsContext { get; private set; }
        public string DipsName { get; private set; }
        public string DisplayName { get; private set; }
        public int VehicleCount { get; private set; }

        public static IReadOnlyCollection<HubDetails> GetDefaultSettings()
        {
            return DefaultDetails.AsReadOnly();
        }
    }
}