using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sanooqna.Models
{
    public class ValueValidation:ValidationAttribute
    {
        double? minVal;
        double? maxVal;
        public ValueValidation(double val,bool isMax)
        {
            if (isMax)
            {
                this.maxVal = val;
            }
            else
            {
                this.minVal = val;
            }
        }
        public ValueValidation(double minVal,double maxVal)
        {
            this.minVal = minVal;
            this.maxVal = maxVal;
        }
        public override bool IsValid(object value)
        {
            if(value is double)
            {
                double val = (double)value;
                if(maxVal == null)
                {
                    return val >= minVal;
                }else if(minVal == null)
                {
                    return val <= maxVal;
                }
                else
                {
                    return val >= minVal && val <= maxVal;
                }
            }
            return false;
        }
    }
}