using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFScheduler
{
    public class Loader
    {
        private static readonly Loader instance = new Loader();
        private int maxDuration;
        private int currentOutsideIndex;
        public static Loader Instance
        {
            get { return instance; }
        }
        protected Loader() { }
        private List<ISchedulerItemData> originalSource;

        public ObservableCollection<ISchedulerItemData> FilterData(ObservableCollection<ISchedulerItemData> sortableSource, DateTime oldDate, DateTime newDate, int availbleRange)
        {
            var isPositiveDirection = oldDate.Date > newDate.Date;
            ObservableCollection<ISchedulerItemData> selectedData = null;
            if (originalSource != null && originalSource.Count != 0)
            {
                var tempList = new List<ISchedulerItemData>();
                var listToRemove = new List<ISchedulerItemData>();
                bool isChanged = false;
                foreach (var item in sortableSource)
                    tempList.Add(item);

                int i = sortableSource.Count != 0 ? originalSource.IndexOf(sortableSource[sortableSource.Count - 1]) : currentOutsideIndex;

                if (isPositiveDirection)
                {
                    while (i <= originalSource.Count - 1 && originalSource[i].Date.Date < newDate.AddDays(availbleRange).Date)
                    {
                        if (originalSource[i].Date.AddDays(originalSource[i].Duration - 1).Date >= newDate.Date && !tempList.Contains(originalSource[i]))
                        {
                            tempList.Add(originalSource[i]);
                            isChanged = true;
                        }
                        i++;
                    }
                    i = 0;
                    while (i < tempList.Count)
                    {
                        if (tempList[i].Date.AddDays(tempList[i].Duration).Date < newDate.Date)
                        {
                            listToRemove.Add(tempList[i]);
                            isChanged = true;
                        }
                        i++;
                    }
                }
                else
                {
                    while (i >= 0 && originalSource[i].Date.Date >= newDate.AddDays(-maxDuration).Date)
                    {
                        if (originalSource[i].Date.AddDays(originalSource[i].Duration).Date >= newDate.Date &&
                            originalSource[i].Date.Date < newDate.AddDays(availbleRange).Date)
                        {
                            if (!tempList.Contains(originalSource[i]))
                            {
                                tempList.Add(originalSource[i]);
                                isChanged = true;
                            }
                        }
                        i--;

                    }

                    i = tempList.Count - 1;
                    while (i >= 0)
                    {
                        if (tempList[i].Date.Date > newDate.AddDays(availbleRange - 1).Date)
                        {
                            listToRemove.Add(tempList[i]);
                            isChanged = true;
                        }
                        i--;
                    }
                }

                tempList.Sort();
                if (isChanged)
                {
                    foreach (var item in listToRemove)
                    {
                        tempList.Remove(item);
                    }
                    if (listToRemove.Count != 0)
                        currentOutsideIndex = originalSource.IndexOf(listToRemove[0]);
                    selectedData = new ObservableCollection<ISchedulerItemData>();
                    foreach (var item in tempList)
                    {
                        selectedData.Add(item);
                    }
                }
                else
                    selectedData = sortableSource;


            }
            return selectedData;
        }

        public ObservableCollection<ISchedulerItemData> FirstLoadData(List<ISchedulerItemData> source, DateTime filterDate, int availbleRange)
        {
            originalSource = source;
            ObservableCollection<ISchedulerItemData> selectedData = null;
            if (source != null && source.Count != 0)
            {
                bool isFounded = false;
                currentOutsideIndex = 0;
                selectedData = new ObservableCollection<ISchedulerItemData>();
                maxDuration = source[source.Count - 1].Duration;

                foreach (var item in source)
                {
                    if (!isFounded && item.Date.Date > filterDate.AddDays(availbleRange - 1).Date)
                    {
                        currentOutsideIndex = source.IndexOf(item);
                        isFounded = true;
                    }

                    if (item.Date.Date < filterDate.AddDays(availbleRange).Date
                        && item.Date.AddDays(item.Duration).Date >= filterDate.Date)
                    {
                        selectedData.Add(item);
                    }

                    if (item.Duration > maxDuration)
                        maxDuration = item.Duration;
                }
            }
            return selectedData;
        }

    }
}
