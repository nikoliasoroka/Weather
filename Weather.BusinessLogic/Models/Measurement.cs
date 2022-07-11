using System;
using Weather.BusinessLogic.Models.Interfaces;

namespace Weather.BusinessLogic.Models
{
    public class Measurement : IFileReader
    {
        public DateTime Time { get; set; }
        public string Value { get; set; }

        public void Populate(string time, string value)
        {
            Time = DateTime.Parse(time);

            if (value.StartsWith(','))
                value = value.Insert(0, "0");

            Value = value;
        }
    }
}
