using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCXAirApi.Domain.Dto
{
    public class CurrencyConverterApiOptions
    {
        public string BaseUrl { get; set; }
        public string ApiKey { get; set; }
    }

    public class CurrencyConversionResponse
    {
        public double Rate { get; set; }
        public CurrencyInfo From { get; set; }
        public CurrencyInfo To { get; set; }
        public long Timestamp { get; set; }
    }
    public class CurrencyInfo
    {
        public double Rate { get; set; }
        public string Currency { get; set; }
    }
}
