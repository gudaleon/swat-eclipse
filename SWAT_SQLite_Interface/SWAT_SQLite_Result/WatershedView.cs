﻿using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SWAT_SQLite_Result
{
    public partial class WatershedView : UserControl
    {
        public WatershedView()
        {
            InitializeComponent();
        }

        private string _resultType = null;
        private string _col = null;
        private DateTime _date = DateTime.Now;

        private ArcSWAT.Project _project = null;
        private ArcSWAT.ScenarioResult _scenario = null;
        private ArcSWAT.SWATUnitType _type = ArcSWAT.SWATUnitType.UNKNOWN;
 
        public void setProjectScenario(ArcSWAT.Project project, ArcSWAT.ScenarioResult scenario)
        {
            _project = project;
            _scenario = scenario;
            _type = ArcSWAT.SWATUnitType.WSHD;
            _date = new DateTime(scenario.StartYear, 1, 1);

            //columns
            resultColumnTree1.onResultTypeAndColumnChanged += (resultType, col) =>
            {
                _resultType = resultType;
                _col = col;

                updateTableAndChart();
            };
            resultColumnTree1.setScenarioAndUnit(scenario, ArcSWAT.SWATUnitType.WSHD);

            //chart export
            outputDisplayChart1.onExport += (s, e) =>
                {

                };

            //update
            updateTableAndChart();

            this.tableView2.SWATResultTable = this._scenario.Watershed.AverageAnnualBasinTable;
        }

        private void updateTableAndChart()
        {
            if (_resultType == null || _col == null) return;

            if (!this._scenario.Watershed.Results.ContainsKey(_resultType)) return;

            ArcSWAT.SWATUnitResult result = this._scenario.Watershed.Results[_resultType];
            if (!result.Columns.Contains(_col)) return;

            DataTable dt = result.getDataTable(_col);

            this.tableView1.SWATResultTable = dt;            

            StringCollection cols = new StringCollection() { _col };
            this.outputDisplayChart1.DrawGraph(dt.Rows, ArcSWAT.SWATUnitResult.COLUMN_NAME_DATE, cols, result.Interval);

            //this.lblStatistics.Text = "Statistics :" + result.getStatistics(_col,year).ToString();           
        }
    }
}