#region Namespaces
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
#endregion

namespace WTA_MECH {
    class AppMechRibbon : IExternalApplication {
        public SplitButton sb_MU;
        public SplitButton sb_MUO;
        public string docsPath = "N:\\CAD\\BDS PRM 2016\\WTA Common\\Revit Resources\\WTAAddins\\SourceCode\\Docs";
        public string docPDF01 = "WTA_MECH.pdf";
        /// Singleton external application class instance.
        internal static AppMechRibbon _app = null;
        /// Provide access to singleton class instance.
        public static AppMechRibbon Instance {
            get { return _app; }
        }
        public Result OnStartup(UIControlledApplication a) {
            _app = this;
            AddMech_WTA_MECH_Ribbon(a);
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication a) {
            return Result.Succeeded;
        }

        public void AddMech_WTA_MECH_Ribbon(UIControlledApplication a) {
            string ExecutingAssemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string ExecutingAssemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            // create ribbon tab 
            String thisNewTabName = "WTA-MECH";
            try {
                a.CreateRibbonTab(thisNewTabName);
            } catch (Autodesk.Revit.Exceptions.ArgumentException) {
                // Assume error generated is due to "WTA" already existing
            }

            //   Add ribbon panels.
            String thisPanelNamBe = "Be This";
            RibbonPanel thisRibbonPanelBe = a.CreateRibbonPanel(thisNewTabName, thisPanelNamBe);

            String thisPanelNameSensor = " Sensor ";
            RibbonPanel thisRibbonPanelSensor = a.CreateRibbonPanel(thisNewTabName, thisPanelNameSensor);
            //   Set the large image shown on button
            //Note that the full image name is namespace_prefix + "." + the actual imageName);
            //pushButton.LargeImage = NewBitmapImage(System.Reflection.Assembly.GetExecutingAssembly(), ExecutingAssemblyName + ".QVis.png");

            //   Create push button in this ribbon panel 
            // Buttons for Sensor/Unit No leader  - these will be used in a split button stack 
            PushButtonData pbMechStatUnit1 = new PushButtonData("SensForUnit1", "Sens for\nUnit ID/#", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdPlaceStatForMechUnitInstance1");
            PushButtonData pbMechStatUnit2 = new PushButtonData("SensForUnit2", "Sens for\nUnit #", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdPlaceStatForMechUnitInstance2");
            // The tool settings buttons
            PushButtonData pbMechStatUnitSet1 = new PushButtonData("SensForUnitSet1", "Setting 1", ExecutingAssemblyPath, ExecutingAssemblyName + ".MechStatUnitSettings1");
            PushButtonData pbMechStatUnitSet2 = new PushButtonData("SensForUnitSet2", "Setting 2", ExecutingAssemblyPath, ExecutingAssemblyName + ".MechStatUnitSettings2");

            // Buttons for Sensor/Unit With leader - these will be used in a split button stack 
            PushButtonData pbMechStatUnitOff1 = new PushButtonData("SensForUnitOff1", "Sens w/L\nfor Unit ID/#", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdPlaceStatOffsetForMechUnitInstance1");
            PushButtonData pbMechStatUnitOff2 = new PushButtonData("SensForUnitOff2", "Sens w/L\nfor Unit #", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdPlaceStatOffsetForMechUnitInstance2");
            // The tool settings buttons
            PushButtonData pbMechStatUnitOffSet1 = new PushButtonData("SensForUnitOffSet1", "Setting 1", ExecutingAssemblyPath, ExecutingAssemblyName + ".MechStatUnitOffSettings1");
            PushButtonData pbMechStatUnitOffSet2 = new PushButtonData("SensForUnitOffSet2", "Setting 2", ExecutingAssemblyPath, ExecutingAssemblyName + ".MechStatUnitOffSettings2");

            // Buttons for ID/# tag - these will be used in a split button stack
            PushButtonData pbMechStatTag1 = new PushButtonData("Sens IDT", "ID/# Tag", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdTwoPickMechSensorTagEuip1");
            PushButtonData pbMechStatTag2 = new PushButtonData("Sens IDO", "# Tag", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdTwoPickMechSensorTagEuip2");


            // Buttons for ID/# Offset tag - these will be used in a split button stack
            PushButtonData pbMechStatTagOff1 = new PushButtonData("Sens OffsetT", "ID/# Offset", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdTwoPickMechSensorTagOffEuip1");
            PushButtonData pbMechStatTagOff2 = new PushButtonData("Sens OffsetO", "# Offset", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdTwoPickMechSensorTagOffEuip2");


            PushButtonData pbBeMechHVAC = new PushButtonData("BeMechHVAC", "HVAC", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdBeMECH_HVACWorkSet");
            pbBeMechHVAC.ToolTip = "Switch to Mech HVAC Workset.";
            string lDescBeMechHVAC = "If you can't beat'm, join'm. Become MECH HVAC workset.";
            pbBeMechHVAC.LongDescription = lDescBeMechHVAC;

            // Split buttons
            SplitButtonData sbMSU = new SplitButtonData("sbMSU", "Sens\nfor Unit");
            SplitButtonData sbMSUO = new SplitButtonData("sbMSUO", "Sens w/L\nfor Unit");
            SplitButtonData sbSTagReg = new SplitButtonData("sbSensTag", "Split Sens ID");
            SplitButtonData sbSTagOff = new SplitButtonData("sbSensTagOff", "Split Sens Offset");

            PushButtonData pbMechSpinStat = new PushButtonData("Sens Spin", "Sens Spin", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdSpinStat");
            PushButtonData pbMechTogSym = new PushButtonData("Sens Symb", "Sens Symb", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdSymbStat");

            //   Set the large image shown on button
            //Note that the full image name is namespace_prefix + "." + the actual imageName);
            pbMechStatUnit1.LargeImage = NewBitmapImage(System.Reflection.Assembly.GetExecutingAssembly(), ExecutingAssemblyName + ".TStatUnit_1.png");
            pbMechStatUnitOff1.LargeImage = NewBitmapImage(System.Reflection.Assembly.GetExecutingAssembly(), ExecutingAssemblyName + ".TStatUnitO_1.png");
            pbMechStatUnit2.LargeImage = NewBitmapImage(System.Reflection.Assembly.GetExecutingAssembly(), ExecutingAssemblyName + ".TStatUnit_2.png");
            pbMechStatUnitOff2.LargeImage = NewBitmapImage(System.Reflection.Assembly.GetExecutingAssembly(), ExecutingAssemblyName + ".TStatUnitO_2.png");

            pbMechStatTag1.LargeImage = NewBitmapImage(System.Reflection.Assembly.GetExecutingAssembly(), ExecutingAssemblyName + ".SensTag_IDNO.png");
            pbMechStatTag2.LargeImage = NewBitmapImage(System.Reflection.Assembly.GetExecutingAssembly(), ExecutingAssemblyName + ".SensTag_NO.png");
            pbMechStatTagOff1.LargeImage = NewBitmapImage(System.Reflection.Assembly.GetExecutingAssembly(), ExecutingAssemblyName + ".SensOffTag_IDNO.png");
            pbMechStatTagOff2.LargeImage = NewBitmapImage(System.Reflection.Assembly.GetExecutingAssembly(), ExecutingAssemblyName + ".SensOffTag_NO.png");

            // add button tips (when data, must be defined prior to adding button.)
            pbMechStatUnit1.ToolTip = "Places a Sensor Stat for mechanical equip with equip. tag.";
            pbMechStatUnitOff1.ToolTip = "Places a Sensor Stat with leader for mechanical equip with equip. tag.";
            pbMechStatUnit2.ToolTip = "Places a Sensor Stat for mechanical equip with equip. tag.";
            pbMechStatUnitOff2.ToolTip = "Places a Sensor Stat with leader for mechanical equip with equip. tag.";
            pbMechStatTag1.ToolTip = "Places ID and number equipment tag that is for Sensors.";
            pbMechStatTagOff1.ToolTip = "Places offset Sensor symbol with ID and number equipment tag that is ID for Sensors.";
            pbMechStatTag2.ToolTip = "Places number only equipment tag that is ID for Sensors.";
            pbMechStatTagOff2.ToolTip = "Places offset Sensor symbol with number only equipment tag that is ID for Sensors.";

            pbMechSpinStat.ToolTip = "Toggles Horizontal/Vertical parameter value.";
            pbMechTogSym.ToolTip = "Toggles Sensor symbol visibility.";

            string lDescMechStatUnit =
                "First pick is the Sensor location."
                + "\nSecond pick selects the mechanical equipment the Sensor is client to."
                + "\nThird pick selects the mechanical equipment Sensor ID text location at the Sensor. This is a Tag on the mechanical equipment.";
            string lDescMechStatUnitOff =
                "Places a Sensor for mechanical equipment in four picks."
                + "\nFirst pick is the Sensor location. Ignore the symbol. It will be turned off later."
                + "\nSecond pick is for the offset Sensor tag symbol location."
                + "\nThird pick selects the mechanical equipment the Sensor is client to."
                + "\nLast pick selects the mechanical equipment Sensor ID text location at the Sensor. This is a Tag on the mechanical equipment.";
            string lDescMechStatTag =
                "First pick is the mechanical equipment the Sensor is client to."
                + "\nLast pick selects the mechanical equipment Sensor ID text location at the Sensor. This is a Tag on the mechanical equipment.";
            string lDescMechStatTagOff = "First pick is the Sensor to change."
                + "\nSecond pick is for the Sensor offset symbol location."
                + "\nIf needed, third pick selects the mechanical equipment the Sensor is client to. Otherwise press Esc to exit."
                + "\nLast pick selects the mechanical equipment Sensor ID text location at the Sensor. This is a Tag on the mechanical equipment.";

            pbMechStatUnit1.LongDescription = lDescMechStatUnit;
            pbMechStatUnitOff1.LongDescription = lDescMechStatUnitOff;
            pbMechStatUnit2.LongDescription = lDescMechStatUnit;
            pbMechStatUnitOff2.LongDescription = lDescMechStatUnitOff;
            pbMechStatTag1.LongDescription = lDescMechStatTag;
            pbMechStatTagOff1.LongDescription = lDescMechStatTagOff;
            pbMechStatTag2.LongDescription = lDescMechStatTag;
            pbMechStatTagOff2.LongDescription = lDescMechStatTagOff;


            //String thisPanelGenScripName = "GenScrip";
            //RibbonPanel thisPanelGenScrip = a.CreateRibbonPanel(thisNewTabName, thisPanelGenScripName);

            //ComboBoxData CB1Data = new ComboBoxData("11111");
            //ComboBoxData CB2Data = new ComboBoxData("22222");
            //IList<RibbonItem> cnt1Stacked = thisPanelGenScrip.AddStackedItems(CB1Data, CB2Data);

            //Autodesk.Revit.UI.ComboBox combo1 = cnt1Stacked[0] as Autodesk.Revit.UI.ComboBox;

            //Autodesk.Revit.UI.ComboBox combo2 = cnt1Stacked[1] as Autodesk.Revit.UI.ComboBox;

            //ComboBoxMemberData SDD11 = new ComboBoxMemberData("CB1_01", "Item for CB1_01 item(SDD11)");
            //ComboBoxMemberData SDD12 = new ComboBoxMemberData("CB1_02", "Item for CB1_02 item(SDD1)");

            //ComboBoxMember cb1Mem11 = combo1.AddItem(SDD11);
            //ComboBoxMember cb1Mem12 = combo1.AddItem(SDD12);

            //ComboBoxMemberData SDD21 = new ComboBoxMemberData("CB2_01", "Item for CB2_01 item(SDD21)");
            //ComboBoxMemberData SDD22 = new ComboBoxMemberData("CB2_02", "Item for CB2_02 item(SDD22)");

            //ComboBoxMember cb1Mem21 = combo2.AddItem(SDD21);
            //ComboBoxMember cb1Mem22 = combo2.AddItem(SDD22);

            //combo1.CurrentChanged += 
            //    new EventHandler<Autodesk.Revit.UI.Events.ComboBoxCurrentChangedEventArgs>(ProcessComboChange);

            //combo2.CurrentChanged +=
            //    new EventHandler<Autodesk.Revit.UI.Events.ComboBoxCurrentChangedEventArgs>(ProcessComboChange);


            // add to ribbon panelA
            // List<RibbonItem> projectButtonsBe = new List<RibbonItem>();
            // projectButtonsBe.AddRange(thisRibbonPanelBe.AddStackedItems(pbBeMechHVAC));
            thisRibbonPanelBe.AddItem(pbBeMechHVAC);

            // assemble split buttons and add to ribbon
            sb_MU = thisRibbonPanelSensor.AddItem(sbMSU) as SplitButton;
            sb_MU.AddPushButton(pbMechStatUnit1);
            sb_MU.AddPushButton(pbMechStatUnit2);
            sb_MU.AddPushButton(pbMechStatUnitSet1);
            sb_MU.AddPushButton(pbMechStatUnitSet2);

            sb_MUO = thisRibbonPanelSensor.AddItem(sbMSUO) as SplitButton;
            sb_MUO.AddPushButton(pbMechStatUnitOff1);
            sb_MUO.AddPushButton(pbMechStatUnitOff2);
            sb_MUO.AddPushButton(pbMechStatUnitOffSet1);
            sb_MUO.AddPushButton(pbMechStatUnitOffSet2);

            thisRibbonPanelSensor.AddSeparator();

            // assemble split buttons and add to ribbon
            SplitButton sbT = thisRibbonPanelSensor.AddItem(sbSTagReg) as SplitButton;
            sbT.AddPushButton(pbMechStatTag1);
            sbT.AddPushButton(pbMechStatTag2);

            SplitButton sbOffT = thisRibbonPanelSensor.AddItem(sbSTagOff) as SplitButton;
            sbOffT.AddPushButton(pbMechStatTagOff1);
            sbOffT.AddPushButton(pbMechStatTagOff2);

            thisRibbonPanelSensor.AddSeparator();
            List<RibbonItem> ribbonItems = new List<RibbonItem>();
            ribbonItems.AddRange(thisRibbonPanelSensor.AddStackedItems(pbMechSpinStat, pbMechTogSym));


            thisRibbonPanelBe.AddSlideOut();
            PushButtonData bInfo = new PushButtonData("Info", "Info", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdOpenDocFolder");
            bInfo.ToolTip = "See the help document regarding this.";
            bInfo.LargeImage = NewBitmapImage(System.Reflection.Assembly.GetExecutingAssembly(), ExecutingAssemblyName + ".InfoLg.png");
            thisRibbonPanelBe.AddItem(bInfo);

            thisRibbonPanelSensor.AddSlideOut();
            thisRibbonPanelSensor.AddItem(bInfo);

        } // AddMech_WTA_MECH_Ribbon

        //void ProcessComboChange(object sender, Autodesk.Revit.UI.Events.ComboBoxCurrentChangedEventArgs args) {
        //    ComboBox cbox = sender as ComboBox;
        //    ComboBoxMember cbxMembr = cbox.Current as ComboBoxMember;
        //    System.Windows.MessageBox.Show(cbxMembr.ItemText);
        //}

        /// <summary>
        /// Load a new icon bitmap from embedded resources.
        /// For the BitmapImage, make sure you reference WindowsBase and Presentation Core
        /// and PresentationCore, and import the System.Windows.Media.Imaging namespace. 
        /// </summary>
        BitmapImage NewBitmapImage(System.Reflection.Assembly a, string imageName) {
            Stream s = a.GetManifestResourceStream(imageName);
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.StreamSource = s;
            img.EndInit();
            return img;
        }
        public void SetSplitButtonToThisOrTop(string _bName, SplitButton _splitButton) {
            IList<PushButton> sbList = _splitButton.GetItems();
            foreach (PushButton pb in sbList) {
                if (pb.Name.Equals(_bName)) {
                    _splitButton.CurrentButton = pb;
                    return;
                }
            }
            _splitButton.CurrentButton = sbList[0];
        }

        public void SetSplitButtonToTop() {
            IList<PushButton> sbList = sb_MU.GetItems();
            sb_MU.CurrentButton = sbList[0];
        }
    }
}



//IList<RibbonItem> stackedItems = thisNewRibbonPanel.AddStackedItems(pbMechStatTag, pbMechStatTagOff, cbData);
//if (stackedItems.Count > 1) {
//    ComboBox cBox = stackedItems[2] as ComboBox;
//    if (cBox != null) {
//        //cBox.ItemText = "ComboBox";

//        cBox.ToolTip = "Options";
//        cBox.LongDescription = "Sets with or without\nthe equipment ID.";

//        ComboBoxMemberData cboxMemDataA = new ComboBoxMemberData("WithID", "Two Liner");
//        cBox.AddItem(cboxMemDataA);

//        ComboBoxMemberData cboxMemDataB = new ComboBoxMemberData("NoID", "One Liner");
//        cBox.AddItem(cboxMemDataB);

//    }
//}

//PushButtonData pbMechStatTag = new PushButtonData("Sens ID", "Sens ID", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdTwoPickMechSensorTagEuip");
//PushButtonData pbMechStatTagOff = new PushButtonData("Sens Offset", "Sens Offset", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdTwoPickMechSensorTagOffEuip");

//thisNewRibbonPanel.AddItem(pbMechStatUnit1);
//thisNewRibbonPanel.AddItem(pbMechStatUnitOff1);
