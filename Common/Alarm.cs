using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AlarmGenerateService
{
    [DataContract]
    [Serializable]
    public class Alarm
    {
        private DateTime timeOfGenerete;
        private string nameOfClient;
        private string message;
        private TypeOfRisk typeOfRisk;
        private bool replicated;

        public Alarm() { }
        public Alarm(DateTime timeOfGenerete, string nameOfClient, string message, TypeOfRisk typeOfRisk)
        {
            this.TimeOfGenerete = timeOfGenerete;
            this.NameOfClient = nameOfClient;
            this.Message = message;
            this.TypeOfRisk = typeOfRisk;
            this.Replicated = false;
        }
        [DataMember]
        public DateTime TimeOfGenerete { get => timeOfGenerete; set => timeOfGenerete = value; }
        [DataMember]
        public string NameOfClient { get => nameOfClient; set => nameOfClient = value; }
        [DataMember]
        public string Message { get => message; set => message = value; }
        [DataMember]
        public TypeOfRisk TypeOfRisk { get => typeOfRisk; set => typeOfRisk = value; }
        [DataMember]
        public bool Replicated { get => replicated; set => replicated = value; }

        public TypeOfRisk CalculateRisk()
        {
            Random random = new Random();
            int num = random.Next(50);
            if (num < 10)
                return TypeOfRisk.Low;
            else if (num >= 10 && num < 40)
                return TypeOfRisk.Medium;
            else
                return TypeOfRisk.High;
        }
    }
}
