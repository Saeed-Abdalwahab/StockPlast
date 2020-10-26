using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.UI;
using CrystalDecisions.Shared;
using System.Collections;
using CrystalDecisions.Web;
using System.Reflection;

namespace CrystalReportHelper
{
    /// <summary>
    /// Summary description for CrystalReportPage

    /// </summary>
    public class CrystalReportPageControler : ICrystalReportPageControler
    {
        public CrystalReportPageControler(Page owner)
        {
            this.owner = owner;
        }

        #region ICrystalReportControler Members
        private Page owner;
        public Page Owner
        {
            get { return owner; }
            set { owner = value; }
        }
        CrystalDecisions.Web.CrystalReportViewer _reportViewer;
        public CrystalDecisions.Web.CrystalReportViewer ReportViewer
        {
            set { _reportViewer = value; }
            get { return _reportViewer; }
        }

        private string reportPath;
        public string ReportPath
        {
            get { return reportPath; }
            set { reportPath = value; }
        }

        private CrystalDecisions.Shared.ConnectionInfo connectionInfo;
        public CrystalDecisions.Shared.ConnectionInfo ConnectionInfo
        {
            get { return connectionInfo; }
            set { connectionInfo = value; }
        }

        public void ConfigureCrystalReports()
        {
            //Define Connection
            if (connectionInfo == null)
            {
                connectionInfo = new ConnectionInfo();
                connectionInfo.ServerName = ConfigurationManager.AppSettings["DataLayer.Server"];
                connectionInfo.DatabaseName = ConfigurationManager.AppSettings["DataLayer.Database"];
                connectionInfo.UserID = ConfigurationManager.AppSettings["DataLayer.UserName"];
                connectionInfo.Password = ConfigurationManager.AppSettings["DataLayer.Password"];
                connectionInfo.IntegratedSecurity = bool.Parse(ConfigurationManager.AppSettings["DataLayer.IntegratedSecurity"]);
            }
            ReportViewer.ReportSource = reportPath;

            SetDBLogonForReport(connectionInfo);

        }

        public void SetDBLogonForReport(CrystalDecisions.Shared.ConnectionInfo connectionInfo)
        {

            TableLogOnInfos tableLogOnInfos = ReportViewer.LogOnInfo;
            foreach (TableLogOnInfo tableLogOnInfo in tableLogOnInfos)
            {
                tableLogOnInfo.ConnectionInfo = connectionInfo;
            }
        }



        public void SetReportParameterValue(CrystalDecisions.Shared.ParameterField parameterField, object value)
        {
            ParameterValues currentParameterValues = new ParameterValues();
            ParameterDiscreteValue parameterDiscreteValue = new ParameterDiscreteValue();
            parameterDiscreteValue.Value = value;
            currentParameterValues.Add(parameterDiscreteValue);
            parameterField.CurrentValues = currentParameterValues;
        }

        
        ParameterFields parameterFields
        {
            get { return ReportViewer.ParameterFieldInfo; }
        }

        public void SetReportParametersValues()
        {
            if (FillReportParameters != null)
                FillReportParameters(this, EventArgs.Empty);
            _reportViewer.RefreshReport();
        }

        public event EventHandler FillReportParameters;
        #endregion

        public void SetReportParameterValue(string parameterField, object value)
        {
            SetReportParameterValue(parameterFields[parameterField], value);
        }

        public void Intialize()
        {
            ConfigureCrystalReports();
            _reportViewer.RefreshReport();

            if (owner == null) return;
            if (!owner.IsPostBack)
            {
                SetReportParametersValues();
            }
        }

        public void CheckReportHasData(CrystalReportViewer cRV)
        {
            object cRS = this.ReportViewer.ReportSource;
            PropertyInfo propertyReportDeocument = cRS.GetType().GetProperty("ReportDocument",BindingFlags.NonPublic);

            object objReportDocument = propertyReportDeocument.GetValue(cRS, null);
            PropertyInfo propertyRows = objReportDocument.GetType().GetProperty("Rows");

            object objRows = propertyRows.GetValue(objReportDocument, null);
            PropertyInfo rowsCount = objRows.GetType().GetProperty("Count");

            int count = Convert.ToInt32(rowsCount.GetValue(objRows, null));

            if (count != 0)
            {
                cRV.Visible = true;
            }
            else
            {
                cRV.Visible = false;
            }
            throw new Exception("Count is " + count);
        }

    }

}