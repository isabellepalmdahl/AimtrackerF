using Aimtracker.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aimtracker.Models
{
    public class Weather
    {
        public int Id { get; set; }
        public int Dt { get; set; }
        public double Temp { get; set; }
        public double Wind_speed { get; set; }
        public double Wind_deg { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }

        public Weather(GetWeatherDto getWeatherDto)
        {
            Dt = getWeatherDto.Current.Dt;
            Temp = getWeatherDto.Current.Temp.KelvinToCelsius();
            Wind_speed = getWeatherDto.Current.Wind_speed;
            Wind_deg = getWeatherDto.Current.Wind_deg;
            Description = getWeatherDto.Current.Weather[0].Description.MakeFirstCharUpperCase();
            Icon = getWeatherDto.Current.Weather[0].Icon;
        }

        public Weather()
        {

        }
    }

    public static class Extensions
    {
        public static string MakeFirstCharUpperCase(this string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return input;
            }
            return input.First().ToString().ToUpper() + input.Substring(1);
        }

        public static double KelvinToCelsius(this double kelvin)
        {
            return Math.Round(kelvin - 273.15);
        }
    }
}
