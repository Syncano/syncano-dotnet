using System;
using System.Collections.Generic;

namespace Syncano.Net.Access
{
    /// <summary>
    /// Class converting TimeZone to string.
    /// </summary>
    public class TimeZoneStringConverter
    {
        private readonly Dictionary<string, string> _mapper;
 
        /// <summary>
        /// Creates TimeZoneStringConverter
        /// </summary>
        public TimeZoneStringConverter()
        {
            _mapper = new Dictionary<string, string>();

            _mapper.Add("Dateline Standard Time", "Etc/GMT+12");
            _mapper.Add("UTC-11", "Etc/GMT+11");
            _mapper.Add("Hawaiian Standard Time", "Pacific/Honolulu");
            _mapper.Add("Alaskan Standard Time", "America/Anchorage");
            _mapper.Add("Pacific Standard Time (Mexico)", "America/Santa_Isabel");
            _mapper.Add("Pacific Standard Time", "America/Los_Angeles");
            _mapper.Add("US Mountain Standard Time", "America/Phoenix");
            _mapper.Add("Mountain Standard Time (Mexico)", "America/Chihuahua");
            _mapper.Add("Mountain Standard Time", "America/Denver");
            _mapper.Add("Central America Standard Time", "America/Guatemala");
            _mapper.Add("Central Standard Time", "America/Chicago");
            _mapper.Add("Central Standard Time (Mexico)", "America/Mexico_City");
            _mapper.Add("Canada Central Standard Time", "America/Regina");
            _mapper.Add("SA Pacific Standard Time", "America/Bogota");
            _mapper.Add("Eastern Standard Time", "America/New_York");
            _mapper.Add("US Eastern Standard Time", "America/Indiana/Indianapolis");
            _mapper.Add("Venezuela Standard Time", "America/Caracas");
            _mapper.Add("Paraguay Standard Time", "America/Asuncion");
            _mapper.Add("Atlantic Standard Time", "America/Halifax");
            _mapper.Add("Central Brazilian Standard Time", "America/Cuiaba");
            _mapper.Add("SA Western Standard Time", "America/La_Paz");
            _mapper.Add("Pacific SA Standard Time", "America/Santiago");
            _mapper.Add("Newfoundland Standard Time", "America/St_Johns");
            _mapper.Add("E. South America Standard Time", "America/Sao_Paulo");
            _mapper.Add("Argentina Standard Time", "America/Argentina/Buenos_Aires");
            _mapper.Add("SA Eastern Standard Time", "America/Cayenne");
            _mapper.Add("Greenland Standard Time", "America/Godthab");
            _mapper.Add("Montevideo Standard Time", "America/Montevideo");
            _mapper.Add("Bahia Standard Time", "America/Bahia");
            _mapper.Add("UTC-02", "Etc/GMT+2");
            _mapper.Add("Azores Standard Time", "Atlantic/Azores");
            _mapper.Add("Cape Verde Standard Time", "Atlantic/Cape_Verde");
            _mapper.Add("Morocco Standard Time", "Africa/Casablanca");
            _mapper.Add("Coordinated Universal Time", "Etc/GMT");
            _mapper.Add("GMT Standard Time", "Europe/London");
            _mapper.Add("Greenwich Standard Time", "Atlantic/Reykjavik");
            _mapper.Add("W. Europe Standard Time", "Europe/Berlin");
            _mapper.Add("Central Europe Standard Time", "Europe/Budapest");
            _mapper.Add("Romance Standard Time", "Europe/Paris");
            _mapper.Add("Central European Standard Time", "Europe/Warsaw");
            _mapper.Add("W. Central Africa Standard Time", "Africa/Lagos");
            _mapper.Add("Namibia Standard Time", "Africa/Windhoek");
            _mapper.Add("Jordan Standard Time", "Asia/Amman");
            _mapper.Add("GTB Standard Time", "Europe/Bucharest");
            _mapper.Add("Middle East Standard Time", "Asia/Beirut");
            _mapper.Add("Egypt Standard Time", "Africa/Cairo");
            _mapper.Add("Syria Standard Time", "Asia/Damascus");
            _mapper.Add("E. Europe Standard Time", "Europe/Bucharest");
            _mapper.Add("South Africa Standard Time", "Africa/Johannesburg");
            _mapper.Add("FLE Standard Time", "Europe/Kiev");
            _mapper.Add("Turkey Standard Time", "Europe/Istanbul");
            _mapper.Add("Jerusalem Standard Time", "Asia/Jerusalem");
            _mapper.Add("Libya Standard Time", "Africa/Tripoli");
            _mapper.Add("Arabic Standard Time", "Asia/Baghdad");
            _mapper.Add("Kaliningrad Standard Time", "Europe/Kaliningrad");
            _mapper.Add("Arab Standard Time", "Asia/Riyadh");
            _mapper.Add("E. Africa Standard Time", "Africa/Nairobi");
            _mapper.Add("Iran Standard Time", "Asia/Tehran");
            _mapper.Add("Arabian Standard Time", "Asia/Dubai");
            _mapper.Add("Azerbaijan Standard Time", "Asia/Baku");
            _mapper.Add("Russian Standard Time", "Europe/Moscow");
            _mapper.Add("Mauritius Standard Time", "Indian/Mauritius");
            _mapper.Add("Georgian Standard Time", "Asia/Tbilisi");
            _mapper.Add("Caucasus Standard Time", "Asia/Yerevan");
            _mapper.Add("Afghanistan Standard Time", "Asia/Kabul");
            _mapper.Add("West Asia Standard Time", "Asia/Tashkent");
            _mapper.Add("Pakistan Standard Time", "Asia/Karachi");
            _mapper.Add("India Standard Time", "Asia/Kolkata");
            _mapper.Add("Sri Lanka Standard Time", "Asia/Colombo");
            _mapper.Add("Nepal Standard Time", "Asia/Kathmandu");
            _mapper.Add("Central Asia Standard Time", "Asia/Almaty");
            _mapper.Add("Bangladesh Standard Time", "Asia/Dhaka");
            _mapper.Add("Ekaterinburg Standard Time", "Asia/Yekaterinburg");
            _mapper.Add("Myanmar Standard Time", "Asia/Rangoon");
            _mapper.Add("SE Asia Standard Time", "Asia/Bangkok");
            _mapper.Add("N. Central Asia Standard Time", "Asia/Novosibirsk");
            _mapper.Add("China Standard Time", "Asia/Shanghai");
            _mapper.Add("North Asia Standard Time", "Asia/Krasnoyarsk");
            _mapper.Add("Malay Peninsula Standard Time", "Asia/Singapore");
            _mapper.Add("W. Australia Standard Time", "Australia/Perth");
            _mapper.Add("Taipei Standard Time", "Asia/Taipei");
            _mapper.Add("Ulaanbaatar Standard Time", "Asia/Ulaanbaatar");
            _mapper.Add("North Asia East Standard Time", "Asia/Irkutsk");
            _mapper.Add("Tokyo Standard Time", "Asia/Tokyo");
            _mapper.Add("Korea Standard Time", "Asia/Seoul");
            _mapper.Add("Cen. Australia Standard Time", "Australia/Adelaide");
            _mapper.Add("AUS Central Standard Time", "Australia/Darwin");
            _mapper.Add("E. Australia Standard Time", "Australia/Brisbane");
            _mapper.Add("AUS Eastern Standard Time", "Australia/Sydney");
            _mapper.Add("West Pacific Standard Time", "Pacific/Port_Moresby");
            _mapper.Add("Tasmania Standard Time", "Australia/Hobart");
            _mapper.Add("Yakutsk Standard Time", "Asia/Yakutsk");
            _mapper.Add("Central Pacific Standard Time", "Pacific/Guadalcanal");
            _mapper.Add("Vladivostok Standard Time", "Asia/Vladivostok");
            _mapper.Add("New Zealand Standard Time", "Pacific/Auckland");
            _mapper.Add("UTC+12", "Etc/GMT-12");
            _mapper.Add("Fiji Standard Time", "Pacific/Fiji");
            _mapper.Add("Magadan Standard Time", "Asia/Magadan");
            _mapper.Add("Tonga Standard Time", "Pacific/Tongatapu");
            _mapper.Add("Samoa Standard Time", "Pacific/Apia");
        }

        /// <summary>
        /// Gets string representation of time zone in IANA format.
        /// </summary>
        /// <param name="timeZone">TimeZoneInfo object to convert.</param>
        /// <returns>String object.</returns>
        public string GetString(TimeZoneInfo timeZone)
        {
            if(timeZone == TimeZoneInfo.Utc)
                return "Etc/UTC";

            string result;
            var success = _mapper.TryGetValue(timeZone.StandardName, out result);
            if (!success)
                throw new ArgumentException("Time zones marked as old are not supported.");

            return result;
        }
    }
}
