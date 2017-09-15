using Domain.Concrete.Common;

namespace Domain.Concrete
{
    public class Crushers
    {
        public string ProductType { get; set; }
        public string ProductionTarget { get; set; }
        public string ProductionTargetUnit { get; set; }
        public string MetallurgicalTestWork { get; set; }
        public string Mobility { get; set; }
        public string PrimaryCrusherType { get; set; }
        public string PrimaryCrusherTypeOther { get; set; }
        public string SecondaryCrusherType { get; set; }
        public string SecondaryCrusherTypeOther { get; set; }
        public string TertiaryCrusherType { get; set; }
        public string TertiaryCrusherTypeOther { get; set; }
        public string CircuitType { get; set; }
        public string Feed80PercentPassingExpected { get; set; }
        public string Product80PercentPassingExpected { get; set; }
        public string KnownSizeOfPrimaryCrusherRequired { get; set; }
        public string KnownSizeOfSecondaryCrusherRequired { get; set; }
        public string KnownSizeOfTertiaryCrusherRequired { get; set; }
        public string ProductTarget { get; set; }
        public string ProductTargetUnit { get; set; }

        public bool MotorIncluded { get; set; }

        public Motor Motor { get; set; }
    }
}
