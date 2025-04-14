namespace SME.DynamicBoard.UI
{

    public class DashboardParam
    {
        public string Name { get; set; }
        public List<string> Values { get; set; }
    }



    public class DashboardData
    {

        public List<DashboardParam> DashboardParams = new();

        public DashboardData()
        {
            DashboardParams = new List<DashboardParam>();
        }

        // public dashboardParam dashboardParam = new();

        public void Clear()
        {
            DashboardParams = new();
        }

        public void Add(string name, string singleValue)
        {
            List<string> values = new List<string>();
            values.Add(singleValue);
            DashboardParams.Add(new DashboardParam { Name = name, Values = values });
        }
        public void Add(string name, List<string> Values)
        {
            DashboardParams.Add(new DashboardParam { Name = name, Values = Values });
        }
        public List<DashboardParam> AddValue(string name, string value)
        {
            DashboardParam dashboardParamToUpdate = DashboardParams.FirstOrDefault(c => c.Name == name);
            if (dashboardParamToUpdate != null)
            {
                dashboardParamToUpdate.Values.Add(value);
            }
            return DashboardParams;
        }
        public List<DashboardParam> Remove(string name)
        {

            DashboardParam paramToRemove = DashboardParams.FirstOrDefault(p => p.Name == name);
            if (paramToRemove != null)
            {
                DashboardParams.Remove(paramToRemove);
            }
            return DashboardParams;
        }
        public List<DashboardParam> RemoveValue(string name, string value)
        {
            DashboardParam dashboardParamToRemove = DashboardParams.FirstOrDefault(c => c.Name == name && c.Values.Contains(value));
            if (dashboardParamToRemove != null)
            {
                dashboardParamToRemove.Values.Remove(value);
            }
            return DashboardParams;
        }
        public List<DashboardParam> ReplaceValue(string name, string oldValue, string newValue)
        {
            DashboardParam dashboardParamToUpdate = DashboardParams.FirstOrDefault(c => c.Name == name && c.Values.Contains(oldValue));
            if (dashboardParamToUpdate != null)
            {
                int index = dashboardParamToUpdate.Values.IndexOf(oldValue);
                if (index != -1)
                {
                    dashboardParamToUpdate.Values[index] = newValue;
                }
            }
            return DashboardParams;
        }



    }
}
