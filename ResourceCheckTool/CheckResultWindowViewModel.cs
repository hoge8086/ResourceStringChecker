using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using ResourceStringChecker;

namespace ResourceCheckTool
{
    public class CheckResultWindowViewModel : BindableBase
    {
        public bool rejectedFilter;

        public bool RejectedFilter
        {
            get { return rejectedFilter; }
            set
            {
                SetProperty(ref rejectedFilter, value);
                var view = System.Windows.Data.CollectionViewSource.GetDefaultView(CheckResults);

                if(value)
                    view.Filter = new Predicate<object>(x => !((CheckResult)x).Result);
                else
                    view.Filter = null;
            }
        }
        public ObservableCollection<CheckResult> CheckResults { get; set; }

        public CheckResultWindowViewModel(List<CheckResult> results)
        {
            CheckResults = new ObservableCollection<CheckResult>(results);
        }
    }
}
