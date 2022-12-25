using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlarmGenerateService
{
    public class Alarm
    {
        private DateTime timeOfGenerete;
        private string nameOfClient;
        private string message;
        private TypeOfRisk typeOfRisk;

        public Alarm() { }
        public Alarm(DateTime timeOfGenerete, string nameOfClient, string message, TypeOfRisk typeOfRisk)
        {
            this.TimeOfGenerete = timeOfGenerete;
            this.NameOfClient = nameOfClient;
            this.Message = message;
            this.TypeOfRisk = typeOfRisk;
        }

        public DateTime TimeOfGenerete { get => timeOfGenerete; set => timeOfGenerete = value; }
        public string NameOfClient { get => nameOfClient; set => nameOfClient = value; }
        public string Message { get => message; set => message = value; }
        public TypeOfRisk TypeOfRisk { get => typeOfRisk; set => typeOfRisk = value; }
    }
}
