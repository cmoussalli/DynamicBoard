using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.DynamicBoard.UI
{

    public class ChartParam
    {
        public string Name { get; set; }
        public List<string> Values { get; set; }
    }



    public class ChartData
    {

        public List<ChartParam> ChartParams = new();

        public ChartData()
        {
            ChartParams = new List<ChartParam>();
        }

        // public ChartParam chartParam = new();

        public void Clear()
        {
            ChartParams = new();
        }

        public void Add(string name, string singleValue)
        {
            List<string> values = new List<string>();
            values.Add(singleValue);
            ChartParams.Add(new ChartParam { Name = name, Values = values });
        }
        public void Add(string name, List<string> Values)
        {
            ChartParams.Add(new ChartParam { Name = name, Values = Values });
        }
        public List<ChartParam> AddValue(string name, string value)
        {
            ChartParam chartParamToUpdate = ChartParams.FirstOrDefault(c => c.Name == name);
            if (chartParamToUpdate != null)
            {
                chartParamToUpdate.Values.Add(value);
            }
            return ChartParams;
        }
        public List<ChartParam> Remove(string name)
        {

            ChartParam paramToRemove = ChartParams.FirstOrDefault(p => p.Name == name);
            if (paramToRemove != null)
            {
                ChartParams.Remove(paramToRemove);
            }
            return ChartParams;
        }
        public List<ChartParam> RemoveValue(string name, string value)
        {
            ChartParam chartParamToRemove = ChartParams.FirstOrDefault(c => c.Name == name && c.Values.Contains(value));
            if (chartParamToRemove != null)
            {
                chartParamToRemove.Values.Remove(value);
            }
            return ChartParams;
        }
        public List<ChartParam> ReplaceValue(string name, string oldValue, string newValue)
        {
            ChartParam chartParamToUpdate = ChartParams.FirstOrDefault(c => c.Name == name && c.Values.Contains(oldValue));
            if (chartParamToUpdate != null)
            {
                int index = chartParamToUpdate.Values.IndexOf(oldValue);
                if (index != -1)
                {
                    chartParamToUpdate.Values[index] = newValue;
                }
            }
            return ChartParams;
        }



    }
}
