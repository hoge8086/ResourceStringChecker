using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;
using ResourceStringChecker;

namespace ResourceCheckTool
{
    public class CheckTargetViewModel : BindableBase
    {
        private string specName;
        private string sheetName;
        public string SpecName
        {
            get { return specName; }
            set
            {
                SetProperty(ref specName, value);
                SheetNames = new ObservableCollection<string>(MainWindowViewModel.SpecDocments.FirstOrDefault(x => x.SpecName == SpecName).SheetNames);
                SheetName = SheetNames[0];
            }
        }
        public string SheetName
        {
            get { return sheetName; }
            set { SetProperty(ref sheetName, value); }
        }

        public string CheckRows{ get; set; }
        public ObservableCollection<string> SheetNames { get; set; }

        public CheckTargetViewModel()
        {
            specName = MainWindowViewModel.SpecDocments[0].SpecName;
            sheetName = MainWindowViewModel.SpecDocments[0].SheetNames[0];
            SheetNames = new ObservableCollection<string>(MainWindowViewModel.SpecDocments.FirstOrDefault(x => x.SpecName == specName).SheetNames);
            CheckRows = "";
        }

    }
    public class MainWindowViewModel : BindableBase
    {
        public ObservableCollection<CheckTargetViewModel> CheckTargets{ get; private set; }
        static public List<SpecDoc> SpecDocments { get; private set; }
        public List<string> SpecList
        {
            get
            {
                return SpecDocments.Select(x => x.SpecName).ToList();
            }
        }
        public DelegateCommand Check { get; }
        public MainWindowViewModel(Action<List<CheckResult>> showResult, Action<string> showMessage)
        {
            try
            {
                var repo = new SpecDocMetaInfoRepository();
                var rcReaderFactory = new ResourceFileReaderFactory();
                var checker = new ResourceChecker(rcReaderFactory, repo);
                SpecDocments = repo.GetSpecList();
                CheckTargets = new ObservableCollection<CheckTargetViewModel>();
                CheckTargets.Add(new CheckTargetViewModel());

                Check = new DelegateCommand(
                    () =>
                    {
                        try
                        {
                            var targets = CheckTargets.Select(
                                x => new CheckTarget()
                                {
                                    SpecName = x.SpecName,
                                    SheetName = x.SheetName,
                                    CheckRows = x.CheckRows.Split(new char[] { ',' }).Select(n => int.Parse(n.Trim())).ToList(),
                                }
                            ).ToList();
                            var result = checker.Check(targets);
                            showResult(result);
                        }catch(Exception ex)
                        {
                            showMessage("エラー:\n" + ex.Message);
                        }
                    }
                );
            }
            catch(Exception ex)
            {
                showMessage("エラー:\n" + ex.Message);
            }
        }
    }
}
