using System;
using CrystalDecisions.Web;
namespace CrystalReportHelper
{
    public interface ICrystalReportPageControler
    {
        System.Web.UI.Page Owner { get; set; }
        string ReportPath { get; set; }
        CrystalDecisions.Web.CrystalReportViewer ReportViewer { get; set; }
        CrystalDecisions.Shared.ConnectionInfo ConnectionInfo { get; set; }
        
        event EventHandler FillReportParameters;

        void ConfigureCrystalReports();
        void Intialize();
        void SetDBLogonForReport(CrystalDecisions.Shared.ConnectionInfo connectionInfo);
        void SetReportParametersValues();
        void SetReportParameterValue(CrystalDecisions.Shared.ParameterField parameterField, object value);
        void SetReportParameterValue(string parameterField, object value);
        void CheckReportHasData(CrystalReportViewer cRV);

    }

}