using CalculatorTools.Interfaces;
using CalculatorTools.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CalculatorTools.Utilities
{
    public static class XMLWriter
    {
        public static string BackedProductsPath
        {
            get => Path.Combine(DataCollection.CachePath, "backedProducts.xml");
        }

        public static string FurnacesPath
        {
            get => Path.Combine(DataCollection.CachePath, "furnaces.xml");
        }

        public static string SteamBoilersPath
        {
            get => Path.Combine(DataCollection.CachePath, "steamBoilers.xml");
        }

        public static string HotWaterBoilersPath
        {
            get => Path.Combine(DataCollection.CachePath, "hotWaterBoilers.xml");
        }

        public static string TaxParamsPath
        {
            get => Path.Combine(DataCollection.CachePath, "taxParams.xml");
        }

        static XMLWriter()
        {
            if (!File.Exists(BackedProductsPath))
                CreateXmlDocument("BackedProducts", BackedProductsPath);
            if (!File.Exists(FurnacesPath))
                CreateXmlDocument("Furnaces", FurnacesPath);
            if (!File.Exists(SteamBoilersPath))
                CreateXmlDocument("SteamBoilers", SteamBoilersPath);
            if (!File.Exists(HotWaterBoilersPath))
                CreateXmlDocument("HotWaterBoilers", HotWaterBoilersPath);
        }

        private static void CreateXmlDocument(string name, string path)
        {
            XmlDocument document = new XmlDocument();
            XmlElement rootElement = document.CreateElement(name);
            document.AppendChild(rootElement);
            document.Save(path);
        }

        public static void WriteItem<T>(T item, string path) where T : ISerializableItem
        {
            XDocument doc = XDocument.Load(path);
            doc.Root.Add(item.ToXElement());
            doc.Save(path);
        }

        public static void RemoveItem<T>(T item, string path) where T : ISerializableItem
        {
            XDocument doc = XDocument.Load(path);

            var i = doc.Root.Elements().FirstOrDefault(b => (int)b.Attribute("Id") == item.Id);
            if (i != null) i.Remove();

            doc.Save(path);
        }

        public static void UpdateBackedProduct(BackedProduct product)
        {
            XDocument doc = XDocument.Load(BackedProductsPath);

            var prod = doc.Root.Elements().FirstOrDefault(b => (int)b.Attribute("Id") == product.Id);
            if (prod != null)
            {
                if (prod.Element("Name").Value != product.Name)
                    prod.Element("Name").Value = product.Name;
                if (prod.Element("RyeFlourRatio").Value != product.RyeFlourRatio.ToString().Replace(",", "."))
                    prod.Element("RyeFlourRatio").Value = product.RyeFlourRatio.ToString().Replace(",", ".");
                if (prod.Element("WheatFlourRatio").Value != product.WheatFlourRatio.ToString().Replace(",", "."))
                    prod.Element("WheatFlourRatio").Value = product.WheatFlourRatio.ToString().Replace(",", ".");
            }

            doc.Save(BackedProductsPath);
        }

        public static void UpdateFurnace(Furnace furn)
        {
            XDocument doc = XDocument.Load(FurnacesPath);

            var furnace = doc.Root.Elements().FirstOrDefault(f => (int)f.Attribute("Id") == furn.Id);
            if (furnace != null)
            {
                if (furnace.Element("Name").Value != furn.Name)
                    furnace.Element("Name").Value = furn.Name;
                if (furnace.Element("Place").Value != furn.Place)
                    furnace.Element("Place").Value = furn.Place;
                if (furnace.Element("FuelType").Value != furn.FuelType)
                    furnace.Element("FuelType").Value = furn.FuelType;
                if (furnace.Element("MinCombustionHeat").Value != furn.MinCombustionHeat.ToString().Replace(",", "."))
                    furnace.Element("MinCombustionHeat").Value = furn.MinCombustionHeat.ToString().Replace(",", ".");
                if (furnace.Element("FuelConsumptionOnMaxPower").Value != furn.FuelConsumptionOnMaxPower.ToString().Replace(",", "."))
                    furnace.Element("FuelConsumptionOnMaxPower").Value = furn.FuelConsumptionOnMaxPower.ToString().Replace(",", ".");
                if (furnace.Element("TheoreticalVolumeOfDryFlueGases").Value != furn.TheoreticalVolumeOfDryFlueGases.ToString().Replace(",", "."))
                    furnace.Element("TheoreticalVolumeOfDryFlueGases").Value = furn.TheoreticalVolumeOfDryFlueGases.ToString().Replace(",", ".");
            }

            doc.Save(FurnacesPath);
        }

        public static void UpdateSteamBoiler(SteamBoiler boil)
        {
            XDocument doc = XDocument.Load(SteamBoilersPath);

            var boiler = doc.Root.Elements().FirstOrDefault(b => (int)b.Attribute("Id") == boil.Id);
            if (boiler != null)
            {
                if (boiler.Element("Name").Value != boil.Name)
                    boiler.Element("Name").Value = boil.Name;
                if (boiler.Element("Place").Value != boil.Place)
                    boiler.Element("Place").Value = boil.Place;
                if (boiler.Element("FuelType").Value != boil.FuelType)
                    boiler.Element("FuelType").Value = boil.FuelType;
                if (boiler.Element("MinCombustionHeat").Value != boil.MinCombustionHeat.ToString().Replace(",", "."))
                    boiler.Element("MinCombustionHeat").Value = boil.MinCombustionHeat.ToString().Replace(",", ".");
                if (boiler.Element("FuelConsumptionOnMaxPower").Value != boil.FuelConsumptionOnMaxPower.ToString().Replace(",", "."))
                    boiler.Element("FuelConsumptionOnMaxPower").Value = boil.FuelConsumptionOnMaxPower.ToString().Replace(",", ".");
                if (boiler.Element("TheoreticalVolumeOfDryFlueGases").Value != boil.TheoreticalVolumeOfDryFlueGases.ToString().Replace(",", "."))
                    boiler.Element("TheoreticalVolumeOfDryFlueGases").Value = boil.TheoreticalVolumeOfDryFlueGases.ToString().Replace(",", ".");
                if (boiler.Element("RatedLoad").Value != boil.RatedLoad.ToString().Replace(",", "."))
                    boiler.Element("RatedLoad").Value = boil.RatedLoad.ToString().Replace(",", ".");
                if (boiler.Element("EfficiencyGrossOfBoiler").Value != boil.EfficiencyGrossOfBoiler.ToString().Replace(",", "."))
                    boiler.Element("EfficiencyGrossOfBoiler").Value = boil.EfficiencyGrossOfBoiler.ToString().Replace(",", ".");
            }

            doc.Save(SteamBoilersPath);
        }

        public static void UpdateHotWaterBoiler(HotWaterBoiler boil)
        {
            XDocument doc = XDocument.Load(HotWaterBoilersPath);

            var boiler = doc.Root.Elements().FirstOrDefault(b => (int)b.Attribute("Id") == boil.Id);
            if (boiler != null)
            {
                if (boiler.Element("Name").Value != boil.Name)
                    boiler.Element("Name").Value = boil.Name;
                if (boiler.Element("Place").Value != boil.Place)
                    boiler.Element("Place").Value = boil.Place;
                if (boiler.Element("FuelType").Value != boil.FuelType)
                    boiler.Element("FuelType").Value = boil.FuelType;
                if (boiler.Element("MinCombustionHeat").Value != boil.MinCombustionHeat.ToString().Replace(",", "."))
                    boiler.Element("MinCombustionHeat").Value = boil.MinCombustionHeat.ToString().Replace(",", ".");
                if (boiler.Element("FuelConsumptionOnMaxPower").Value != boil.FuelConsumptionOnMaxPower.ToString().Replace(",", "."))
                    boiler.Element("FuelConsumptionOnMaxPower").Value = boil.FuelConsumptionOnMaxPower.ToString().Replace(",", ".");
                if (boiler.Element("TheoreticalVolumeOfDryFlueGases").Value != boil.TheoreticalVolumeOfDryFlueGases.ToString().Replace(",", "."))
                    boiler.Element("TheoreticalVolumeOfDryFlueGases").Value = boil.TheoreticalVolumeOfDryFlueGases.ToString().Replace(",", ".");
                if (boiler.Element("RatedLoad").Value != boil.RatedLoad.ToString().Replace(",", "."))
                    boiler.Element("RatedLoad").Value = boil.RatedLoad.ToString().Replace(",", ".");
                if (boiler.Element("EfficiencyGrossOfBoiler").Value != boil.EfficiencyGrossOfBoiler.ToString().Replace(",", "."))
                    boiler.Element("EfficiencyGrossOfBoiler").Value = boil.EfficiencyGrossOfBoiler.ToString().Replace(",", ".");
            }

            doc.Save(HotWaterBoilersPath);
        }

        public static void UpdateTaxParams(List<TaxParam> taxParams)
        {
            XDocument doc = XDocument.Load(TaxParamsPath);

            for (int i = 0; i < taxParams.Count; i++)
            {
                doc.Root.Elements().ElementAt(i).Element("Rate").Value = taxParams[i].Rate.ToString().Replace(",", ".");
                doc.Root.Elements().ElementAt(i).Element("Limit").Value = taxParams[i].Limit.ToString().Replace(",", ".");
            }

            doc.Save(TaxParamsPath);
        }



        private static void CreateYearFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Directory.CreateDirectory(Path.Combine(path, "month"));
                Directory.CreateDirectory(Path.Combine(path, "extra"));
                CreateXmlDocument("ExtraReports", Path.Combine(path, "extra", "extraReports.xml"));
            }
        }

        private static void MakeDirectory(string path)
        {
            if (Directory.Exists(path))
                Directory.Delete(path, true);

            Directory.CreateDirectory(path);
        }

        private static void SaveItemResults<T>(List<T> items, string path, string xmlName) where T : ISerializableItem
        {
            CreateXmlDocument(xmlName, path);
            XDocument doc = XDocument.Load(path);

            foreach (T i in items) doc.Root.Add(i.ToResultsXElement());

            doc.Save(path);
        }

        private static void SaveBackedProductsExtra(List<BackedProduct> prods, string path)
        {
            XDocument doc = XDocument.Load(path);

            doc.Root.Add(new XElement("ExtraWheatFlour", prods[0].ExtraWheatFlour));
            doc.Root.Add(new XElement("ExtraRyeFlour", prods[0].ExtraRyeFlour));

            doc.Save(path);
        }

        public static void SaveMonthReport(List<BackedProduct> prods, List<Furnace> furns, List<SteamBoiler> steamBoils,
                                       List<HotWaterBoiler> waterBoils, List<TaxParam> taxParams, DateTime date)
        {
            string path = Path.Combine(DataCollection.DataPath, date.Year.ToString());
            CreateYearFolder(path);

            path = Path.Combine(path, "month", date.Month.ToString());
            MakeDirectory(path);

            string backProdsPath = Path.Combine(path, "backedProducts.xml");
            SaveItemResults(prods, backProdsPath, "BackedProducts");
            SaveBackedProductsExtra(prods, backProdsPath);

            string furnsPath = Path.Combine(path, "furnaces.xml");
            SaveItemResults(furns, furnsPath, "Furnaces");

            string steamBoilsPath = Path.Combine(path, "steamBoilers.xml");
            SaveItemResults(steamBoils, steamBoilsPath, "SteamBoilers");

            string hotWaterBoilsPath = Path.Combine(path, "hotWaterBoilers.xml");
            SaveItemResults(waterBoils, hotWaterBoilsPath, "HotWaterBoilers");

            string taxParamsPath = Path.Combine(path, "taxParams.xml");
            SaveTaxParams(taxParams, taxParamsPath);
        }

        private static void SaveTaxParams(List<TaxParam> taxParams, string path)
        {
            CreateXmlDocument("TaxParams", path);
            XDocument doc = XDocument.Load(path);

            foreach (TaxParam p in taxParams) doc.Root.Add(p.ToXElement());

            doc.Save(path);
        }

        public static void SaveQuartalReport(DateTime date, string[] months, string quartalNumber)
        {
            string path = Path.Combine(DataCollection.DataPath, date.Year.ToString());
            CreateYearFolder(path);

            path = Path.Combine(path, "extra", "extraReports.xml");
            XDocument doc = XDocument.Load(path);
            if (doc.Root.Elements().Any(e => e.Name == "QuartalReport" && e.Attribute("QuartalNumber").Value == quartalNumber))
                doc.Root.Elements().Where(e => e.Name == "QuartalReport" && e.Attribute("QuartalNumber").Value == quartalNumber).Remove();

            XElement quartalReport = new XElement("QuartalReport", 
                            new XAttribute("QuartalNumber", quartalNumber),
                            new XElement("Month", months[0]),
                            new XElement("Month", months[1]),
                            new XElement("Month", months[2]));
            doc.Root.Add(quartalReport);

            doc.Save(path);
        }

        public static void SaveYearReport(DateTime date)
        {
            string path = Path.Combine(DataCollection.DataPath, date.Year.ToString());
            CreateYearFolder(path);

            path = Path.Combine(path, "extra", "extraReports.xml");
            XDocument doc = XDocument.Load (path);

            if (doc.Root.Elements().Any(e => e.Name == "YearReport")) doc.Root.Elements().Where(e => e.Name == "YearReport").Remove();
            doc.Root.Add(new XElement("YearReport"));

            doc.Save(path);
        }
    }
}
