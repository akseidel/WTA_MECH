using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
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
using System.Windows.Threading;

namespace WTA_MECH {
    /// <summary>
    /// Interaction logic for FormToolSetting.xaml
    /// </summary>
    public partial class FormToolSetting : Window {
        string _purpose;
        string toolCodeName;
        public Dictionary<string, string> settingsDictForThisTool;
        DispatcherTimer timeOut = new DispatcherTimer();
        //bool debug = true;

        public FormToolSetting(string SettingsForThisTool) {
            InitializeComponent();
            toolCodeName = SettingsForThisTool;
            Top = Properties.Settings.Default.SetMgr_Top;
            Left = Properties.Settings.Default.SetMgr_left;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e) {
          //  ResizeMode = System.Windows.ResizeMode.CanResizeWithGrip;
            InitTheFields();
            timeOut.Tick += new EventHandler(timeOut_Tick);
        }
        public void SetMsg(string _msg, string purpose, string _bot = "") {
            MsgTextBlockMainMsg.Text = _msg;
            _purpose = purpose;
            MsgLabelTop.Text = purpose;
            if (_bot != "") {
                MsgLabelBot.Text = _bot;
            }
        }
        private void InitTheFields(bool resetThisSetting = false) {
            ToolSettingsClass thisTool = null;
            thisTool = new ToolSettingsClass(toolCodeName, resetThisSetting);
            settingsDictForThisTool = thisTool.GetSettingsFor(toolCodeName);
            // wpf datagrid will only allow edits to ieditableobjects , a dictionary is not one
            // so a List is being used as an intermeadiate device
            SettingsGrid.ItemsSource = LoadSettingsData(settingsDictForThisTool, toolCodeName);
            SettingsGrid.Columns[0].IsReadOnly = true;
            RootSearchPath.Text = Properties.Settings.Default.RootSearchPath;
            chkTagOtherViews.IsChecked = Properties.Settings.Default.TagOtherViews;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            Properties.Settings.Default.SetMgr_Top =  Top ;
            Properties.Settings.Default.SetMgr_left = Left ;
            StringCollection scThisSensor = UpdatedSettingsDictionary().ToStringCollection();
            Properties.Settings.Default.RootSearchPath = RootSearchPath.Text;
            Properties.Settings.Default.SensorToolSettings = scThisSensor;
            Properties.Settings.Default.TagOtherViews = (bool)chkTagOtherViews.IsChecked;
            Properties.Settings.Default.Save();
        }

        private List<SettingsItem> LoadSettingsData(Dictionary<string, string> _settingsDictForThisTool, string _toolCodeName) {
            List<SettingsItem> ToolSettings = new List<SettingsItem>();
            foreach (KeyValuePair<string, string> entry in _settingsDictForThisTool) {
                // make this list only for the _toolCodeName entries
                if (entry.Key.StartsWith(_toolCodeName)) {
                    ToolSettings.Add(new SettingsItem() {
                        Description = entry.Key.ToString(),
                        SettingValue = entry.Value.ToString(),
                    });
                }
            }
            return ToolSettings;
        }

        public class SettingsItem {
            public string Description { get; set; }
            public string SettingValue { get; set; }
        }

        // A dictionary bound to a WPF datagrid will not be automatically updated when
        // the user edits the datagrid. So we have to convert back and forth between the
        // bound list collection and the dictionary. 
        private Dictionary<string, string> UpdatedSettingsDictionary() {
            // first get all saved settings
            StringCollection scAllSensorSettings = new StringCollection();
            scAllSensorSettings = Properties.Settings.Default.SensorToolSettings;
            // put into a dictionary
            Dictionary<string, string> dictToReturnAsAllSensorToolSettings = new Dictionary<string, string>();
            dictToReturnAsAllSensorToolSettings = scAllSensorSettings.ToDictionary();
            // now edit dictionary according to the current settingsgrid.itemssource
            foreach (SettingsItem setItem in SettingsGrid.ItemsSource) {
                // update only the _SettingsForThisTool entries
                AddOrUpdateSettingsDictionary(dictToReturnAsAllSensorToolSettings, setItem.Description, setItem.SettingValue);
            }
            return dictToReturnAsAllSensorToolSettings;
        }

        private void ResetToDefaults_Click(object sender, RoutedEventArgs e) {
            InitTheFields(true);
        }

        void AddOrUpdateSettingsDictionary(Dictionary<string, string> dic, string key, string val) {
            if (dic.ContainsKey(key)) { dic[key] = val; } else { dic.Add(key, val); }
        }

        private void Window_LocationChanged(object sender, EventArgs e) {
            timeOut.Stop();
            timeOut.Interval = new TimeSpan(0, 0, 2);
            timeOut.Start();
        }
        private void timeOut_Tick(object sender, EventArgs e) {
            timeOut.Stop();
            this.MsgLabelTop.Text = _purpose;
        }
        public void DragWindow(object sender, MouseButtonEventArgs args) {
            timeOut.Stop();
            // Watch out. Fatal error if not primary button!
            if (args.LeftButton == MouseButtonState.Pressed) { DragMove(); }
        }

        private void MsgLabelTop_MouseEnter(object sender, MouseEventArgs e) {
            this.MsgLabelTop.Text = "Position As You Like.";
            timeOut.Stop();
            timeOut.Interval = new TimeSpan(0, 0, 1);
            timeOut.Start();
        }

        private void MsgLabelBot_MouseEnter(object sender, MouseEventArgs e) {
            MsgLabelBot.FontWeight = FontWeights.Bold;
        }

        private void MsgLabelBot_MouseLeave(object sender, MouseEventArgs e) {
            MsgLabelBot.FontWeight = FontWeights.Normal;
        }

        private void MsgLabelBot_MouseDown(object sender, MouseButtonEventArgs e) {
            Close();
        }

    }

}
