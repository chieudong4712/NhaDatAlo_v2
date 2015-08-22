using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceMetadata.Core.Common
{
    public class FilterCondition
    {
        private IList<FilterConditionItem> _conditions = new List<FilterConditionItem>();
        public IList<FilterConditionItem> Conditions
        {
            get
            {
                return _conditions;
            }
            set
            {
                _conditions = value;
            }
        }

        private bool _isNeedAllConditions = false;
        public bool IsNeedAllConditions
        {
            get
            {
                return _isNeedAllConditions;
            }
            set
            {
                _isNeedAllConditions = value;
            }
        }
    }

    public class FilterConditionItem
    {
        public string PropertyName { get; set; }
        public string PropertyType { get; set; }
        public string ComparisonOperator { get; set; }
        public string Value { get; set; }
    }
}
