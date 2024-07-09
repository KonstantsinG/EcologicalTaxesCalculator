using CalculatorTools.Items;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CalculatorTools.Utilities
{
    public static class EcologicalTaxesHandler
    {
        public enum ReportTypes
        {
            Month,
            Quartal,
            Year
        }

        public static DateTime CurrentDate { get; set; } = DateTime.Now;

        public static int CurrentQuartal { get; set; } = 1;

        public static double[] TaxesCoeffs => new double[] { 1, 1, 1, 1, 1, 1, 1 };

        public static double[] ReducedTaxes => new double[] { 0, 0, 0, 0, 0, 0, 0 };


        public static double[] GetStoredTaxRates()
        {
            string path = Path.Combine(DataCollection.DataPath, CurrentDate.Year.ToString(), "month", CurrentDate.Month.ToString(), "taxParams.xml");
            List<TaxParam> taxParams = XMLReader.ReadTaxParams(path);
            double[] rates = new double[taxParams.Count];

            for (int i = 0; i < taxParams.Count; i++) rates[i] = taxParams[i].Rate;

            return rates;
        }

        public static double[] GetStoredTaxLimits()
        {
            string path = Path.Combine(DataCollection.DataPath, CurrentDate.Year.ToString(), "month", CurrentDate.Month.ToString(), "taxParams.xml");
            List<TaxParam> taxParams = XMLReader.ReadTaxParams(path);
            double[] limits = new double[taxParams.Count];

            for (int i = 0; i < taxParams.Count; i++) limits[i] = taxParams[i].Limit;

            return limits;
        }

        private static double[] GetSummary(List<BackedProduct> prods, List<Furnace> furns, List<SteamBoiler> steamBoils, List<HotWaterBoiler> waterBoils)
        {
            return new double[]
            {
                    GetNitrogetIVGrossOut(furns, steamBoils, waterBoils),
                    GetNitrogetIIGrossOut(furns, steamBoils, waterBoils),
                    GetAceticAcidGrossOut(prods),
                    0.000,
                    GetEthanolGrossOut(prods),
                    GetFlourDustGrossOut(prods),
                    GetCarbonOxidGrossOut(furns, steamBoils, waterBoils)
            };
        }

        public static double[] GetCurrentOut()
        {
            List<BackedProduct> prods = DataCollection.Instance.BackedProducts;
            List<Furnace> furns = DataCollection.Instance.Furnaces;
            List<SteamBoiler> steamBoils = DataCollection.Instance.SteamBoilers;
            List<HotWaterBoiler> waterBoils = DataCollection.Instance.HotWaterBoilers;

            return GetSummary(prods, furns, steamBoils, waterBoils);
        }

        public static double[] GetStoredMonthOut()
        {
            List<BackedProduct> prods = XMLReader.ReadBackedProducts(Path.Combine(DataCollection.DataPath, CurrentDate.Year.ToString(), "month",
                                                                                  CurrentDate.Month.ToString(), "backedProducts.xml"));
            List<Furnace> furns = XMLReader.ReadFurnaces(Path.Combine(DataCollection.DataPath, CurrentDate.Year.ToString(), "month",
                                                                      CurrentDate.Month.ToString(), "furnaces.xml"));
            List<SteamBoiler> steamBoils = XMLReader.ReadSteamBoilers(Path.Combine(DataCollection.DataPath, CurrentDate.Year.ToString(), "month",
                                                                                   CurrentDate.Month.ToString(), "steamBoilers.xml"));

            List<HotWaterBoiler> waterBoils = XMLReader.ReadHotWaterBoilers(Path.Combine(DataCollection.DataPath, CurrentDate.Year.ToString(), "month",
                                                                                   CurrentDate.Month.ToString(), "hotWaterBoilers.xml"));

            return GetSummary(prods, furns, steamBoils, waterBoils);
        }

        public static double[] GetQuartalOut()
        {
            List<BackedProduct> prods = XMLReader.GetItemsForQuartal(CurrentQuartal, CurrentDate, "backedProducts.xml", XMLReader.ReadBackedProducts);
            List<Furnace> furns = XMLReader.GetItemsForQuartal(CurrentQuartal, CurrentDate, "furnaces.xml", XMLReader.ReadFurnaces);
            List<SteamBoiler> steamBoils = XMLReader.GetItemsForQuartal(CurrentQuartal, CurrentDate, "steamBoilers.xml", XMLReader.ReadSteamBoilers);
            List<HotWaterBoiler> waterBoils = XMLReader.GetItemsForQuartal(CurrentQuartal, CurrentDate, "hotWaterBoilers.xml", XMLReader.ReadHotWaterBoilers);

            return GetSummary(prods, furns, steamBoils, waterBoils);
        }

        public static double[] GetYearOut()
        {
            List<BackedProduct> prods = XMLReader.GetItemsForYear(CurrentDate, "backedProducts.xml", XMLReader.ReadBackedProducts);
            List<Furnace> furns = XMLReader.GetItemsForYear(CurrentDate, "furnaces.xml", XMLReader.ReadFurnaces);
            List<SteamBoiler> steamBoils = XMLReader.GetItemsForYear(CurrentDate, "steamBoilers.xml", XMLReader.ReadSteamBoilers);
            List<HotWaterBoiler> waterBoils = XMLReader.GetItemsForYear(CurrentDate, "hotWaterBoilers.xml", XMLReader.ReadHotWaterBoilers);

            return GetSummary(prods, furns, steamBoils, waterBoils);
        }

        public static string GetQuartalString(string num)
        {
            switch (num)
            {
                case "1": return "Первый квартал";
                case "2": return "Второй квартал";
                case "3": return "Третий квартал";
                case "4": return "Четвёртый квартал";
                default: return "";
            }
        }

        public static string GetMonthString(string num)
        {
            switch (num)
            {
                case "1": return "Январь";
                case "2": return "Февраль";
                case "3": return "Март";
                case "4": return "Апрель";
                case "5": return "Май";
                case "6": return "Июнь";
                case "7": return "Июль";
                case "8": return "Август";
                case "9": return "Сентябрь";
                case "10": return "Октябрь";
                case "11": return "Ноябрь";
                case "12": return "Декабрь";
                default: return "";
            }
        }

        public static string[] GetQuartalMonths(int num)
        {
            switch (num)
            {
                case 1: return new string[] { "1", "2", "3" };
                case 2: return new string[] { "4", "5", "6" };
                case 3: return new string[] { "7", "8", "9" };
                case 4: return new string[] { "10", "11", "12" };
                default: return new string[] { };
            }
        }

        public static double GetCarbonOxidGrossOut(List<Furnace> furns, List<SteamBoiler> steamBoils, List<HotWaterBoiler> waterBoils)
        {
            if (furns.Count == 0 && steamBoils.Count == 0 && waterBoils.Count == 0) return 0;

            double sum = 0;
            sum += furns.Sum(f => f.CarbonOxidGrossOut);
            sum += steamBoils.Sum(b => b.CarbonOxidGrossOut);
            sum += waterBoils.Sum(b => b.CarbonOxidGrossOut);

            return sum;
        }

        public static double GetNitrogetIVGrossOut(List<Furnace> furns, List<SteamBoiler> steamBoils, List<HotWaterBoiler> waterBoils)
        {
            if (furns.Count == 0 && steamBoils.Count == 0 && waterBoils.Count == 0) return 0;

            double sum = 0;
            sum += furns.Sum(f => f.NitrogenOxidIVGrossOut);
            sum += steamBoils.Sum(b => b.NitrogenOxidIVGrossOut);
            sum += waterBoils.Sum(b => b.NitrogenOxidIVGrossOut);

            return sum;
        }

        public static double GetNitrogetIIGrossOut(List<Furnace> furns, List<SteamBoiler> steamBoils, List<HotWaterBoiler> waterBoils)
        {
            if (furns.Count == 0 && steamBoils.Count == 0 && waterBoils.Count == 0) return 0;

            double sum = 0;
            sum += furns.Sum(f => f.NitrogenOxidIIGrossOut);
            sum += steamBoils.Sum(b => b.NitrogenOxidIIGrossOut);
            sum += waterBoils.Sum(b => b.NitrogenOxidIIGrossOut);

            return sum;
        }

        public static double GetEthanolGrossOut(List<BackedProduct> prods)
        {
            if (prods.Count == 0) return 0;

            double wheatSum = prods.Sum(bp => bp.WheatFlourCount) + prods[0].ExtraWheatFlour;
            double ryeSum = prods.Sum(bp => bp.RyeFlourCount) + prods[0].ExtraRyeFlour;

            return Math.Round((BackedProduct.EthanolWheatCoeff * wheatSum + BackedProduct.EthanolRyeCoeff * ryeSum) / 1000, 6);
        }

        public static double GetAceticAcidGrossOut(List<BackedProduct> prods)
        {
            if (prods.Count == 0) return 0;

            double wheatSum = prods.Sum(bp => bp.WheatFlourCount) + prods[0].ExtraWheatFlour;
            double ryeSum = prods.Sum(bp => bp.RyeFlourCount) + prods[0].ExtraRyeFlour;

            return Math.Round((BackedProduct.AceticAcidWheatCoeff * wheatSum + BackedProduct.AceticAcidRyeCoeff * ryeSum) / 1000, 6);
        }

        public static double GetFlourDustGrossOut(List<BackedProduct> prods)
        {
            if (prods.Count == 0) return 0;

            double wheatSum = prods.Sum(bp => bp.WheatFlourCount) + prods[0].ExtraWheatFlour;
            double ryeSum = prods.Sum(bp => bp.RyeFlourCount) + prods[0].ExtraRyeFlour;

            return Math.Round((BackedProduct.FlourDustWheatCoeff * wheatSum + BackedProduct.FlourDustRyeCoeff * ryeSum) / 1000, 6);
        }
    }
}
