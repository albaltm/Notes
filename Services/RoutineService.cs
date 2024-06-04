using AppNotes.Models;
using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppNotes.Services
{
    public class RoutineService(ConexionBBDD _conn)
    {
        public bool ShowToday(Routine routine, DateTime date, string user)
        {
            if (routine.Start > date)
            {
                return false;
            }
            if (!routine.Active)
            {
                var registry = _conn.GetRoutinesRegistry(user).Where(x => x.Routine.Equals(routine.Id) && x.Start == date).FirstOrDefault();
                return registry != null;
            }
            if (routine.Frequency == FrequencyType.Everyday)
            {
                return true;
            }
            if (routine.Frequency == FrequencyType.DaysOfWeek)
            {
                var days = routine.DaysOfWeek.Split(";");
                var dayofweek = (int) date.DayOfWeek;
                if (dayofweek == 0) dayofweek = 7;
                return days[dayofweek - 1].Equals("True");
            }
            if (routine.Frequency == FrequencyType.FlexibleWeek)
            {
                var monday = GetFirstDayOfWeek(date);
                var sunday = monday.AddDays(6);

                var registry = _conn.GetRoutinesRegistry(user).Where(x => x.Routine.Equals(routine.Id) && x.Start == date).FirstOrDefault();
                if (registry != null) { return true; }

                var achieved = _conn.GetRoutinesRegistry(user).Where(x => x.Routine.Equals(routine.Id) && x.Start >= monday && x.Start <= sunday && x.Status == Status.Done).Count();
                return achieved < routine.Period;
            }
            if (routine.Frequency == FrequencyType.Personalized)
            {
                var days = (date - routine.Start).TotalDays;
                return (int) days % routine.Period == 0;
            }
            if (routine.Frequency == FrequencyType.Periods)
            {
                bool[] period = new bool[routine.Period + routine.Rest];
                for (int i = 0; i < routine.Period; i++)
                {
                    period[i] = true;
                }
                for (int i = 0; i < routine.Rest; i++)
                {
                    period[i + routine.Period] = false;
                }
                var days = (date - routine.Start).TotalDays;
                var dayofperiod = (int) (days % (routine.Period + routine.Rest));
                return period[dayofperiod];
            }
            return false;
        }

        private DateTime GetFirstDayOfWeek(DateTime day)
        {
            var diff = (int)day.DayOfWeek - 1;

            if (diff < 0)
                diff += 7;

            return day.AddDays(-diff).Date;
        }
    }
}
