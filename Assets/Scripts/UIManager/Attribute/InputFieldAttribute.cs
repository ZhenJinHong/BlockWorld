using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatFramework.UiMiao
{
    [AttributeUsage(AttributeTargets.Property)]
    public class InputFieldAttribute : Attribute
    {
    }
    [AttributeUsage(AttributeTargets.Property)]
    public class DropdownAttribute : Attribute
    {
        public string methodName;
        public DropdownAttribute()
        {
        }
        public DropdownAttribute(string getOptionsMothod)
        {
            this.methodName = getOptionsMothod;
        }
    }
}
