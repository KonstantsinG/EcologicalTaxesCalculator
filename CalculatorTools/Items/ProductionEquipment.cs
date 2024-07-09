using CalculatorTools.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CalculatorTools.Items
{
    public class ProductionEquipment : ISerializableItem
    {
        public virtual int Id { get; set; }

        public string TypeName { get; set; } // extra param

        public string Place { get; set; } // extra

        public string Name { get; set; } // extra

        public virtual string FuelType { get; set; } = null; // const (null)

        public double MinCombustionHeat { get; set; } = 0.0d; // const

        public double WorkingTimePerYear { get; set; } = 0.0; // --------------------------> enter

        public double CurrentWorkingTime { get; set; } = 0.0; // -----------------> enter(Minor)

        public double FuelConsumptionOnMaxPower { get; set; } = 0.0; // const

        public double FuelConsumptionPerYear { get; set; } = 0.0; // ----------------------> enter

        public double CurrentFuelConsumption { get; set; } = 0.0; // -----------------> enter(Minor)

        public double FuelConsumptionOnMeanPower // calck (if WorkingTimePerYear > 0) round(FuelConsumptionPerYear * 1000 / WorkingTimePerYear / 3600, 4) else 0.)
        {
            get
            {
                if (WorkingTimePerYear > 0)
                    return Math.Round(FuelConsumptionPerYear * 1000 / WorkingTimePerYear / 3600, 4);
                else
                    return 0.0;
            }
        }

        public double TheoreticalVolumeOfDryFlueGases { get; set; } = 0.0; // const

        public double VolumeOfDryFlueGases // calck (FuelConsumptionOnMaxPower * TheoreticalVolumeOfDryFlueGases)
        {
            get
            {
                return Math.Round(FuelConsumptionOnMaxPower * TheoreticalVolumeOfDryFlueGases, 2);
            }
        }

        public double StrangeEmptyField
        {
            get
            {
                return Math.Round(FuelConsumptionPerYear * TheoreticalVolumeOfDryFlueGases, 1);
            }
        }

        public ProductionEquipment()
        {
        }

        public ProductionEquipment(string place, string name, string fuelType, double minCombustionHeat, double fuelConsumptionOnMaxPower, 
                                   double theoreticalVolumeOfDryFlueGases)
        {
            Place = place;
            Name = name;

            FuelType = fuelType;
            MinCombustionHeat = minCombustionHeat;
            FuelConsumptionOnMaxPower = fuelConsumptionOnMaxPower;
            TheoreticalVolumeOfDryFlueGases = theoreticalVolumeOfDryFlueGases;
        }

        public void SetParams(double time, double fuel)
        {
            WorkingTimePerYear = time;
            FuelConsumptionPerYear = fuel;
        }

        public virtual XElement ToXElement() => null;

        public virtual XElement ToResultsXElement() => null;
    }
}
