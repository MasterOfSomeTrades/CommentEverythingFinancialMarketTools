using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace CommentEverythingMarketToolsNETCore
{
    public static class OptionSymbolConversion {
        public enum OptionType {
            PUT,
            CALL
        }

        public static string CreateCBOEOptionSymbol(string underlyingSymbol, string expiry_yyMMdd, OptionType optionType, decimal strikePrice) {
            // --- Pad price with leading zeroes, and to tenth of a penny
            string priceStr = (strikePrice * 1000).ToString("0");
            priceStr = priceStr.PadLeft(8, '0');

            // --- Default to "C"
            string putOrCall = "C";
            if (optionType == OptionType.PUT) {
                putOrCall = "P";
            }

            return underlyingSymbol + expiry_yyMMdd + putOrCall + priceStr;
        }

        public static string GetUnderlyingSymbolFromCBOEOption(string cboeOptionString) {
            return cboeOptionString.Substring(0, cboeOptionString.Length - 15);
        }

        public static double GetStrikePriceFromCBOEOption(string cboeOptionString) {
            return double.Parse(cboeOptionString.Substring(cboeOptionString.Length - 8, 8))/1000;
        }

        public static double GetUnixTimeExpiryFromCBOEOption(string cboeOptionString) {
            return ConvertCBOEExpiryStringToUnixTime(cboeOptionString.Substring(cboeOptionString.Length - 15, 6));
        }

        public static OptionType GetOptionTypeFromCBOEOption(string cboeOptionString) {
            OptionType ret = OptionType.CALL;
            string optionTypeString = cboeOptionString.Substring(cboeOptionString.Length - 9, 1);
            if (optionTypeString.Equals("P", StringComparison.InvariantCultureIgnoreCase)) {
                ret = OptionType.PUT;
            }

            return ret;
        }

        public static string GetFriendlyCBOEOptionString(string cboeOptionString) {
            string underlyingSymbol = GetUnderlyingSymbolFromCBOEOption(cboeOptionString);
            double expiryInUnixTime = ConvertCBOEExpiryStringToUnixTime(cboeOptionString.Substring(underlyingSymbol.Length, 6));
            string prettyExpiryDate = Clock.DisplayDateString(Clock.Convert(expiryInUnixTime, Clock.TimeZoneId.UTC));
            string optionType = cboeOptionString.Substring(underlyingSymbol.Length + 6, 1);
            string strikePriceString = cboeOptionString.Substring(underlyingSymbol.Length + 7);
            decimal strikePrice = decimal.Parse(strikePriceString) / 1000M;

            return underlyingSymbol + " " + optionType + " " + prettyExpiryDate + " " + strikePrice.ToString(("0.00"));
        }

        public static double ConvertCBOEExpiryStringToUnixTime(string cboeExpiry) {
            DateTime date = DateTime.ParseExact(cboeExpiry, "yyMMdd", CultureInfo.InvariantCulture);
            return Clock.ConvertToUnixTimestamp(date, Clock.TimeZoneId.UTC);
        }
    }
}
