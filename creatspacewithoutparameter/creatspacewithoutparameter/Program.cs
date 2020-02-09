using System;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection;


namespace 拾取创建空间 //英文表达，添加注释
{
    [TransactionAttribute(TransactionMode.Manual)]

    public class cmdRoom : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string messages, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            Document doc = uiApp.ActiveUIDocument.Document;
            Selection sel = uiApp.ActiveUIDocument.Selection;

            Transaction ts = new Transaction(doc, "");
            ts.Start(); 

            Room room = doc.GetElement(sel.PickObject(ObjectType.Element, "选择一个房间")) as Room;
            LocationPoint roomPoint = room.Location as LocationPoint;
            //doc.Create.NewSpace(room.Level, room.PhaseCreated, new UV(roomPoint.Point.X, roomPoint.Point.Y));
            doc.Create.NewSpace(room.Level, new UV(roomPoint.Point.X, roomPoint.Point.Y));

            ts.Commit();

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]

    public class CreateSpace : IExternalCommand
    {
        public Result Execute(ExternalCommandData cmdData, ref string msg, ElementSet elements)
        {
            UIDocument uiDoc = cmdData.Application.ActiveUIDocument;

            try
            {
                Transaction ts = new Transaction(uiDoc.Document, "space");
                ts.Start();

                //Level
                Level level = null;
                FilteredElementIterator levelsIterator = (new FilteredElementCollector(uiDoc.Document)).OfClass(typeof(Level)).GetElementIterator();
                levelsIterator.Reset();
                while (levelsIterator.MoveNext())
                {
                    level = levelsIterator.Current as Level;
                    break;
                }
                using break faction >

                //Phase面域
                Parameter para = uiDoc.Document.ActiveView.get_Parameter(BuiltInParameter.VIEW_PHASE);
                ElementId phaseId = para.AsElementId();
                Phase phase = uiDoc.Document.get_Element(phaseId) as Phase;

                if (phase == null)
                {
                    System.Windows.Forms.MessageBox.Show("The phase of the active view is null, you can't create spaces in a null phase");
                }

                //CreateSpace
                if (uiDoc.Document.ActiveView.ViewType == ViewType.FloorPlan)
                {
                    uiDoc.Document.Create.NewSpaces(level, phase, uiDoc.ActiveView);
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("You can not create spaces in this plan view");
                }

                ts.Commit();
            }
            catch (Exception ex)
            {
                TaskDialog.Show("ex", ex.ToString());
            }

            return Result.Succeeded;
        }
    }
}