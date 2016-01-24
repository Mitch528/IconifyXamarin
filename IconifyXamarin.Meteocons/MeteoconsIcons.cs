using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IconifyXamarin.Meteocons
{
    public class MeteoconsIcons : IIcon
    {
        public const char mc_sunrise_o = 'A';
        public const char mc_sun_o = 'B';
        public const char mc_moon_o = 'C';
        public const char mc_eclipse = 'D';
        public const char mc_cloudy_o = 'E';
        public const char mc_wind_o = 'F';
        public const char mc_snow = 'G';
        public const char mc_sun_cloud_o = 'H';
        public const char mc_moon_cloud_o = 'I';
        public const char mc_sunrise_sea_o = 'J';
        public const char mc_moonrise_sea_o = 'K';
        public const char mc_cloud_sea_o = 'L';
        public const char mc_sea_o = 'M';
        public const char mc_cloud_o = 'N';
        public const char mc_cloud_thunder_o = 'O';
        public const char mc_cloud_thunder2_o = 'P';
        public const char mc_cloud_drop_o = 'Q';
        public const char mc_cloud_rain_o = 'R';
        public const char mc_cloud_wind_o = 'S';
        public const char mc_cloud_wind_rain_o = 'T';
        public const char mc_cloud_snow_o = 'U';
        public const char mc_cloud_snow2_o = 'V';
        public const char mc_cloud_snow3_o = 'W';
        public const char mc_cloud_rain2_o = 'X';
        public const char mc_cloud_double_o = 'Y';
        public const char mc_cloud_double_thunder_o = 'Z';
        public const char mc_cloud_double_thunder2_o = '0';
        public const char mc_sun = '1';
        public const char mc_moon = '2';
        public const char mc_sun_cloud = '3';
        public const char mc_moon_cloud = '4';
        public const char mc_cloud = '5';
        public const char mc_cloud_thunder = '6';
        public const char mc_cloud_drop = '7';
        public const char mc_cloud_rain = '8';
        public const char mc_cloud_wind = '9';
        public const char mc_cloud_wind_rain = '!';
        public const char mc_cloud_snow = '"';
        public const char mc_cloud_snow2 = '#';
        public const char mc_cloud_rain2 = '$';
        public const char mc_cloud_double = '%';
        public const char mc_cloud_double_thunder = '&';
        public const char mc_thermometer = '\'';
        public const char mc_compass = '(';
        public const char mc_not_applicable = ')';
        public const char mc_celsius = '*';
        public const char mc_fahrenheit = '+';

        public static readonly List<KeyValuePair<char, string>> Characters;

        public static IIcon[] Icons { get; }

        static MeteoconsIcons()
        {
            Characters = IconReflectionUtils.GetIcons<MeteoconsIcons>();
            Icons = Characters.Select(p => new MeteoconsIcons(p.Value, p.Key)).Cast<IIcon>().ToArray();
        }

        private readonly string _key;

        public MeteoconsIcons(string key, char @char)
        {
            _key = key;
            Character = @char;
        }

        public string Key => Characters.Single(p => p.Value == _key).Value.Replace("_", "-");

        public char Character { get; }
    }
}
