using Domain.Concrete.Common;

namespace Domain.Concrete
{
    public class Mill
    {
        public string Mobility { get; set; }
        public string WorkIndex { get; set; }
        public string PrimaryMillType { get; set; }
        public string PrimaryMillTypeOther { get; set; }
        public string SecondaryMillType { get; set; }
        public string SecondaryMillTypeOther { get; set; }
        public string TertiaryMillType { get; set; }
        public string TertiaryMillTypeOther { get; set; }
        public string ProcessType { get; set; }
        public string OverflowPassingTarget { get; set; }
        public string CircuitType { get; set; }
        public string Feed80PercentPassingExpected { get; set; }
        public string Product80PercentPassingExpected { get; set; }
        public string LinerType { get; set; }
        public string LinerTypeOther { get; set; }
        public string LinerAverageThicknessExpected { get; set; }
        public string ProductTarget { get; set; }
        public string MillPercentOfTheCriticalSpeed { get; set; }
        public string MediaCharge { get; set; }
        public string PercentWaterInPulpExpected { get; set; }
        public string TypeOfOutletExpected { get; set; }
        public string TypeOfOutletExpectedOther { get; set; }
        public string MillTrommel { get; set; }
        public string KnownSizeOfAgglomeratorDiameter { get; set; }
        public string KnownSizeOfAgglomeratorDiameterUnit { get; set; }
        public string KnownSizeOfAgglomeratorLength { get; set; }
        public string KnownSizeOfAgglomeratorLengthUnit { get; set; }
        public string PercentCirculatingLoad { get; set; }

        public bool MotorIncluded { get; set; }

        public Motor Motor { get; set; }
        public Product Product { get; set; }
    }
}
