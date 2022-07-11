using System.Collections.Generic;

namespace Weather.BusinessLogic.Models
{
    public class DeviceMeasurement
    {
        public List<Measurement> Measurements { get; set; }
        public Device Device { get; set; }
    }
}
