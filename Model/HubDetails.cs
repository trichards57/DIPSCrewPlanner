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
                DipsName = "COVID-19 Ambulance Support Shift: WMAS (Worcestershire Hub)",
                DipsContext = DipsContext.WMR,
                VehicleCount = 2
            },
            new HubDetails
            {
                DisplayName= "Coventry (PTS)",
                DipsName = "COVID-19 Ambulance Support Shift: WMAS (Coventry Hub)",
                DipsContext = DipsContext.WMR,
                VehicleCount = 2
            },
            new HubDetails
            {
                DisplayName= "Union Park",
                DipsName = "COVID-19 Ambulance Support Shift: WMAS (Birmingham Hub)",
                DipsContext = DipsContext.WMR,
                VehicleCount = 5
            },
            new HubDetails
            {
                DisplayName= "Rugeley",
                DipsName = "COVID-19 Ambulance Support Shift: WMAS (Rugeley Hub)",
                DipsContext = DipsContext.WMR,
                VehicleCount = 2
            },
            new HubDetails
            {
                DisplayName= "Truro",
                DipsName = "COVID-19 Ambulance Support Shift: SWAST (Truro Hub)",
                DipsContext = DipsContext.SWR,
                VehicleCount = 2
            },
            new HubDetails
            {
                DisplayName= "Exeter",
                DipsName = "COVID-19 Ambulance Support Shift: SWAST (Exeter Hub)",
                DipsContext = DipsContext.SWR,
                VehicleCount = 2
            },
            new HubDetails
            {
                DisplayName= "Bristol",
                DipsName = "COVID-19 Ambulance Support Shift: SWAST (Bristol Hub)",
                DipsContext = DipsContext.SWR,
                VehicleCount = 6
            },
            new HubDetails
            {
                DisplayName= "Poole",
                DipsName = "COVID-19 Ambulance Support Shift: SWAST (Poole Hub)",
                DipsContext = DipsContext.SWR,
                VehicleCount = 3
            },
            new HubDetails
            {
                DisplayName= "Staverton",
                DipsName = "COVID-19 Ambulance Support Shift: SWAST (Staverton Hub)",
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
