using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.IO;

namespace WTA_MECH {
    class ToolSettingsClass {
        Dictionary<string, string> dicSettings;
        string _toolSettingsName;
        bool _resetThis;
        bool debug = false;
        //bool debug =  true;

        public ToolSettingsClass(string toolSettingsName, bool resetThis = false) {
            _toolSettingsName = toolSettingsName;
            _resetThis = resetThis;
        }

        public Dictionary<string, string> GetSettingsFor(string _toolName) {
            dicSettings = new Dictionary<string, string>();
            StringCollection scSettings = new StringCollection();
            scSettings = Properties.Settings.Default.SensorToolSettings;
            if (scSettings != null) {
                try {
                    dicSettings = scSettings.ToDictionary();
                    //if (debug) { dicSettings.DebugReveal(); }
                } catch (Exception) {
                    System.Windows.MessageBox.Show("Starting from scratch. \n\nCould not convert scSettings to a dictionary.", "Failed To Use Saved Settings");
                }
            } else {
                System.Windows.MessageBox.Show("Starting from scratch. \n\nscSettings is null.", "Failed To See Saved Settings");
            }

            //
            // Check for each tool setting and fill it if it is missing or reset it
            // if a reset is desired.
            //
            int settingMode = 0;
            if (_resetThis) { settingMode = 2; }

            if (_toolName == "ST1") {
                if (debug) { System.Windows.MessageBox.Show("Ensuring dictionary. settingMode is " + settingMode.ToString(), _toolName); }

                EnsureUpdateResetSettingsDictionary(dicSettings, _toolName + "_Workset", "MECH HVAC", settingMode, "MECH HVAC");
                EnsureUpdateResetSettingsDictionary(dicSettings, _toolName + "_SensorFamily", "M_BAS SENSOR", settingMode, "M_BAS SENSOR");
                EnsureUpdateResetSettingsDictionary(dicSettings, _toolName + "_SensorFamilySymbol", "THERMOSTAT", settingMode, "THERMOSTAT");
                EnsureUpdateResetSettingsDictionary(dicSettings, _toolName + "_IDParameter", "STAT ZONE NUMBER", settingMode, "STAT ZONE NUMBER");
                EnsureUpdateResetSettingsDictionary(dicSettings, _toolName + "_SensorTagFamily", "M_EQIP_BAS_SENSOR_TAG", settingMode, "M_EQIP_BAS_SENSOR_TAG");
                EnsureUpdateResetSettingsDictionary(dicSettings, _toolName + "_SensorTagFamilySymbol", "SENSOR", settingMode, "SENSOR");
            }

            if (_toolName == "ST2") {
                if (debug) { System.Windows.MessageBox.Show("Ensuring dictionary. settingMode is " + settingMode.ToString(), _toolName); }

                EnsureUpdateResetSettingsDictionary(dicSettings, _toolName + "_Workset", "MECH HVAC", settingMode, "MECH HVAC");
                EnsureUpdateResetSettingsDictionary(dicSettings, _toolName + "_SensorFamily", "M_BAS SENSOR", settingMode, "M_BAS SENSOR");
                EnsureUpdateResetSettingsDictionary(dicSettings, _toolName + "_SensorFamilySymbol", "THERMOSTAT", settingMode, "THERMOSTAT");
                EnsureUpdateResetSettingsDictionary(dicSettings, _toolName + "_IDParameter", "STAT ZONE NUMBER", settingMode, "STAT ZONE NUMBER");
                EnsureUpdateResetSettingsDictionary(dicSettings, _toolName + "_SensorTagFamily", "M_EQIP_BAS_SENSOR_TAG", settingMode, "M_EQIP_BAS_SENSOR_TAG");
                EnsureUpdateResetSettingsDictionary(dicSettings, _toolName + "_SensorTagFamilySymbol", "TAG NUMBER ONLY", settingMode, "TAG NUMBER ONLY");
            }

            if (_toolName == "STO1") {
                if (debug) { System.Windows.MessageBox.Show("Ensuring dictionary. settingMode is " + settingMode.ToString(), _toolName); }

                EnsureUpdateResetSettingsDictionary(dicSettings, _toolName + "_Workset", "MECH HVAC", settingMode, "MECH HVAC");
                EnsureUpdateResetSettingsDictionary(dicSettings, _toolName + "_SensorFamily", "M_BAS SENSOR", settingMode, "M_BAS SENSOR");
                EnsureUpdateResetSettingsDictionary(dicSettings, _toolName + "_SensorFamilySymbol", "THERMOSTAT", settingMode, "THERMOSTAT");
                EnsureUpdateResetSettingsDictionary(dicSettings, _toolName + "_SensorTagFamily", "M_EQIP_BAS_SENSOR_TAG", settingMode, "M_EQIP_BAS_SENSOR_TAG");
                EnsureUpdateResetSettingsDictionary(dicSettings, _toolName + "_SensorTagFamilySymbol", "SENSOR", settingMode, "SENSOR");
                EnsureUpdateResetSettingsDictionary(dicSettings, _toolName + "_SensorTagFamily2", "M_DEVICE_BAS_TAG_SYM", settingMode, "M_DEVICE_BAS_TAG_SYM");
                EnsureUpdateResetSettingsDictionary(dicSettings, _toolName + "_SensorTagFamilySymbol2", "M-DATA-SENSOR", settingMode, "M-DATA-SENSOR");

            }

            if (_toolName == "STO2") {
                if (debug) { System.Windows.MessageBox.Show("Ensuring dictionary. settingMode is " + settingMode.ToString(), _toolName); }

                EnsureUpdateResetSettingsDictionary(dicSettings, _toolName + "_Workset", "MECH HVAC", settingMode, "MECH HVAC");
                EnsureUpdateResetSettingsDictionary(dicSettings, _toolName + "_SensorFamily", "M_BAS SENSOR", settingMode, "M_BAS SENSOR");
                EnsureUpdateResetSettingsDictionary(dicSettings, _toolName + "_SensorFamilySymbol", "THERMOSTAT", settingMode, "THERMOSTAT");
                EnsureUpdateResetSettingsDictionary(dicSettings, _toolName + "_SensorTagFamily", "M_EQIP_BAS_SENSOR_TAG", settingMode, "M_EQIP_BAS_SENSOR_TAG");
                EnsureUpdateResetSettingsDictionary(dicSettings, _toolName + "_SensorTagFamilySymbol", "TAG NUMBER ONLY", settingMode, "TAG NUMBER ONLY");
                EnsureUpdateResetSettingsDictionary(dicSettings, _toolName + "_SensorTagFamily2", "M_DEVICE_BAS_TAG_SYM", settingMode, "M_DEVICE_BAS_TAG_SYM");
                EnsureUpdateResetSettingsDictionary(dicSettings, _toolName + "_SensorTagFamilySymbol2", "M-DATA-SENSOR", settingMode, "M-DATA-SENSOR");

            }

            //if (debug) { dicSettings.DebugReveal(); }
            return dicSettings;

        }

        // This is probably not used anymore.
        void AddOrUpdateSettingsDictionary(Dictionary<string, string> dic, string key, string val) {
            if (dic.ContainsKey(key)) { dic[key] = val; } else { dic.Add(key, val); }
        }

        // 0 = check for, add default if missing, 1 = change to , 2 = reset to default
        void EnsureUpdateResetSettingsDictionary(Dictionary<string, string> _dic, string _key, string _val, int _mode, string _defVal) {
            switch (_mode) {
                case 0: // check for, add default if missing
                    if (_dic.ContainsKey(_key)) {
                        if (debug) { System.Windows.MessageBox.Show("Is present " + _key + " => " + _dic[_key], "EnsureUpdateResetSettingsDictionary"); }
                        break;
                    } else {
                        _dic.Add(_key, _defVal);
                        if (debug) { System.Windows.MessageBox.Show("Missing, so adding " + _key + " => " + _defVal, "EnsureUpdateResetSettingsDictionary"); }
                        break;
                    }
                case 1: // change to, add to if missing
                    if (_dic.ContainsKey(_key)) {
                        _dic[_key] = _val;
                        if (debug) { System.Windows.MessageBox.Show("Is present and changing " + _key + " => " + _defVal, "EnsureUpdateResetSettingsDictionary"); }
                        break;
                    } else {
                        _dic.Add(_key, _val);
                        if (debug) { System.Windows.MessageBox.Show("Was to change but missing, so adding " + _key + " => " + _defVal, "EnsureUpdateResetSettingsDictionary"); }
                        break;
                    }
                case 2: // reset to default
                    if (_dic.ContainsKey(_key)) {
                        _dic[_key] = _defVal;
                        if (debug) { System.Windows.MessageBox.Show("Is present, setting default " + _key + " => " + _defVal, "EnsureUpdateResetSettingsDictionary"); }
                        break;
                    } else {
                        _dic.Add(_key, _defVal);
                        if (debug) { System.Windows.MessageBox.Show("Missing, adding and setting default " + _key + " => " + _defVal, "EnsureUpdateResetSettingsDictionary"); }
                        break;
                    }
                default:
                    if (debug) { System.Windows.MessageBox.Show("Switch on mode fell through to default! Mode: " + _mode.ToString(), "EnsureUpdateResetSettingsDictionary"); }
                    if (_dic.ContainsKey(_key)) {
                        _dic[_key] = _defVal;
                        break;
                    } else {
                        _dic.Add(_key, _defVal);
                        break;
                    }
            }
        }
    }

    public static class Extender {
        public static Dictionary<string, string> ToDictionary(this StringCollection sc) {
            if (sc.Count % 2 != 0) throw new InvalidDataException("Broken dictionary");
            //string s = "";
            var dic = new Dictionary<string, string>();
            for (var i = 0; i < sc.Count; i += 2) {
                dic.Add(sc[i], sc[i + 1]);
                //s += "\n" + sc[i] + " : " + sc[i + 1];
            }
            //System.Windows.MessageBox.Show(s, "StringCollection.ToDictionary");
            return dic;
        }

        public static StringCollection ToStringCollection(this Dictionary<string, string> dic) {
            var sc = new StringCollection();
            //string s = "";
            foreach (var d in dic) {
                sc.Add(d.Key);
                sc.Add(d.Value);
                //s += "\n" + d.Key + " : " + d.Value;
            }
            //System.Windows.MessageBox.Show(s, "Dictionary.ToStringCollection");
            return sc;
        }

        public static void DebugReveal(this Dictionary<string, string> dic) {
            string s = "";
            foreach (KeyValuePair<string, string> entry in dic) {
                s += "\n" + entry.Key + " : " + entry.Value;
            }
            System.Windows.MessageBox.Show(s, "DebugReveal Dictionary");
        }
    }

}
