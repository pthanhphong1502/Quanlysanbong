﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QlySanBong.ResourceXAML
{
    public  static class CurrentAccount /// Tài khoản hiện đang đăng nhập
    {
        private static int type;
        private static string displayName;
        private static byte[] image;
        private static int idAccount;
        private static string password;
        private static int idEmployee;
        public static int Type { get => type; set => type = value; }
        public static string DisplayName { get => displayName; set => displayName = value; }
        public static byte[] Image { get => image; set => image = value; }
        public static int IdAccount { get => idAccount; set => idAccount = value; }
        public static string Password { get => password; set => password = value; }
        public static int IdEmployee { get => idEmployee; set => idEmployee = value; }
    }
}
