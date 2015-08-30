using System;
using System.Collections.Generic;

namespace Common
{
    public class GroupInfoList<T> : ObservableRangeCollection<Object>
    {
        public object Key { get; set; }

        public new IEnumerator<object> GetEnumerator()
        {
            return (System.Collections.Generic.IEnumerator<object>) base.GetEnumerator();
        } 
    }
}
