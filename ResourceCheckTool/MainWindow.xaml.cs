using ResourceStringChecker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ResourceCheckTool
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var showMessage = new Action<string>(x => MessageBox.Show(x, "リソースチェックツール"));
            var showResult = new Action<List<CheckResult>>(
                x =>
                {
                    var win = new CheckResultWindow(x);
                    win.ShowDialog();
                });
            InitializeComponent();
            this.DataContext = new MainWindowViewModel(showResult, showMessage);
        }
    }
}
