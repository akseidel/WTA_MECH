#region Header
//
// based on examples from BuildingCoder Jeremy Tammik,
// AKS 6/27/2016
//
#endregion // Header

#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using ComponentManager = Autodesk.Windows.ComponentManager;
using IWin32Window = System.Windows.Forms.IWin32Window;
using Keys = System.Windows.Forms.Keys;
using Autodesk.Revit.UI.Selection;
using System.Text;
using System.Runtime.InteropServices;



#endregion // Namespaces

namespace WTA_MECH {
   
    [Transaction(TransactionMode.Manual)]
    class CmdTwoPickTag : IExternalCommand {
        public Result Execute(ExternalCommandData commandData,
                              ref string message,
                              ElementSet elements) {

            PlunkOClass plunkThis = new PlunkOClass(commandData.Application);
            string wsName = "MECH HVAC";
            string FamilyTagName = "T-COMM TAG - INSTANCE";
            string FamilyTagSymbName = "T-COMM INSTANCE";
            bool hasLeader = false;
            bool oneShot = false;
            BuiltInCategory bicItemBeingTagged = BuiltInCategory.OST_CommunicationDevices;
            BuiltInCategory bicTagBeing = BuiltInCategory.OST_CommunicationDeviceTags;
            Element elemTagged = null;

            plunkThis.TwoPickTag(wsName, FamilyTagName, FamilyTagSymbName, bicItemBeingTagged, bicTagBeing, hasLeader, oneShot, ref elemTagged);

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    class CmdPickAirDevicesOnly : IExternalCommand {
        public Result Execute(ExternalCommandData commandData,
                             ref string message,
                             ElementSet elements) {

            UIApplication uiapp = commandData.Application;
            UIDocument _uidoc = uiapp.ActiveUIDocument;
            Autodesk.Revit.ApplicationServices.Application _app = uiapp.Application;
            Autodesk.Revit.DB.Document _doc = _uidoc.Document;

            PlunkOClass plunkThis = new PlunkOClass(commandData.Application);
            BuiltInCategory _bicItemDesired = BuiltInCategory.OST_DuctTerminal;

            List<ElementId> _selIds;
            plunkThis.PickTheseBicsOnly(_bicItemDesired, out _selIds);
            _uidoc.Selection.SetElementIds(_selIds);

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    class CmdCycleAirDeviceTypes : IExternalCommand {
        public Result Execute(ExternalCommandData commandData,
                             ref string message,
                             ElementSet elements) {

            UIApplication uiapp = commandData.Application;
            UIDocument _uidoc = uiapp.ActiveUIDocument;
            Autodesk.Revit.ApplicationServices.Application _app = uiapp.Application;
            Autodesk.Revit.DB.Document _doc = _uidoc.Document;

            BuiltInCategory bicFamilyA = BuiltInCategory.OST_DuctTerminal;
            BuiltInCategory bicFamilyB = BuiltInCategory.OST_DataDevices;
            BuiltInCategory bicFamilyC = BuiltInCategory.OST_MechanicalEquipment;
            //BuiltInCategory bicFamilyC = BuiltInCategory.OST_Sprinklers;

            ICollection<BuiltInCategory> categories = new[] { bicFamilyA, bicFamilyB, bicFamilyC };
            ElementFilter myPCatFilter = new ElementMulticategoryFilter(categories);
            ISelectionFilter myPickFilter = SelFilter.GetElementFilter(myPCatFilter);

            bool keepOnTruckn = true;
            FormMsgWPF formMsg = new FormMsgWPF();
            formMsg.Show();

            using (TransactionGroup pickGrp = new TransactionGroup(_doc)) {
                pickGrp.Start("CmdCycleType");
                bool firstTime = true;

                //string strCats= "";
                //foreach (BuiltInCategory iCat in categories) {
                //    strCats = strCats + iCat.ToString().Replace("OST_", "") + ", "; 
                //}
                string strCats = FamilyUtils.BICListMsg(categories);

                formMsg.SetMsg("Pick the " + strCats + " to check its type.", "Type Cycle:");
                while (keepOnTruckn) {
                    try {
                        Reference pickedElemRef = _uidoc.Selection.PickObject(ObjectType.Element, myPickFilter, "Pick the " + bicFamilyA.ToString() + " to cycle its types. (Press ESC to cancel)");
                        Element pickedElem = _doc.GetElement(pickedElemRef.ElementId);

                        FamilyInstance fi = pickedElem as FamilyInstance;
                        FamilySymbol fs = fi.Symbol;

                        var famTypesIds = fs.Family.GetFamilySymbolIds().OrderBy(e => _doc.GetElement(e).Name).ToList();
                        int thisIndx = famTypesIds.FindIndex(e => e == fs.Id);
                        int nextIndx = thisIndx;
                        if (!firstTime) {
                            nextIndx = nextIndx + 1;
                            if (nextIndx >= famTypesIds.Count) {
                                nextIndx = 0;
                            }
                        } else {
                            firstTime = false;
                        }

                        if (pickedElem != null) {
                            using (Transaction tp = new Transaction(_doc, "PlunkOMatic:SetParam")) {
                                tp.Start();
                                if (pickedElem != null) {
                                    fi.Symbol = _doc.GetElement(famTypesIds[nextIndx]) as FamilySymbol;
                                    formMsg.SetMsg("Currently:\n" + fi.Symbol.Name + "\n\nPick again to cycle its types.", "Type Cycling");
                                }
                                tp.Commit();
                            }
                        } else {
                            keepOnTruckn = false;
                        }
                    } catch (Exception) {
                        keepOnTruckn = false;
                        //throw;
                    }
                }
                pickGrp.Assimilate();
            }

            formMsg.Close();
            return Result.Succeeded;
        }
    }


    [Transaction(TransactionMode.Manual)]
    class CmdTest : IExternalCommand {
        public Result Execute(ExternalCommandData commandData,
                             ref string message,
                             ElementSet elements) {
            FormMsgWPF formMsgWPF = new FormMsgWPF();
            formMsgWPF.Show();
            formMsgWPF.SetMsg("What", "What", "What");
            //System.Windows.MessageBox.Show(formMsgWPF.IsVisible.ToString());
            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    class CmdSpinStat : IExternalCommand {
        public Result Execute(ExternalCommandData commandData,
                             ref string message,
                             ElementSet elements) {

            UIApplication uiapp = commandData.Application;
            UIDocument _uidoc = uiapp.ActiveUIDocument;
            Autodesk.Revit.ApplicationServices.Application _app = uiapp.Application;
            Autodesk.Revit.DB.Document _doc = _uidoc.Document;

            BuiltInCategory bicFamily = BuiltInCategory.OST_DataDevices;
            BuiltInCategory bicFamilyTag = BuiltInCategory.OST_DataDeviceTags;
            ICollection<BuiltInCategory> categories = new[] { bicFamily, bicFamilyTag };
            ElementFilter myPCatFilter = new ElementMulticategoryFilter(categories);
            ISelectionFilter myPickFilter = SelFilter.GetElementFilter(myPCatFilter);

            bool keepOnTruckn = true;
            FormMsgWPF formMsg = new FormMsgWPF();
            formMsg.Show();

            while (keepOnTruckn) {
                try {
                    formMsg.SetMsg("Pick the Sensor to change symbol orientation.", "Symbol Spin");
                    Reference pickedElemRef = _uidoc.Selection.PickObject(ObjectType.Element, myPickFilter, "Pick the Sensor to change symbol orientation. (Press ESC to cancel)");
                    Element pickedElem = _doc.GetElement(pickedElemRef.ElementId);
                    // get tagged element instead if user picked the tag
                    if (pickedElem.GetType() == typeof(IndependentTag)) {
                        IndependentTag _tag = (IndependentTag)pickedElem;
                        pickedElem = _doc.GetElement(_tag.TaggedLocalElementId);
                    }
                    if (pickedElem != null) {
                        Transaction tp = new Transaction(_doc, "PlunkOMatic:SetParam");
                        tp.Start();
                        //TaskDialog.Show("Debug", pickedElem.ToString());
                        if (pickedElem != null) {
                            Parameter parForHoriz = pickedElem.LookupParameter("HORIZONTAL");
                            if (null != parForHoriz) {
                                if (parForHoriz.AsInteger() == 1) {
                                    parForHoriz.Set(0);
                                } else {
                                    parForHoriz.Set(1);
                                }
                            }
                        }
                        tp.Commit();
                    } else {
                        keepOnTruckn = false;
                    }
                } catch (Exception) {
                    keepOnTruckn = false;
                    //throw;
                }
            }
            formMsg.Close();
            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    class CmdSymbStat : IExternalCommand {

        public Result Execute(ExternalCommandData commandData,
                             ref string message,
                             ElementSet elements) {

            UIApplication uiapp = commandData.Application;
            UIDocument _uidoc = uiapp.ActiveUIDocument;
            Autodesk.Revit.ApplicationServices.Application _app = uiapp.Application;
            Autodesk.Revit.DB.Document _doc = _uidoc.Document;

            string parmTogN = "SHOW SYM";
            BuiltInCategory bicFamily = BuiltInCategory.OST_DataDevices;
            BuiltInCategory bicFamilyTag = BuiltInCategory.OST_DataDeviceTags;
            ICollection<BuiltInCategory> categories = new[] { bicFamily, bicFamilyTag };
            ElementFilter myPCatFilter = new ElementMulticategoryFilter(categories);
            ISelectionFilter myPickFilter = SelFilter.GetElementFilter(myPCatFilter);

            bool keepOnTruckn = true;
            FormMsgWPF formMsgWPF = new FormMsgWPF();
            formMsgWPF.Show();

            while (keepOnTruckn) {
                try {
                    formMsgWPF.SetMsg("Pick the Sensor to toggle symbol visibility.", "Sensor Symbol Toggle");
                    Reference pickedElemRef = _uidoc.Selection.PickObject(ObjectType.Element, myPickFilter, "Pick the Sensor to toggle symbol visibility. (Press ESC to cancel)");
                    Element pickedElem = _doc.GetElement(pickedElemRef.ElementId);
                    // get tagged element instead if user picked the tag
                    if (pickedElem.GetType() == typeof(IndependentTag)) {
                        IndependentTag _tag = (IndependentTag)pickedElem;
                        pickedElem = _doc.GetElement(_tag.TaggedLocalElementId);
                    }
                    if (pickedElem != null) {
                        Transaction tp = new Transaction(_doc, "PlunkOMatic:SetParam");
                        tp.Start();
                        //TaskDialog.Show("Debug", pickedElem.ToString());
                        if (pickedElem != null) {
                            Parameter parForTog = pickedElem.LookupParameter(parmTogN);
                            if (null != parForTog) {
                                if (parForTog.AsInteger() == 1) {
                                    parForTog.Set(0);
                                } else {
                                    parForTog.Set(1);
                                }
                            }
                        }
                        tp.Commit();
                    } else {
                        keepOnTruckn = false;
                    }
                } catch (Exception) {
                    keepOnTruckn = false;
                    //throw;
                }
            }
            formMsgWPF.Close();
            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    class CmdTwoPickMechSensorTagOffEuip1 : IExternalCommand {
        public Result Execute(ExternalCommandData commandData,
                              ref string message,
                              ElementSet elements) {

            Document _doc = commandData.Application.ActiveUIDocument.Document;

            PlunkOClass plunkThis = new PlunkOClass(commandData.Application);
            string wsName = "MECH HVAC";
            string FamilyTagName = "M_DEVICE_BAS_TAG_SYM";
            string FamilyTagNameSymb = "M-DATA-SENSOR";
            BuiltInCategory bicItemBeingTagged = BuiltInCategory.OST_DataDevices;
            BuiltInCategory bicTagBeing = BuiltInCategory.OST_DataDeviceTags;
            bool oneShot = true;
            bool hasLeader = true;
            Element elemTagged = null;
            string cmdPurpose = "Change To Offset Data";
            Result result;

            try {
                // first pass
                result = plunkThis.TwoPickTag(wsName, FamilyTagName, FamilyTagNameSymb,
                    bicItemBeingTagged, bicTagBeing, hasLeader, oneShot, ref elemTagged, cmdPurpose);
                using (Transaction tp = new Transaction(_doc, "PlunkOMatic:SetParam")) {
                    tp.Start();
                    // try to uncheck the show sym yes/no is 1/0
                    Parameter parForVis = elemTagged.LookupParameter("SHOW SYM");
                    if (null != parForVis) {
                        parForVis.Set(0);
                    }
                    tp.Commit();
                }
                // second pass
                if (elemTagged != null) {
                    FamilyTagName = "M_EQIP_BAS_SENSOR_TAG";
                    FamilyTagNameSymb = "SENSOR";
                    bicTagBeing = BuiltInCategory.OST_MechanicalEquipmentTags;
                    bicItemBeingTagged = BuiltInCategory.OST_MechanicalEquipment;
                    hasLeader = false;
                    elemTagged = null;
                    cmdPurpose = "Sensor Data";
                    result = plunkThis.TwoPickTag(wsName, FamilyTagName, FamilyTagNameSymb,
                        bicItemBeingTagged, bicTagBeing, hasLeader, oneShot, ref elemTagged, cmdPurpose);
                }
            } catch (Exception) {
                //throw;
            }
            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    class CmdTwoPickMechSensorTagOffEuip2 : IExternalCommand {
        public Result Execute(ExternalCommandData commandData,
                              ref string message,
                              ElementSet elements) {

            Document _doc = commandData.Application.ActiveUIDocument.Document;

            PlunkOClass plunkThis = new PlunkOClass(commandData.Application);
            string wsName = "MECH HVAC";
            string FamilyTagName = "M_DEVICE_BAS_TAG_SYM";
            string FamilyTagNameSymb = "M-DATA-SENSOR";
            BuiltInCategory bicItemBeingTagged = BuiltInCategory.OST_DataDevices;
            BuiltInCategory bicTagBeing = BuiltInCategory.OST_DataDeviceTags;
            bool oneShot = true;
            bool hasLeader = true;
            Element elemTagged = null;
            string cmdPurpose = "Change To Offset Data";
            Result result;

            try {
                // first pass
                result = plunkThis.TwoPickTag(wsName, FamilyTagName, FamilyTagNameSymb,
                    bicItemBeingTagged, bicTagBeing, hasLeader, oneShot, ref elemTagged, cmdPurpose);
                using (Transaction tp = new Transaction(_doc, "PlunkOMatic:SetParam")) {
                    tp.Start();
                    // try to uncheck the show sym yes/no is 1/0
                    Parameter parForVis = elemTagged.LookupParameter("SHOW SYM");
                    if (null != parForVis) {
                        parForVis.Set(0);
                    }
                    tp.Commit();
                }
                // second pass
                if (elemTagged != null) {
                    FamilyTagName = "M_EQIP_BAS_SENSOR_TAG";
                    FamilyTagNameSymb = "TAG NUMBER ONLY";
                    bicTagBeing = BuiltInCategory.OST_MechanicalEquipmentTags;
                    bicItemBeingTagged = BuiltInCategory.OST_MechanicalEquipment;
                    hasLeader = false;
                    elemTagged = null;
                    cmdPurpose = "Sensor Data";
                    result = plunkThis.TwoPickTag(wsName, FamilyTagName, FamilyTagNameSymb,
                        bicItemBeingTagged, bicTagBeing, hasLeader, oneShot, ref elemTagged, cmdPurpose);
                }
            } catch (Exception) {
                //throw;
            }
            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    class CmdTwoPickMechSensorTagEuip1 : IExternalCommand {
        public Result Execute(ExternalCommandData commandData,
                              ref string message,
                              ElementSet elements) {

            PlunkOClass plunkThis = new PlunkOClass(commandData.Application);
            string wsName = "MECH HVAC";
            string FamilyTagName = "M_EQIP_BAS_SENSOR_TAG";
            string FamilyTagNameSymb = "SENSOR";
            bool hasLeader = false;
            bool oneShot = false;
            BuiltInCategory bicTagBeing = BuiltInCategory.OST_MechanicalEquipmentTags;
            BuiltInCategory bicItemBeingTagged = BuiltInCategory.OST_MechanicalEquipment;
            Element elemTagged = null;
            string cmdPurpose = "Sensor Data";

            Result res = plunkThis.TwoPickTag(wsName, FamilyTagName, FamilyTagNameSymb,
                bicItemBeingTagged, bicTagBeing, hasLeader, oneShot, ref elemTagged, cmdPurpose);

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    class CmdTwoPickMechSensorTagEuip2 : IExternalCommand {
        public Result Execute(ExternalCommandData commandData,
                              ref string message,
                              ElementSet elements) {

            PlunkOClass plunkThis = new PlunkOClass(commandData.Application);
            string wsName = "MECH HVAC";
            string FamilyTagName = "M_EQIP_BAS_SENSOR_TAG";
            string FamilyTagNameSymb = "TAG NUMBER ONLY";
            bool hasLeader = false;
            bool oneShot = false;
            BuiltInCategory bicTagBeing = BuiltInCategory.OST_MechanicalEquipmentTags;
            BuiltInCategory bicItemBeingTagged = BuiltInCategory.OST_MechanicalEquipment;
            Element elemTagged = null;
            string cmdPurpose = "Sensor Data";

            Result res = plunkThis.TwoPickTag(wsName, FamilyTagName, FamilyTagNameSymb,
                bicItemBeingTagged, bicTagBeing, hasLeader, oneShot, ref elemTagged, cmdPurpose);

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    class CmdPlaceStatForMechUnitInstance1 : IExternalCommand {
        public Result Execute(ExternalCommandData commandData,
                             ref string message,
                             ElementSet elements) {

            UIApplication uiapp = commandData.Application;
            UIDocument _uidoc = uiapp.ActiveUIDocument;
            Autodesk.Revit.ApplicationServices.Application _app = uiapp.Application;
            Autodesk.Revit.DB.Document _doc = _uidoc.Document;

            PlunkOClass plunkThis = new PlunkOClass(commandData.Application);
            string wsName = "MECH HVAC";
            string FamilyName = "M_BAS SENSOR";
            string FamilySymbolName = "THERMOSTAT";
            string pName = "STAT ZONE NUMBER";
            string FamilyTagName = "M_EQIP_BAS_SENSOR_TAG";
            string FamilyTagNameSymb = "SENSOR";
            bool oneShot = true;
            bool hasLeader = false;
            BuiltInCategory bicTagBeing = BuiltInCategory.OST_MechanicalEquipmentTags;
            BuiltInCategory bicFamily = BuiltInCategory.OST_DataDevices;
            BuiltInCategory _bicMechItem = BuiltInCategory.OST_MechanicalEquipment;
            Element elemPlunked;
            bool keepOnTruckn = true;
            while (keepOnTruckn) {
                try {
                    Result result = plunkThis.PlunkThisFamilyType(FamilyName, FamilySymbolName, wsName, bicFamily, out elemPlunked, oneShot);
                    FormMsgWPF formMsg = new FormMsgWPF();
                    if ((result == Result.Succeeded) & (elemPlunked != null)) {
                        formMsg.Show();
                        formMsg.SetMsg("Now Select the Mech Unit for this sensor.", "Sensor For MEQ");
                        Transaction tp = new Transaction(_doc, "PlunkOMatic:OrientGuts ");
                        tp.Start();
                        plunkThis.OrientTheInsides(elemPlunked);
                        tp.Commit();
                        ICollection<BuiltInCategory> categories = new[] { _bicMechItem };
                        ElementFilter myPCatFilter = new ElementMulticategoryFilter(categories);
                        ISelectionFilter myPickFilter = SelFilter.GetElementFilter(myPCatFilter);
                        try {
                            Reference pickedElemRef = _uidoc.Selection.PickObject(ObjectType.Element, myPickFilter, "Select the Mech Unit for this sensor.");
                            Element pickedElem = _doc.GetElement(pickedElemRef.ElementId);
                            formMsg.SetMsg("Now place the unit text at the sensor.", "Sensor For MEQ");
                            plunkThis.AddThisTag(pickedElem, FamilyTagName, FamilyTagNameSymb, pName, bicTagBeing, hasLeader);
                            formMsg.Close();
                        } catch (Exception) {
                            formMsg.Close();
                            keepOnTruckn = false;
                            //throw;
                        }
                    } else {
                        formMsg.Close();
                        keepOnTruckn = false;
                    }
                } catch (Autodesk.Revit.Exceptions.OperationCanceledException) {
                    keepOnTruckn = false;
                    //    TaskDialog.Show("Where", "here  " );
                }
            }
            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    class CmdPlaceStatForMechUnitInstance2 : IExternalCommand {
        public Result Execute(ExternalCommandData commandData,
                             ref string message,
                             ElementSet elements) {

            UIApplication uiapp = commandData.Application;
            UIDocument _uidoc = uiapp.ActiveUIDocument;
            Autodesk.Revit.ApplicationServices.Application _app = uiapp.Application;
            Autodesk.Revit.DB.Document _doc = _uidoc.Document;

            PlunkOClass plunkThis = new PlunkOClass(commandData.Application);
            string wsName = "MECH HVAC";
            string FamilyName = "M_BAS SENSOR";
            string FamilySymbolName = "THERMOSTAT";
            string pName = "STAT ZONE NUMBER";
            string FamilyTagName = "M_EQIP_BAS_SENSOR_TAG";
            string FamilyTagNameSymb = "TAG NUMBER ONLY";
            bool oneShot = true;
            bool hasLeader = false;
            BuiltInCategory bicTagBeing = BuiltInCategory.OST_MechanicalEquipmentTags;
            BuiltInCategory bicFamily = BuiltInCategory.OST_DataDevices;
            BuiltInCategory _bicMechItem = BuiltInCategory.OST_MechanicalEquipment;
            Element elemPlunked;
            bool keepOnTruckn = true;
            while (keepOnTruckn) {
                try {
                    Result result = plunkThis.PlunkThisFamilyType(FamilyName, FamilySymbolName, wsName, bicFamily, out elemPlunked, oneShot);
                    FormMsgWPF formMsg = new FormMsgWPF();
                    if ((result == Result.Succeeded) & (elemPlunked != null)) {
                        formMsg.Show();
                        formMsg.SetMsg("Now Select the Mech Unit for this sensor.", "Sensor For MEQ");
                        Transaction tp = new Transaction(_doc, "PlunkOMatic:OrientGuts ");
                        tp.Start();
                        plunkThis.OrientTheInsides(elemPlunked);
                        tp.Commit();
                        ICollection<BuiltInCategory> categories = new[] { _bicMechItem };
                        ElementFilter myPCatFilter = new ElementMulticategoryFilter(categories);
                        ISelectionFilter myPickFilter = SelFilter.GetElementFilter(myPCatFilter);
                        try {
                            Reference pickedElemRef = _uidoc.Selection.PickObject(ObjectType.Element, myPickFilter, "Select the Mech Unit for this sensor.");
                            Element pickedElem = _doc.GetElement(pickedElemRef.ElementId);
                            formMsg.SetMsg("Now place the unit text at the sensor.", "Sensor For MEQ");
                            plunkThis.AddThisTag(pickedElem, FamilyTagName, FamilyTagNameSymb, pName, bicTagBeing, hasLeader);
                            formMsg.Close();
                        } catch (Exception) {
                            formMsg.Close();
                            keepOnTruckn = false;
                            //throw;
                        }
                    } else {
                        formMsg.Close();
                        keepOnTruckn = false;
                    }
                } catch (Autodesk.Revit.Exceptions.OperationCanceledException) {
                    keepOnTruckn = false;
                    //    TaskDialog.Show("Where", "here  " );
                }
            }
            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    class CmdPlaceStatOffsetForMechUnitInstance1 : IExternalCommand {
        public Result Execute(ExternalCommandData commandData,
                             ref string message,
                             ElementSet elements) {

            UIApplication uiapp = commandData.Application;
            UIDocument _uidoc = uiapp.ActiveUIDocument;
            Autodesk.Revit.ApplicationServices.Application _app = uiapp.Application;
            Autodesk.Revit.DB.Document _doc = _uidoc.Document;

            PlunkOClass plunkThis = new PlunkOClass(commandData.Application);
            string wsName = "MECH HVAC";
            string FamilyName = "M_BAS SENSOR";
            string FamilySymbolName = "THERMOSTAT";
            string FamilyTagName = "M_EQIP_BAS_SENSOR_TAG";
            string FamilyTagNameSymb = "SENSOR";
            string FamilyTagName2 = "M_DEVICE_BAS_TAG_SYM";
            string FamilyTagNameSymb2 = "M-DATA-SENSOR";
            bool hasLeader = false;
            bool oneShot = true;
            BuiltInCategory bicTagBeing = BuiltInCategory.OST_MechanicalEquipmentTags;
            BuiltInCategory bicTagBeing2 = BuiltInCategory.OST_DataDeviceTags;
            BuiltInCategory bicFamily = BuiltInCategory.OST_DataDevices;
            BuiltInCategory bicMechItem = BuiltInCategory.OST_MechanicalEquipment;
            Element elemPlunked;
            bool keepOnTruckn = true;

            while (keepOnTruckn) {
                try {
                    Result result = plunkThis.PlunkThisFamilyType(FamilyName, FamilySymbolName, wsName, bicFamily, out elemPlunked, oneShot);
                    FormMsgWPF formMsg = new FormMsgWPF();
                    if ((result == Result.Succeeded) & (elemPlunked != null)) {
                        formMsg.Show();
                        formMsg.SetMsg("Now pick the location for the offset symbol.", "Offset Sensor For MEQ");
                        plunkThis.AddThisTag(elemPlunked, FamilyTagName2, FamilyTagNameSymb2, "Offset Stat", bicTagBeing2, true);

                        formMsg.SetMsg("Now Select the Mech Unit for this sensor.", "Offset Sensor For MEQ");
                        Transaction tp = new Transaction(_doc, "PlunkOMatic:SymVis");
                        tp.Start();
                        Parameter parForVis = elemPlunked.LookupParameter("SHOW SYM");
                        if (null != parForVis) {
                            parForVis.Set(0);
                        }
                        plunkThis.OrientTheInsides(elemPlunked);  // left in in case type is changed later
                        tp.Commit();

                        ICollection<BuiltInCategory> categories = new[] { bicMechItem };
                        ElementFilter myPCatFilter = new ElementMulticategoryFilter(categories);
                        ISelectionFilter myPickFilter = SelFilter.GetElementFilter(myPCatFilter);
                        try {
                            Reference pickedElemRef = _uidoc.Selection.PickObject(ObjectType.Element, myPickFilter, "Select the Mech Unit for this sensor.");
                            Element pickedElem = _doc.GetElement(pickedElemRef.ElementId);
                            formMsg.SetMsg("Now place the unit text at the sensor.", "Offset Sensor For MEQ");
                            plunkThis.AddThisTag(pickedElem, FamilyTagName, FamilyTagNameSymb, "Offset Stat", bicTagBeing, hasLeader);
                            formMsg.Close();
                        } catch (Exception) {
                            formMsg.Close();
                            keepOnTruckn = false;
                            //throw;
                        }
                    } else {
                        formMsg.Close();
                        keepOnTruckn = false;
                    }
                } catch (Exception) {
                    keepOnTruckn = false;
                    //throw;
                }
            }
            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    class CmdPlaceStatOffsetForMechUnitInstance2 : IExternalCommand {
        public Result Execute(ExternalCommandData commandData,
                             ref string message,
                             ElementSet elements) {

            UIApplication uiapp = commandData.Application;
            UIDocument _uidoc = uiapp.ActiveUIDocument;
            Autodesk.Revit.ApplicationServices.Application _app = uiapp.Application;
            Autodesk.Revit.DB.Document _doc = _uidoc.Document;

            PlunkOClass plunkThis = new PlunkOClass(commandData.Application);
            string wsName = "MECH HVAC";
            string FamilyName = "M_BAS SENSOR";
            string FamilySymbolName = "THERMOSTAT";
            string FamilyTagName = "M_EQIP_BAS_SENSOR_TAG";
            string FamilyTagNameSymb = "TAG NUMBER ONLY";
            string FamilyTagName2 = "M_DEVICE_BAS_TAG_SYM";
            string FamilyTagNameSymb2 = "M-DATA-SENSOR";
            bool hasLeader = false;
            bool oneShot = true;
            BuiltInCategory bicTagBeing = BuiltInCategory.OST_MechanicalEquipmentTags;
            BuiltInCategory bicTagBeing2 = BuiltInCategory.OST_DataDeviceTags;
            BuiltInCategory bicFamily = BuiltInCategory.OST_DataDevices;
            BuiltInCategory bicMechItem = BuiltInCategory.OST_MechanicalEquipment;
            Element elemPlunked;
            bool keepOnTruckn = true;

            while (keepOnTruckn) {
                try {
                    Result result = plunkThis.PlunkThisFamilyType(FamilyName, FamilySymbolName, wsName, bicFamily, out elemPlunked, oneShot);
                    FormMsgWPF formMsg = new FormMsgWPF();
                    if ((result == Result.Succeeded) & (elemPlunked != null)) {
                        plunkThis.AddThisTag(elemPlunked, FamilyTagName2, FamilyTagNameSymb2, "Offset Stat", bicTagBeing2, true);
                        formMsg.Show();
                        formMsg.SetMsg("Now Select the Mech Unit for this sensor.", "Offset Sensor For MEQ");
                        Transaction tp = new Transaction(_doc, "PlunkOMatic:SymVis");
                        tp.Start();
                        Parameter parForVis = elemPlunked.LookupParameter("SHOW SYM");
                        if (null != parForVis) {
                            parForVis.Set(0);
                        }
                        plunkThis.OrientTheInsides(elemPlunked);  // left in in case type is changed later
                        tp.Commit();

                        ICollection<BuiltInCategory> categories = new[] { bicMechItem };
                        ElementFilter myPCatFilter = new ElementMulticategoryFilter(categories);
                        ISelectionFilter myPickFilter = SelFilter.GetElementFilter(myPCatFilter);
                        try {
                            Reference pickedElemRef = _uidoc.Selection.PickObject(ObjectType.Element, myPickFilter, "Select the Mech Unit for this sensor.");
                            Element pickedElem = _doc.GetElement(pickedElemRef.ElementId);
                            formMsg.SetMsg("Now place the unit text at the sensor.", "Offset Sensor For MEQ");
                            plunkThis.AddThisTag(pickedElem, FamilyTagName, FamilyTagNameSymb, "Offset Stat", bicTagBeing, hasLeader);
                            formMsg.Close();
                        } catch (Exception) {
                            formMsg.Close();
                            keepOnTruckn = false;
                            //throw;
                        }
                    } else {
                        formMsg.Close();
                        keepOnTruckn = false;
                    }
                } catch (Exception) {
                    keepOnTruckn = false;
                    //throw;
                }
            }
            return Result.Succeeded;
        }
    }

}


//[Transaction(TransactionMode.Manual)]
//class CmdPlaceStatForMechUnitInstanceOffset : IExternalCommand {
//    public Result Execute(ExternalCommandData commandData,
//                         ref string message,
//                         ElementSet elements) {

//        UIApplication uiapp = commandData.Application;
//        UIDocument _uidoc = uiapp.ActiveUIDocument;
//        Autodesk.Revit.ApplicationServices.Application _app = uiapp.Application;
//        Autodesk.Revit.DB.Document _doc = _uidoc.Document;

//        PlunkOClass plunkThis = new PlunkOClass(commandData.Application);
//        string wsName = "MECH HVAC";
//        string FamilyName = "M_BAS SENSOR";
//        string FamilySymbolName = "THERMOSTAT";
//        string pName = "STAT ZONE NUMBER";
//        string pNameVal = "Now Pick Equip";
//        string FamilyTagName = "M_DEVICE_BAS_TAG_SYM";
//        string FamilyTagNameSymb = "M-DATA-SENSOR";
//        string FamilyTagName2 = "M_DEVICE_BAS_SENSOR_TAG_NO";
//        string FamilyTagNameSymb2 = "SENSOR";
//        string pName2 = "STAT ZONE NUMBER";
//        string param1GetFromMech = "99.3 TAG NUMBER";
//        bool oneShot = true;
//        BuiltInCategory bicTagBeing = BuiltInCategory.OST_DataDeviceTags;
//        BuiltInCategory bicFamily = BuiltInCategory.OST_DataDevices;
//        BuiltInCategory _bicMechItem = BuiltInCategory.OST_MechanicalEquipment;
//        Element elemPlunked;

//        bool keepOnTruckn = true;

//        while (keepOnTruckn) {
//            try {
//                Result result = plunkThis.PlunkThisFamilyWithThisTagWithThisParameterSet(FamilyName, FamilySymbolName,
//                    pName, pNameVal, wsName, FamilyTagName, FamilyTagNameSymb, bicTagBeing, bicFamily, out elemPlunked, oneShot, true);

//                if ((result == Result.Succeeded) & (elemPlunked != null)) {
//                    ICollection<BuiltInCategory> categories = new[] { _bicMechItem };
//                    ElementFilter myPCatFilter = new ElementMulticategoryFilter(categories);
//                    ISelectionFilter myPickFilter = SelFilter.GetElementFilter(myPCatFilter);

//                    FormMsgWPF formMsg = new FormMsgWPF();
//                    formMsg.Show(new JtWindowHandle(ComponentManager.ApplicationWindow));
//                    formMsg.SetMsg("Now pick the location for the text tag.");

//                    plunkThis.AddThisTag(elemPlunked, FamilyTagName2, FamilyTagNameSymb2, pName2, bicTagBeing, false);

//                    formMsg.SetMsg("Now Select the Mech Unit for this sensor.");
//                    try {
//                        Reference pickedElemRef = _uidoc.Selection.PickObject(ObjectType.Element, myPickFilter, "Select the Mech Unit for this sensor.");
//                        Element pickedElem = _doc.GetElement(pickedElemRef.ElementId);
//                        formMsg.Close();
//                        Transaction tp = new Transaction(_doc, "PlunkOMatic:SetParam");
//                        tp.Start();
//                        // try to uncheck the show sym yes/no is 1/0
//                        Parameter parForVis = elemPlunked.LookupParameter("SHOW SYM");
//                        if (null != parForVis) {
//                            parForVis.Set(0);
//                        }

//                        if (plunkThis.HostedFamilyOrientation(_doc, elemPlunked)) {
//                            Parameter parForHoriz = elemPlunked.LookupParameter("HORIZONTAL");
//                            if (null != parForHoriz) {
//                                parForHoriz.Set(0);
//                            }
//                        }

//                        Parameter parForTag = pickedElem.LookupParameter(param1GetFromMech);
//                        if (null != parForTag) {
//                            //parForTag.SetValueString("PLUNKED");  // not for text, use for other
//                            Parameter parTagToSet = elemPlunked.LookupParameter(pName);
//                            if (null != parTagToSet) {
//                                parTagToSet.Set(parForTag.AsString());
//                            } else {
//                                TaskDialog.Show("There is not parameter named", pName);
//                            }
//                        } else {
//                            TaskDialog.Show("There is not parameter named", param1GetFromMech);
//                        }
//                        tp.Commit();
//                    } catch (Exception) {
//                        formMsg.Close();
//                        keepOnTruckn = false;
//                        //throw;
//                    }
//                } else {
//                    keepOnTruckn = false;

//                }
//            } catch (Autodesk.Revit.Exceptions.OperationCanceledException) {
//                keepOnTruckn = false;
//            }

//        }
//        return Result.Succeeded;
//    }
//}


//[Transaction(TransactionMode.Manual)]
//class CmdTwoPickMechSensorTag : IExternalCommand {
//    public Result Execute(ExternalCommandData commandData,
//                          ref string message,
//                          ElementSet elements) {

//        PlunkOClass plunkThis = new PlunkOClass(commandData.Application);
//        string wsName = "MECH HVAC";
//        string FamilyTagName = "M_DEVICE_BAS_SENSOR_TAG_NO";
//        string FamilyTagNameSymb = "SENSOR";
//        bool hasLeader = false;
//        bool oneShot = false;
//        BuiltInCategory bicTagBeing = BuiltInCategory.OST_DataDeviceTags;
//        BuiltInCategory bicItemBeingTagged = BuiltInCategory.OST_DataDevices;
//        Element elemTagged = null;

//        //string pName = "STAT ZONE NUMBER";

//        Result res = plunkThis.TwoPickTag(wsName, FamilyTagName, FamilyTagNameSymb, bicItemBeingTagged, bicTagBeing, hasLeader, oneShot, ref elemTagged);

//        return Result.Succeeded;
//    }
//}

//[Transaction(TransactionMode.Manual)]
//class CmdTwoPickMechSensorTagOff : IExternalCommand {
//    public Result Execute(ExternalCommandData commandData,
//                          ref string message,
//                          ElementSet elements) {

//        Document _doc = commandData.Application.ActiveUIDocument.Document;

//        PlunkOClass plunkThis = new PlunkOClass(commandData.Application);
//        string wsName = "MECH HVAC";
//        string FamilyTagName = "M_DEVICE_BAS_TAG_SYM";
//        string FamilyTagNameSymb = "M-DATA-SENSOR";
//        string FamilyTagName2 = "M_DEVICE_BAS_SENSOR_TAG_NO";
//        string FamilyTagNameSymb2 = "SENSOR";
//        BuiltInCategory bicItemBeingTagged = BuiltInCategory.OST_DataDevices;
//        BuiltInCategory bicTagBeing = BuiltInCategory.OST_DataDeviceTags;

//        Result result;

//        bool oneShot = true;
//        bool hasLeader = true;
//        Element elemTagged = null;

//        // first pass
//        result = plunkThis.TwoPickTag(wsName, FamilyTagName, FamilyTagNameSymb, bicItemBeingTagged, bicTagBeing, hasLeader, oneShot, ref elemTagged);

//        Transaction tp = new Transaction(_doc, "PlunkOMatic:SetParam");
//        tp.Start();
//        // try to uncheck the show sym yes/no is 1/0
//        Parameter parForVis = elemTagged.LookupParameter("SHOW SYM");
//        if (null != parForVis) {
//            parForVis.Set(0);
//        }
//        tp.Commit();

//        // second pass
//        if (elemTagged != null) {
//            hasLeader = false;
//            result = plunkThis.TwoPickTag(wsName, FamilyTagName2, FamilyTagNameSymb2, bicItemBeingTagged, bicTagBeing, hasLeader, oneShot, ref elemTagged);
//        }

//        return Result.Succeeded;
//    }
//}

//[Transaction(TransactionMode.Manual)]
//class CmdUpdateStat : IExternalCommand {
//    public Result Execute(ExternalCommandData commandData,
//                         ref string message,
//                         ElementSet elements) {

//        PlunkOClass plunkThis = new PlunkOClass(commandData.Application);
//        UIApplication uiapp = commandData.Application;
//        UIDocument _uidoc = uiapp.ActiveUIDocument;
//        Autodesk.Revit.ApplicationServices.Application _app = uiapp.Application;
//        Autodesk.Revit.DB.Document _doc = _uidoc.Document;

//        //  string FamilyName = "M_BAS SENSOR";
//        //  string FamilySymbolName = "THERMOSTAT";
//        string pName = "STAT ZONE NUMBER";
//        string param1GetFromMech = "99.3 TAG NUMBER";
//        BuiltInCategory _bicFamily = BuiltInCategory.OST_DataDevices;
//        BuiltInCategory _bicFamilyTag = BuiltInCategory.OST_DataDeviceTags;
//        BuiltInCategory _bicMechItem = BuiltInCategory.OST_MechanicalEquipment;

//        ICollection<BuiltInCategory> categoriesA = new[] { _bicFamily, _bicFamilyTag };
//        ElementFilter myPCatFilterA = new ElementMulticategoryFilter(categoriesA);
//        ISelectionFilter myPickFilterA = SelFilter.GetElementFilter(myPCatFilterA);

//        bool keepOnTruckn = true;
//        FormMsgWPF formMsg = new FormMsgWPF();

//        while (keepOnTruckn) {
//            try {
//                formMsg.Show(new JtWindowHandle(ComponentManager.ApplicationWindow));
//                formMsg.SetMsg("Pick the TStat to update tag data.");
//                Reference pickedElemRef = _uidoc.Selection.PickObject(ObjectType.Element, myPickFilterA, "Pick the TStat to update tag data. (ESC to cancel)");
//                Element pickedElem = _doc.GetElement(pickedElemRef.ElementId);
//                formMsg.Hide();
//                // get tagged element instead if user picked the tag
//                if (pickedElem.GetType() == typeof(IndependentTag)) {
//                    IndependentTag _tag = (IndependentTag)pickedElem;
//                    pickedElem = _doc.GetElement(_tag.TaggedLocalElementId);
//                }
//                if (pickedElem != null) {

//                    String selPrompt = "Select the Mech Unit for this sensor. (ESC to cancel)";
//                    plunkThis.SetParamValueToParmValue(pickedElem, pName, _bicMechItem, param1GetFromMech, selPrompt);
//                } else {
//                    keepOnTruckn = false;
//                }
//            } catch (Exception) {
//                formMsg.Close();
//                keepOnTruckn = false;
//                //throw;
//            }
//        }
//        formMsg.Close();
//        return Result.Succeeded;
//    }
//}