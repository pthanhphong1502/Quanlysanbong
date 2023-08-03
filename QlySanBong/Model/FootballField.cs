﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QlySanBong.Model
{
    class FootballField
    {
        //Properties

        private int idField;

        public int IdField { get => idField; set => idField = value; }

        private string name;

        public string Name { get => name; set => name = value; }

        private int type; //5 => sân 5, 7 => sân 7

        public int Type { get => type; set => type = value; }
        private int status;
        public int Status { get => status; set => status = value; }

        private string note;

        public string Note { get => note; set => note = value; }

        private int isDeleted;

        public int IsDeleted { get => isDeleted; set => isDeleted = value; }
        //Constructor

        public FootballField()
        {

        }

        public FootballField(int idField, string name, int type, int status, string note, int isDeleted)
        {
            this.idField = idField;
            this.name = name;
            this.type = type;
            this.status = status;
            this.note = note;
            this.isDeleted = isDeleted;
        }
    }
}
