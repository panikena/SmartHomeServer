using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeServer.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum WidgetType
    {
        WarmFloor = 1, 
        LightSwitch,
        Thermo,
        ColorPickerSwitch,
        Test

    }
}
