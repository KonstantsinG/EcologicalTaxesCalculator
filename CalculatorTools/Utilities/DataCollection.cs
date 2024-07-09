using CalculatorTools.Items;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace CalculatorTools.Utilities
{
    public class DataCollection
    {
        private static readonly DataCollection _instance = new DataCollection();

        public static DataCollection Instance
        {
            get => _instance;
        }

        public static string AppDataPath
        {
            get
            {
                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string appFolderPath = Path.Combine(appDataPath, "EcologicalTaxesCalculator");
                return appFolderPath;
            }
        }

        public static string CachePath
        {
            get => Path.Combine(AppDataPath, "cache");
        }

        public static string DataPath
        {
            get => Path.Combine(AppDataPath, "data");
        }

        public static void SetupAppData()
        {
            if (!Directory.Exists(CachePath))
                Directory.CreateDirectory(CachePath);

            if (!Directory.Exists(DataPath))
                Directory.CreateDirectory(DataPath);
        }

        private List<BackedProduct> _backedProducts = null;

        public List<BackedProduct> BackedProducts
        {
            get
            {
                if (_backedProducts == null)
                    _backedProducts = XMLReader.ReadBackedProducts(XMLWriter.BackedProductsPath);

                return _backedProducts;
            }
            private set => _backedProducts = value;
        }

        private List<Furnace> _furnaces = null;

        public List<Furnace> Furnaces
        {
            get
            {
                if (_furnaces == null)
                    _furnaces = XMLReader.ReadFurnaces(XMLWriter.FurnacesPath);

                return _furnaces;
            }
            private set => _furnaces = value;
        }

        private List<SteamBoiler> _steamBoilers = null;

        public List<SteamBoiler> SteamBoilers
        {
            get
            {
                if (_steamBoilers == null)
                    _steamBoilers= XMLReader.ReadSteamBoilers(XMLWriter.SteamBoilersPath);

                return _steamBoilers;
            }
            private set => _steamBoilers = value;
        }

        private List<HotWaterBoiler> _hotWaterBoilers = null;

        public List<HotWaterBoiler> HotWaterBoilers
        {
            get
            {
                if (_hotWaterBoilers == null)
                    _hotWaterBoilers = XMLReader.ReadHotWaterBoilers(XMLWriter.HotWaterBoilersPath);

                return _hotWaterBoilers;
            }
            private set => _hotWaterBoilers = value;
        }

        private List<TaxParam> _taxParams = null;

        public List<TaxParam> TaxParams
        {
            get
            {
                if (_taxParams == null)
                    _taxParams = XMLReader.ReadTaxParams(XMLWriter.TaxParamsPath);

                return _taxParams;
            }
            set => _taxParams = value;
        }

        public double[] TaxRates
        {
            get
            {
                double[] rates = new double[TaxParams.Count];
                for (int i = 0; i < TaxParams.Count; i++) rates[i] = TaxParams[i].Rate;

                return rates;
            }
        }

        public double[] TaxLimits
        {
            get
            {
                double[] limits = new double[TaxParams.Count];
                for (int i = 0; i < TaxParams.Count; i++) limits[i] = TaxParams[i].Limit;

                return limits;
            }
        }


        public static void AddBackedProduct(BackedProduct product)
        {
            foreach (BackedProduct prod in Instance.BackedProducts)
            {
                if (prod.Equals(product))
                    throw new Exception("Хлебобулочное изделие с такими данными уже существует.");
            }

            Instance.BackedProducts.Add(product);
            XMLWriter.WriteItem(product, XMLWriter.BackedProductsPath);
        }

        public static BackedProduct FindBackedProduct(string[] data)
        {
            foreach (BackedProduct bp in Instance.BackedProducts)
            {
                if (bp.GetHashCode() != BackedProduct.GetHashCode(data[0], double.Parse(data[1]), double.Parse(data[2]))) continue;

                return bp;
            }

            return null;
        }

        public static void UpdateBackedProduct(BackedProduct product, string[] data)
        {
            BackedProduct tempProd = new BackedProduct(data[0], double.Parse(data[1]), double.Parse(data[2]));
            if (product.Equals(tempProd)) return;

            foreach (BackedProduct prod in Instance.BackedProducts)
            {
                if (prod.Equals(tempProd))
                    throw new Exception("Хлебобулочное изделие с такими данными уже существует.");
            }

            if (product.Name != tempProd.Name) product.Name = tempProd.Name;
            if (product.RyeFlourRatio != tempProd.RyeFlourRatio) product.RyeFlourRatio = tempProd.RyeFlourRatio;
            if (product.WheatFlourRatio != tempProd.WheatFlourRatio) product.WheatFlourRatio = tempProd.WheatFlourRatio;

            XMLWriter.UpdateBackedProduct(product);
        }

        public static void RemoveBackedProduct(BackedProduct product)
        {
            Instance.BackedProducts.Remove(product);
            XMLWriter.RemoveItem(product, XMLWriter.BackedProductsPath);
        }

        public static Furnace FindFurnace(string[] data)
        {
            foreach (Furnace furn in Instance.Furnaces)
            {
                if (furn.GetHashCode() != Furnace.GetHashCode(data[0], data[1], data[2], double.Parse(data[3]), double.Parse(data[4]), double.Parse(data[5]))) continue;

                return furn;
            }

            return null;
        }

        public static void AddFurnace(Furnace furn)
        {
            foreach (Furnace f in Instance.Furnaces)
            {
                if (furn.Equals(f))
                    throw new Exception("Печь с такими данными уже существует.");
            }

            Instance.Furnaces.Add(furn);
            XMLWriter.WriteItem(furn, XMLWriter.FurnacesPath);
        }

        public static void UpdateFurnace(Furnace furn, string[] data)
        {
            Furnace tempFurn = new Furnace(data[1], data[0], data[2], double.Parse(data[3]), double.Parse(data[4]), double.Parse(data[5]));
            if (furn.Equals(tempFurn)) return;

            foreach (Furnace f in Instance.Furnaces)
            {
                if (f.Equals(tempFurn))
                    throw new Exception("Печь с такими данными уже существует.");
            }

            if (furn.Name != tempFurn.Name) furn.Name = tempFurn.Name;
            if (furn.Place != tempFurn.Place) furn.Place = tempFurn.Place;
            if (furn.FuelType != tempFurn.FuelType) furn.FuelType = tempFurn.FuelType;
            if (furn.MinCombustionHeat != tempFurn.MinCombustionHeat) furn.MinCombustionHeat = tempFurn.MinCombustionHeat;
            if (furn.FuelConsumptionOnMaxPower != tempFurn.FuelConsumptionOnMaxPower) furn.FuelConsumptionOnMaxPower = tempFurn.FuelConsumptionOnMaxPower;
            if (furn.TheoreticalVolumeOfDryFlueGases != tempFurn.TheoreticalVolumeOfDryFlueGases) furn.TheoreticalVolumeOfDryFlueGases = tempFurn.TheoreticalVolumeOfDryFlueGases;

            XMLWriter.UpdateFurnace(furn);
        }

        public static void RemoveFurnace(Furnace furn)
        {
            Instance.Furnaces.Remove(furn);
            XMLWriter.RemoveItem(furn, XMLWriter.FurnacesPath);
        }

        public static void AddSteamBoiler(SteamBoiler boiler)
        {
            foreach (SteamBoiler boil in Instance.SteamBoilers)
            {
                if (boiler.Equals(boil))
                    throw new Exception("Паровой котёл с такими данными уже существует.");
            }

            Instance.SteamBoilers.Add(boiler);
            XMLWriter.WriteItem(boiler, XMLWriter.SteamBoilersPath);
        }

        public static SteamBoiler FindSteamBoiler(string[] data)
        {
            foreach (SteamBoiler boiler in Instance.SteamBoilers)
            {
                if (boiler.GetHashCode() != SteamBoiler.GetHashCode(data[1], data[0], data[2], double.Parse(data[3]), double.Parse(data[6]), double.Parse(data[7]), 
                                                                    double.Parse(data[4]), double.Parse(data[5]))) continue;

                return boiler;
            }

            return null;
        }

        public static void UpdateSteamBoiler(SteamBoiler boiler, string[] data)
        {
            SteamBoiler tempBoil = new SteamBoiler(data[1], data[0], data[2], double.Parse(data[3]), double.Parse(data[6]), double.Parse(data[7]), double.Parse(data[4]), double.Parse(data[5]));
            if (boiler.Equals(tempBoil)) return;

            foreach (SteamBoiler b in Instance.SteamBoilers)
            {
                if (b.Equals(tempBoil))
                    throw new Exception("Паровой котёл с такими данными уже существует.");
            }

            if (boiler.Name != tempBoil.Name) boiler.Name = tempBoil.Name;
            if (boiler.Place != tempBoil.Place) boiler.Place = tempBoil.Place;
            if (boiler.FuelType != tempBoil.FuelType) boiler.FuelType = tempBoil.FuelType;
            if (boiler.MinCombustionHeat != tempBoil.MinCombustionHeat) boiler.MinCombustionHeat = tempBoil.MinCombustionHeat;
            if (boiler.RatedLoad != tempBoil.RatedLoad) boiler.RatedLoad = tempBoil.RatedLoad;
            if (boiler.EfficiencyGrossOfBoiler != tempBoil.EfficiencyGrossOfBoiler) boiler.EfficiencyGrossOfBoiler = tempBoil.EfficiencyGrossOfBoiler;
            if (boiler.FuelConsumptionOnMaxPower != tempBoil.FuelConsumptionOnMaxPower) boiler.FuelConsumptionOnMaxPower = tempBoil.FuelConsumptionOnMaxPower;
            if (boiler.TheoreticalVolumeOfDryFlueGases != tempBoil.TheoreticalVolumeOfDryFlueGases) boiler.TheoreticalVolumeOfDryFlueGases = tempBoil.TheoreticalVolumeOfDryFlueGases;

            XMLWriter.UpdateSteamBoiler(boiler);
        }

        public static void RemoveSteamBoiler(SteamBoiler boiler)
        {
            Instance.SteamBoilers.Remove(boiler);
            XMLWriter.RemoveItem(boiler, XMLWriter.SteamBoilersPath);
        }

        public static void AddHotWaterBoiler(HotWaterBoiler boiler)
        {
            foreach (HotWaterBoiler boil in Instance.HotWaterBoilers)
            {
                if (boiler.Equals(boil))
                    throw new Exception("Водонагревательный котёл с такими данными уже существует.");
            }

            Instance.HotWaterBoilers.Add(boiler);
            XMLWriter.WriteItem(boiler, XMLWriter.HotWaterBoilersPath);
        }

        public static HotWaterBoiler FindHotWaterBoiler(string[] data)
        {
            foreach (HotWaterBoiler boiler in Instance.HotWaterBoilers)
            {
                if (boiler.GetHashCode() != HotWaterBoiler.GetHashCode(data[1], data[0], data[2], double.Parse(data[3]), double.Parse(data[6]), double.Parse(data[7]), 
                                                                       double.Parse(data[4]), double.Parse(data[5]))) continue;

                return boiler;
            }

            return null;
        }

        public static void UpdateHotWaterBoiler(HotWaterBoiler boiler, string[] data)
        {
            HotWaterBoiler tempBoil = new HotWaterBoiler(data[1], data[0], data[2], double.Parse(data[3]), double.Parse(data[6]), double.Parse(data[7]), double.Parse(data[4]), double.Parse(data[5]));
            if (boiler.Equals(tempBoil)) return;

            foreach (HotWaterBoiler b in Instance.HotWaterBoilers)
            {
                if (b.Equals(tempBoil))
                    throw new Exception("Водонагревательный котёл с такими данными уже существует.");
            }

            if (boiler.Name != tempBoil.Name) boiler.Name = tempBoil.Name;
            if (boiler.Place != tempBoil.Place) boiler.Place = tempBoil.Place;
            if (boiler.FuelType != tempBoil.FuelType) boiler.FuelType = tempBoil.FuelType;
            if (boiler.MinCombustionHeat != tempBoil.MinCombustionHeat) boiler.MinCombustionHeat = tempBoil.MinCombustionHeat;
            if (boiler.RatedLoad != tempBoil.RatedLoad) boiler.RatedLoad = tempBoil.RatedLoad;
            if (boiler.EfficiencyGrossOfBoiler != tempBoil.EfficiencyGrossOfBoiler) boiler.EfficiencyGrossOfBoiler = tempBoil.EfficiencyGrossOfBoiler;
            if (boiler.FuelConsumptionOnMaxPower != tempBoil.FuelConsumptionOnMaxPower) boiler.FuelConsumptionOnMaxPower = tempBoil.FuelConsumptionOnMaxPower;
            if (boiler.TheoreticalVolumeOfDryFlueGases != tempBoil.TheoreticalVolumeOfDryFlueGases) boiler.TheoreticalVolumeOfDryFlueGases = tempBoil.TheoreticalVolumeOfDryFlueGases;

            XMLWriter.UpdateHotWaterBoiler(boiler);
        }

        public static void RemoveHotWaterBoiler(HotWaterBoiler boiler)
        {
            Instance.HotWaterBoilers.Remove(boiler);
            XMLWriter.RemoveItem(boiler, XMLWriter.HotWaterBoilersPath);
        }
    }
}
