using GeoCoordinatePortable;
using Business.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utilities
{
    public static class GeoCoordinateTool
    {
        public static double Distance(GeoCoordinate source, GeoCoordinate target, int type)
        {
            //1- miles, other km
            //Use 3960 if you want miles; use 6371 if you want km
            double earthRadius = (type == 1) ? 3960 : 6371;
            double destinationLatitude = ToRadian(target.Latitude - source.Latitude);
            double destinationLongitude = ToRadian(target.Longitude - source.Longitude);

            double a = Math.Sin(destinationLatitude / 2) * Math.Sin(destinationLatitude / 2) + Math.Cos(ToRadian(source.Latitude)) * Math.Cos(ToRadian(target.Latitude)) * Math.Sin(destinationLongitude / 2) * Math.Sin(destinationLongitude / 2);

            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            double d = earthRadius * c;

            return d;
        }

        private static double ToRadian(double val)
        {
            return (Math.PI / 180) * val;
        }

        public static async Task<LocationEntity> GetLocation(string address, string googleMapAPIUrl, string googleMapKey)
        {
            dynamic googleMapData = null;
            try
            {
                if (!String.IsNullOrEmpty(address))
                {

                    string[] splitAddress = address.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                    string addressResult = String.Join("+", splitAddress);

                    string url = $"{googleMapAPIUrl}{addressResult}&key={googleMapKey}";
                    string googleMapApiResponse = string.Empty;

                    using (HttpClient client = new HttpClient())
                    {
                        using (HttpResponseMessage res = await client.GetAsync(url))
                        {
                            using (HttpContent content = res.Content)
                            {
                                googleMapApiResponse = await content.ReadAsStringAsync();
                            }
                        }
                    }

                    googleMapData = JsonConvert.DeserializeObject<dynamic>(googleMapApiResponse);
                    return new LocationEntity() { Latitude = googleMapData.results[0].geometry.location.lat, Longitude = googleMapData.results[0].geometry.location.lng, Address = address };
                }
                return null;
            }
            catch (Exception ex)
            {
                return new LocationEntity() { ErrorMessage_GoogleMapAPI = googleMapData.error_message, Status_GoogleMapAPI = googleMapData.status };
            }
        }
    }
}
