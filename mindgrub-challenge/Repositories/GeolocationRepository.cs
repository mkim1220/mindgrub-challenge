using mindgrub_challenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mindgrub_challenge.Repositories
{
    public class GeolocationRepository : IDisposable
    {
        private MindgrubChallengeDBContext context;
        private bool disposed;
        private string HQZip = "21230";

        public GeolocationRepository(MindgrubChallengeDBContext context)
        {
            this.context = context;
        }

        public bool ZipcodeExists(string zip)
        {
            return context.Zipcodes.Any(z => z.Zip == zip);
        }
        public void CalculateLocation(ZipcodeViewModel zipcodeVM)
        {
            Zipcode z = FindZipcodeFromZip(zipcodeVM.Zipcode);
            Zipcode hq = FindHQZipcode();

            zipcodeVM.City = z.City;
            zipcodeVM.State = z.State;
            zipcodeVM.Distance = Haversine(z.Latitude, hq.Latitude, z.Longitude, hq.Longitude);
        }

        private Zipcode FindZipcodeFromZip(string zip)
        {
            return context.Zipcodes.First(z => z.Zip == zip);
        }
        private Zipcode FindHQZipcode()
        {
            return context.Zipcodes.First(z => z.Zip == HQZip);
        }

        // https://stormconsultancy.co.uk/blog/storm-news/the-haversine-formula-in-c-and-sql/
        private double Haversine(double x1, double x2, double y1, double y2)
        {
            double r = 3960;
            var lat = ConvertToRadians(x2 - x1);
            var lng = ConvertToRadians(y2 - y1);
            var h1 = Math.Sin(lat / 2) * Math.Sin(lat / 2) +
                  Math.Cos(ConvertToRadians(x1)) * Math.Cos(ConvertToRadians(x2)) *
                  Math.Sin(lng / 2) * Math.Sin(lng / 2);
            var h2 = 2 * Math.Asin(Math.Min(1, Math.Sqrt(h1)));
            return r * h2;
        }
        private double ConvertToRadians(double d)
        {
            return (Math.PI / 180) * d;
        }

        public void Save()
        {
            context.SaveChanges();
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
