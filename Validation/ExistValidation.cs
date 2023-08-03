using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using QlySanBong.data_provier;
using System.Globalization;

namespace QlySanBong.Validation
{
    class ExistValidation : ValidationRule
    {
        public string ErrorMessage { get; set; }
        public string ErrorMessageNull { get; set; }
        public string ElementName { get; set; }


        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult result = new ValidationResult(true, null);
            if (value == null)
            {
                return new ValidationResult(true, null);
            }
            if (value.ToString() == "")
            {
                return new ValidationResult(false, this.ErrorMessageNull);
            }
            switch (ElementName)
            {
                case "UserName":
                    if (AccountDP.Instance.IsExistUserName(value.ToString()))
                    {
                        result = new ValidationResult(false, this.ErrorMessage);
                    }
                    break;
                case "FieldName":
                    if (FootballFieldDP.Instance.isExistFieldName(value.ToString()))
                    {
                        result = new ValidationResult(false, this.ErrorMessage);
                    }
                    break;
                case "GoodsName":
                    if (GoodsDP.Instance.IsExistGoodsName(value.ToString()))
                    {
                        result = new ValidationResult(false, this.ErrorMessage);
                    }
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}
