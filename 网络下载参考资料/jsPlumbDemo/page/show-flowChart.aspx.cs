﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPMS.Domain;

public partial class prototype_project_reply_show_flowChart : System.Web.UI.Page
{

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			string str = Request["path"];
			StreamReader sr = new StreamReader(str);
			string jsonText = sr.ReadToEnd();

			List<JsPlumbConnect> list = new JavaScriptSerializer().Deserialize<List<JsPlumbConnect>>(jsonText.Split('&')[0]);
			List<JsPlumbBlock> blocks = new JavaScriptSerializer().Deserialize<List<JsPlumbBlock>>(jsonText.Split('&')[1]);
			string htmlText = "";
			string conn = "";
			if (blocks.Count > 0)
			{
				foreach (JsPlumbBlock block in blocks)
				{
					if(block.BlockContent=="开始"||block.BlockContent=="结束")
						htmlText += "<div class='node radius' id='" + block.BlockId + "'style='left:"+block.BlockX+"px;top:"+block.BlockY+"px;' >" + block.BlockContent + "</div>";
					else
						htmlText += "<div class='node' id='" + block.BlockId + "'style='left:" + block.BlockX + "px;top:" + block.BlockY + "px;' >" + block.BlockContent + "</div>";
				}

				foreach (JsPlumbConnect jsplum in list)
				{
					if (jsplum.ConnectText== null)
						conn +=
							"jsPlumb.bind(\"connection\",function (connInfo, originalEvent) {	connInfo.connection.setLabel(\" \")});";
					else
						conn +=
						  "jsPlumb.bind(\"connection\",function (connInfo, originalEvent) {	connInfo.connection.setLabel(\"<span style='display:block;padding:10px;opacity: 0.5;height:auto;background-color:white;border:1px solid #346789;text-align:center;font-size:12px;color:black;border-radius:0.5em;'>" + jsplum.ConnectText + "</span>\")});";
					conn += "jsPlumb.connect({ source: \"" + jsplum.PageSourceId + "\", target: \"" + jsplum.PageTargetId +
					        "\" ,anchors:[\"" + jsplum.SourceAnchor + "\",\"" + jsplum.TargetAnchor + "\"]},flowConnector);";
					
				}
				Literal1.Text = htmlText;
				string script = "jsPlumb.ready(function () {" + conn + "});";
				ClientScript.RegisterStartupScript(this.GetType(), "myscript", script, true);
			}
		}
	}
}