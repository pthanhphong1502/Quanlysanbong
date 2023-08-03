using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QlySanBong.Model
{
    class Attendance
    {
        private int dayInMonth;
        public int DayInMonth { get => dayInMonth; set => dayInMonth = value; }
        private int idEmployee;
        public int IdEmployee { get => idEmployee; set => idEmployee = value; }
        private int month;
        public int Month { get => month; set => month = value; }
        public Attendance()
        {

        }
        public Attendance(int dayInMonth, int month, int idEmployee)
        {
            this.dayInMonth = dayInMonth;
            this.idEmployee = idEmployee;
            this.month = month;
        }
    }
}
