using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResourceStringChecker;

namespace ResourceCheckTool
{
    public class CheckResultWindowViewModel
    {
        public ObservableCollection<CheckResult> CheckResults { get; set; }

        public CheckResultWindowViewModel(List<CheckResult> results)
        {
            CheckResults = new ObservableCollection<CheckResult>(results);
        }
    }
}
