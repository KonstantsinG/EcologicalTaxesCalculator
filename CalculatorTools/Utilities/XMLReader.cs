using CalculatorTools.Items;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CalculatorTools.Utilities
{
    public static class XMLReader
    {
        public static string GetPath(string storage, string type, DateTime date, string fileName)
        {
            string path = Path.Combine(storage, date.Year.ToString(), type);
            if (type == "month") path = Path.Combine(path, date.Month.ToString());
            path = Path.Combine(path, fileName);

            return path;
        }

        public static List<BackedProduct> ReadBackedProducts(string path)
        {
            XDocument doc = XDocument.Load(path);
            XElement root = doc.Root;

            List<BackedProduct> products = new List<BackedProduct>();
            XElement extraRye = root.Element("ExtraRyeFlour");
            XElement extraWheat = root.Element("ExtraWheatFlour");

            foreach (XElement productElement in root.Elements("BackedProduct"))
            {
                BackedProduct product = new BackedProduct
                {
                    Id = (int)productElement.Attribute("Id"),
                    Name = (string)productElement.Element("Name"),
                    RyeFlourRatio = (double)productElement.Element("RyeFlourRatio"),
                    WheatFlourRatio = (double)productElement.Element("WheatFlourRatio"),
                    RyeFlourCount = productElement.Element("RyeFlourCount") != null ? (double)productElement.Element("RyeFlourCount") : 0.0d,
                    WheatFlourCount = productElement.Element("WheatFlourCount") != null ? (double)productElement.Element("WheatFlourCount") : 0.0d
                };

                if (extraRye != null) product.ExtraRyeFlour = (double)extraRye;
                if (extraWheat != null) product.ExtraWheatFlour = (double)extraWheat;

                products.Add(product);
            }

            return products;
        }

        public static List<Furnace> ReadFurnaces(string path)
        {
            XDocument doc = XDocument.Load(path);
            XElement root = doc.Root;

            List<Furnace> furns = new List<Furnace>();

            foreach (XElement furnElement in root.Elements("Furnace"))
            {
                Furnace furn = new Furnace
                {
                    Id = (int)furnElement.Attribute("Id"),
                    Name = (string)furnElement.Element("Name"),
                    Place = (string)furnElement.Element("Place"),
                    FuelType = (string)furnElement.Element("FuelType"),
                    MinCombustionHeat = (double)furnElement.Element("MinCombustionHeat"),
                    FuelConsumptionOnMaxPower = (double)furnElement.Element("FuelConsumptionOnMaxPower"),
                    TheoreticalVolumeOfDryFlueGases = (double)furnElement.Element("TheoreticalVolumeOfDryFlueGases"),
                    WorkingTimePerYear = furnElement.Element("WorkingTimePerYear") != null ? (double)furnElement.Element("WorkingTimePerYear") : 0.0d,
                    FuelConsumptionPerYear = furnElement.Element("FuelConsumptionPerYear") != null ? (double)furnElement.Element("FuelConsumptionPerYear") : 0.0d
                };

                furns.Add(furn);
            }

            return furns;
        }

        public static List<SteamBoiler> ReadSteamBoilers(string path)
        {
            XDocument doc = XDocument.Load(path);
            XElement root = doc.Root;

            List<SteamBoiler> boils = new List<SteamBoiler>();

            foreach (XElement boilElement in root.Elements("SteamBoiler"))
            {
                SteamBoiler boil = new SteamBoiler
                {
                    Id = (int)boilElement.Attribute("Id"),
                    Name = (string)boilElement.Element("Name"),
                    Place = (string)boilElement.Element("Place"),
                    FuelType = (string)boilElement.Element("FuelType"),
                    MinCombustionHeat = (double)boilElement.Element("MinCombustionHeat"),
                    RatedLoad = (double)boilElement.Element("RatedLoad"),
                    EfficiencyGrossOfBoiler = (double)boilElement.Element("EfficiencyGrossOfBoiler"),
                    FuelConsumptionOnMaxPower = (double)boilElement.Element("FuelConsumptionOnMaxPower"),
                    TheoreticalVolumeOfDryFlueGases = (double)boilElement.Element("TheoreticalVolumeOfDryFlueGases"),
                    WorkingTimePerYear = boilElement.Element("WorkingTimePerYear") != null ? (double)boilElement.Element("WorkingTimePerYear") : 0.0d,
                    FuelConsumptionPerYear = boilElement.Element("FuelConsumptionPerYear") != null ? (double)boilElement.Element("FuelConsumptionPerYear") : 0.0d
                };

                boils.Add(boil);
            }

            return boils;
        }

        public static List<HotWaterBoiler> ReadHotWaterBoilers(string path)
        {
            XDocument doc = XDocument.Load(path);
            XElement root = doc.Root;

            List<HotWaterBoiler> boils = new List<HotWaterBoiler>();

            foreach (XElement boilElement in root.Elements("HotWaterBoiler"))
            {
                HotWaterBoiler boil = new HotWaterBoiler
                {
                    Id = (int)boilElement.Attribute("Id"),
                    Name = (string)boilElement.Element("Name"),
                    Place = (string)boilElement.Element("Place"),
                    FuelType = (string)boilElement.Element("FuelType"),
                    MinCombustionHeat = (double)boilElement.Element("MinCombustionHeat"),
                    RatedLoad = (double)boilElement.Element("RatedLoad"),
                    EfficiencyGrossOfBoiler = (double)boilElement.Element("EfficiencyGrossOfBoiler"),
                    FuelConsumptionOnMaxPower = (double)boilElement.Element("FuelConsumptionOnMaxPower"),
                    TheoreticalVolumeOfDryFlueGases = (double)boilElement.Element("TheoreticalVolumeOfDryFlueGases"),
                    WorkingTimePerYear = boilElement.Element("WorkingTimePerYear") != null ? (double)boilElement.Element("WorkingTimePerYear") : 0.0d,
                    FuelConsumptionPerYear = boilElement.Element("FuelConsumptionPerYear") != null ? (double)boilElement.Element("FuelConsumptionPerYear") : 0.0d
                };

                boils.Add(boil);
            }

            return boils;
        }

        public static List<TaxParam> ReadTaxParams(string path)
        {
            XDocument doc = XDocument.Load(path);
            List<TaxParam> taxParams = new List<TaxParam>();

            foreach (XElement el in doc.Root.Elements("TaxParam"))
            {
                TaxParam p = new TaxParam
                {
                    Name = (string)el.Element("Name"),
                    Rate = (double)el.Element("Rate"),
                    Limit = (double)el.Element("Limit")
                };

                taxParams.Add(p);
            }

            return taxParams;
        }

        private static List<string> SearchForPaths(string basePath, string itemName, string[] months)
        {
            List<string> paths = new List<string>();
            string newPath;

            for (int i = 0; i < months.Count(); i++)
            {
                newPath = Path.Combine(basePath, months[i]);

                if (Directory.Exists(newPath))
                    paths.Add(Path.Combine(newPath, itemName));
            }

            return paths;
        }

        public static List<T> GetItemsForYear<T>(DateTime date, string xmlName, Func<string, List<T>> readMethod)
        {
            List<T> boils = new List<T>();
            string[] months = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
            string path = Path.Combine(DataCollection.DataPath, date.Year.ToString(), "month");
            List<string> paths = SearchForPaths(path, xmlName, months);

            foreach (string p in paths)
                boils.AddRange(readMethod(p));

            return boils;
        }

        public static List<T> GetItemsForQuartal<T>(int quartal, DateTime date, string xmlName, Func<string, List<T>> readMethod)
        {
            List<T> boils = new List<T>();

            string[] months = EcologicalTaxesHandler.GetQuartalMonths(quartal);
            string basePath = Path.Combine(DataCollection.DataPath, date.Year.ToString(), "month");
            List<string> paths = SearchForPaths(basePath, xmlName, months);

            foreach (string p in paths)
                boils.AddRange(readMethod(p));

            return boils;
        }

        public static bool[] CheckForMonths(DateTime date, string[] months)
        {
            string basePath = Path.Combine(DataCollection.DataPath, date.Year.ToString(), "month");
            List<string> paths = SearchForPaths(basePath, "backedProducts.xml", months);

            string checkString;
            bool[] exists = new bool[months.Count()];
            for (int i = 0; i < months.Count(); i++)
            {
                checkString = Path.Combine(basePath, months[i], "backedProducts.xml");
                exists[i] = paths.Any(p => p.Contains(checkString));
            }

            return exists;
        }



        public static List<ReportsCollection> ReadReports()
        {
            List<ReportsCollection> collections = new List<ReportsCollection>();
            ReportsCollection rep;
            string monthsPath; string extraPath;

            foreach (string folder in Directory.EnumerateDirectories(DataCollection.DataPath))
            {
                rep = new ReportsCollection(folder.Split('\\').Last());
                monthsPath = Path.Combine(folder, "month");

                foreach (string month in Directory.EnumerateDirectories(monthsPath))
                    rep.MonthReports.Add(month.Split('\\').Last());

                extraPath = Path.Combine(DataCollection.DataPath, folder, "extra");
                (rep.QuartalReports, rep.YearReport) = ReadExtraReports(extraPath);
                rep.SortReports();

                collections.Add(rep);
            }

            return collections;
        }

        public static (List<string>, bool) ReadExtraReports(string path)
        {
            List<string> reps = new List<string>();
            bool yearReport = false;

            XDocument doc = XDocument.Load(Path.Combine(path, "extraReports.xml"));

            foreach (XElement el in doc.Root.Elements())
            {
                if (el.Name == "QuartalReport") reps.Add(el.Attribute("QuartalNumber").Value);
                else if (el.Name == "YearReport") yearReport = true;
            }

            return (reps, yearReport);
        }
    }
}
