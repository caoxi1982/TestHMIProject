#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.HMIProject;
using FTOptix.AuditSigning;
using FTOptix.NativeUI;
using FTOptix.UI;
using FTOptix.WebUI;
using FTOptix.RAEtherNetIP;
using FTOptix.Retentivity;
using FTOptix.CommunicationDriver;
using FTOptix.CoreBase;
using FTOptix.Core;
using FTOptix.NetLogic;
using FTOptix.OPCUAServer;
using FTOptix.OPCUAClient;
using System.Linq;
using System.Net.Http.Headers;
using FTOptix.Modbus;
using System.Threading;
#endregion

public class RuntimeNetLogic1 : BaseNetLogic
{
    public override void Start()
    {
        show = Project.Current.Get("Model/ShowPens");
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
    }
    [ExportMethod]
    public void addPen(string browserName)
    {
        var a = (Trend)Owner;
        var rnd = new Random();
        string penName = "Pen" + count + "_" + browserName;
        var newpen = InformationModel.MakeVariable<TrendPen>(penName, OpcUa.DataTypes.Float);        
        IUAVariable source = Project.Current.GetVariable(objectFolderName+ browserName);
        newpen.Color = new Color(255, (byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255));
        newpen.SetDynamicLink(source);
        a.Pens.Add(newpen);
        showedPens(browserName,penName);
        count++;
        //var threshold = InformationModel.MakeObject<TrendThreshold>("Threshold" + count++);
        //threshold.Color = new Color(255, 0, 0, 0);
        //(pen as TrendPen).Thresholds.Add(threshold);


    }

    [ExportMethod]
    public void deletePen(string browserName,string description)
    {
        var a = (Trend)Owner;
        //mypen = a.Pens.First();
        a.Pens.Remove(description);
        if (show != null)
            show.Children.Remove(browserName);
    }

    [ExportMethod]
    public void clearPen(string browserName, string description)
    {
        var a = (Trend)Owner;
        //mypen = a.Pens.First();
        a.Pens.Clear();
    }

    private void showedPens(string penName,string description)
    {
        var _penName = InformationModel.MakeVariable(penName, OpcUa.DataTypes.String);
        _penName.Description = new LocalizedText(text: description, textId: description, localeId: Project.Current.Localization.Locales[0]);
        show.Add(_penName);
    }
    private TrendPen? mypen;
    private int count = 0;
    private string objectFolderName = "Model/Pens/";
    private IUANode? show;
    

}
