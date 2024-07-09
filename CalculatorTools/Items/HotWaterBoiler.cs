﻿using CalculatorTools.Utilities;
using System;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace CalculatorTools.Items
{
    public class HotWaterBoiler : ProductionEquipment
    {
        private static int _currentId = 0;

        private int _id;

        public override int Id
        {
            get => _id;
            set
            {
                _id = value;
                if (_id >= _currentId) _currentId = _id + 1;
            }
        }

        public double RatedLoad { get; set; } // const

        public double EfficiencyGrossOfBoiler { get; set; } // const

        public double ActualFuelConsumption
        {
            get
            {
                return Math.Round(RatedLoad / MinCombustionHeat / EfficiencyGrossOfBoiler * 100.0, 4);
            }
        }
        public HotWaterBoiler() : base()
        {
            TypeName = "Котёл водогрейный";
            _id = _currentId;
            _currentId++;
        }


        public HotWaterBoiler(string place, string name, string fuelType, double minCombustionHeat, double fuelConsumptionOnMaxPower,
                                   double theoreticalVolumeOfDryFlueGases, double ratedLoad, double efficiencyGrossOfBoiler) :
            base(place, name, fuelType, minCombustionHeat, fuelConsumptionOnMaxPower, theoreticalVolumeOfDryFlueGases)
        {
            TypeName = "Котёл водогрейный";
            RatedLoad = ratedLoad;
            EfficiencyGrossOfBoiler = efficiencyGrossOfBoiler;

            _id = _currentId;
            _currentId++;
        }


        // 1.Carbon Oxid emissions calculations

        public double HeatLossDueToChemicalIncompleteCombustionOfFuel_ForGross { get; set; } = 0.07; // const

        public double HeatLossDueToChemicalIncompleteCombustionOfFuel_ForMax { get; set; } = 0.09; // const

        public double HeatLossDueToChemicalIncompleteCombustionOfFuel_Coeff { get; set; } = 0.5; // const

        public double CarbonOxidOut_ForGross
        {
            get
            {
                return Math.Round(HeatLossDueToChemicalIncompleteCombustionOfFuel_ForGross * HeatLossDueToChemicalIncompleteCombustionOfFuel_Coeff * MinCombustionHeat, 3);
            }
        }

        public double CarbonOxidOut_ForMax
        {
            get
            {
                return Math.Round(HeatLossDueToChemicalIncompleteCombustionOfFuel_ForMax * HeatLossDueToChemicalIncompleteCombustionOfFuel_Coeff * MinCombustionHeat, 3);
            }
        }

        public double CarbonOxidGrossOut
        {
            get
            {
                return Math.Round(FuelConsumptionPerYear * CarbonOxidOut_ForGross / 1000, 6);
            }
        }

        public double CarbonOxidMaxCount
        {
            get
            {
                return Math.Round(FuelConsumptionOnMaxPower * CarbonOxidOut_ForMax, 3);
            }
        }

        public double CalculatedMaxCOConcentration
        {
            get
            {
                return Math.Round(1000.0 * CarbonOxidMaxCount / TheoreticalVolumeOfDryFlueGases / FuelConsumptionOnMaxPower, 1);
            }
        }

        public double CarbonOxidMaxEmission_Norm
        {
            get
            {
                return Math.Round(95 * TheoreticalVolumeOfDryFlueGases * FuelConsumptionOnMaxPower / 1000.0, 3);
            }
        }


        // 2.Nitrogen oxid emissions calculations

        public double NitrogenOxidSpecificEmission
        {
            get
            {
                return 0.0113 * Math.Sqrt(0.86 * FuelConsumptionOnMaxPower * MinCombustionHeat) + 0.03;
            }
        }

        public string NitrogenOxidSpecificEmissionString
        { get => Math.Round(NitrogenOxidSpecificEmission, 6).ToString(); }

        public double NitrogenOxidSpecificMeanEmission
        {
            get
            {
                return 0.0113 * Math.Sqrt(0.86 * FuelConsumptionOnMeanPower * MinCombustionHeat) + 0.03;
            }
        }

        public string NitrogenOxidSpecificMeanEmissionString
        { get => Math.Round(NitrogenOxidSpecificMeanEmission, 6).ToString(); }

        public double BurnerConstructionCoeff { get; set; } = 1.0; // const

        public double AirTemperatureForBurnerCoeff { get; set; } = 0.98; // const

        public double FlueGasRecirculationCoeff { get; set; } = 1.0; // const

        public double SteppedAirInputIntoCombustionChamberCoeff { get; set; } = 1.0; // const

        public double NitrogenOxidMaxOnNO2
        {
            get
            {
                return Math.Round(FuelConsumptionOnMaxPower * MinCombustionHeat * NitrogenOxidSpecificEmission * BurnerConstructionCoeff * AirTemperatureForBurnerCoeff *
                                  FlueGasRecirculationCoeff * SteppedAirInputIntoCombustionChamberCoeff, 3);
            }
        }

        public double NitrogetOxidsGrossOut
        {
            get
            {
                return Math.Round(0.001 * FuelConsumptionPerYear * MinCombustionHeat * NitrogenOxidSpecificMeanEmission * BurnerConstructionCoeff * AirTemperatureForBurnerCoeff *
                                  FlueGasRecirculationCoeff * SteppedAirInputIntoCombustionChamberCoeff, 3);
            }
        }

        public double NitrogenOxidIVGrossOut
        {
            get
            {
                return Math.Round(NitrogetOxidsGrossOut * 0.8, 6);
            }
        }

        public double NitrogenOxidIIGrossOut
        {
            get
            {
                return Math.Round(NitrogetOxidsGrossOut * 0.13, 6);
            }
        }

        public double CalculatedMaxConcentrationNO2
        {
            get
            {
                return 1000.0 * NitrogenOxidIIGrossOut / VolumeOfDryFlueGases;
            }
        }

        public string CalculatedMaxConcentrationNO2String
        { get => Math.Round(CalculatedMaxConcentrationNO2, 6).ToString(); }

        // 3.Calculation of benz(a)pyrene emissions

        public double ExcessAirInFireboxCoeff { get; set; } = 2.5; // const

        public double BoilerLroadOnBenzopyreneConcentrationCoeff
        {
            get
            {
                return Math.Round(7.46 * Math.Exp(-1.99 * 0.93), 3);
            }
        }

        public double BoilerLroadOnBenzopyreneConcentrationCoeff_GrossOut { get; set; } = 1.0; // const

        public double FlueGasRecirculationBenzapyreneCoeff { get; set; } = 1.0; // const

        public double SteppedAirInputIntoCombustionChamberBenzapyreneCoeff { get; set; } = 0.99; // const

        public double BenzapyreneConcentration
        {
            get
            {
                return 0.001 * ExcessAirInFireboxCoeff * (0.032 + 0.043 * 0.001 * HeatVoltageOfCombustionVolume) / 1.4 / Math.Exp(0.88 * (ExcessAirInFireboxCoeff - 1) +
                       ExcessAirInFireboxCoeff) * BoilerLroadOnBenzopyreneConcentrationCoeff * FlueGasRecirculationBenzapyreneCoeff * SteppedAirInputIntoCombustionChamberBenzapyreneCoeff;
            }
        }

        public string BenzapyreneConcentrationString
        { get => TableInstancesFactory.FormatNumber(BenzapyreneConcentration, 3); }

        public double HeatVoltageOfCombustionVolume
        {
            get
            {
                return 1000.0 * MinCombustionHeat * FuelConsumptionOnMaxPower / 1.2;
            }
        }

        public string HeatVoltageOfCombustionVolumeString
        { get => Math.Round(HeatVoltageOfCombustionVolume, 6).ToString(); }

        public double BenzapyreneGrossOut
        {
            get
            {
                return BenzapyreneConcentration * 0.000_001 * StrangeEmptyField / BoilerLroadOnBenzopyreneConcentrationCoeff;
            }
        }

        public string BenzapyreneGrossOutString
        { get => TableInstancesFactory.FormatNumber(BenzapyreneGrossOut, 3); }

        public double BenzapyreneMaxOut
        {
            get
            {
                return BenzapyreneConcentration * VolumeOfDryFlueGases * 0.001;
            }
        }

        public string BenzapyreneMaxOutString
        { get => TableInstancesFactory.FormatNumber(BenzapyreneMaxOut, 3); }


        // 4. Calculation of mercury emission

        public double MercurySpecificEmission { get; set; } = 0.0014; // const

        public double FuelConsumptionInBurningInstallation
        {
            get
            {
                return FuelConsumptionOnMaxPower * 3600 / 1000;
            }
        }

        public double MercuryGrossOut
        {
            get
            {
                return 0.000_001 * MercurySpecificEmission * FuelConsumptionPerYear;
            }
        }

        public string MercuryGrossOutString
        { get => TableInstancesFactory.FormatNumber(MercuryGrossOut, 3); }

        public double MercuryMaxOut
        {
            get
            {
                return FuelConsumptionInBurningInstallation * MercurySpecificEmission * 0.001 / 3.6;
            }
        }

        public override XElement ToXElement()
        {
            return new XElement("HotWaterBoiler",
                new XAttribute("Id", Id),
                new XElement("Name", Name),
                new XElement("Place", Place),
                new XElement("FuelType", FuelType),
                new XElement("MinCombustionHeat", MinCombustionHeat),
                new XElement("RatedLoad", RatedLoad),
                new XElement("EfficiencyGrossOfBoiler", EfficiencyGrossOfBoiler),
                new XElement("FuelConsumptionOnMaxPower", FuelConsumptionOnMaxPower),
                new XElement("TheoreticalVolumeOfDryFlueGases", TheoreticalVolumeOfDryFlueGases)
            );
        }

        public override XElement ToResultsXElement()
        {
            XElement el = ToXElement();
            el.Add(new XElement("WorkingTimePerYear", WorkingTimePerYear));
            el.Add(new XElement("FuelConsumptionPerYear", FuelConsumptionPerYear));

            return el;
        }

        public override int GetHashCode()
        {
            int nameHash = Encoding.UTF8.GetBytes(Name).Sum(b => (int)b);
            int placeHash = Encoding.UTF8.GetBytes(Place).Sum(b => (int)b);
            int fuelHash = Encoding.UTF8.GetBytes(FuelType).Sum(b => (int)b);
            return (int)(4000 * FuelConsumptionOnMaxPower + 3 * nameHash + 2 * fuelHash + 5 * placeHash + 17 * RatedLoad + 5 * EfficiencyGrossOfBoiler + 2 * MinCombustionHeat + TheoreticalVolumeOfDryFlueGases);
        }

        public static int GetHashCode(string place, string name, string fuelType, double combustion, double fuelConsumption, double gasVolume, double load, double kpd)
        {
            int nameHash = Encoding.UTF8.GetBytes(name).Sum(b => (int)b);
            int placeHash = Encoding.UTF8.GetBytes(place).Sum(b => (int)b);
            int fuelHash = Encoding.UTF8.GetBytes(fuelType).Sum(b => (int)b);
            return (int)(4000 * fuelConsumption + 3 * nameHash + 2 * fuelHash + 5 * placeHash + 17 * load + 5 * kpd + 2 * combustion + gasVolume);
        }

        public override bool Equals(object obj)
        {
            HotWaterBoiler boil = obj as HotWaterBoiler;
            if (boil == null) return false;

            if (GetHashCode() == boil.GetHashCode()) return true;
            else return false;
        }
    }
}
