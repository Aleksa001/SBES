using AlarmGenerateService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class DataBase
    {
        private List<Alarm> alarms = new List<Alarm>();

        public List<Alarm> Alarms { get => alarms; set => alarms = value; }


        public string AddToList(Alarm a)
        {
            if (!alarms.Contains(a))
            {
                alarms.Add(a);
                return "Successfully added!";
            }
            else
            {
                return "This alarm already exsist!";
            }

        }

    }
   
}
