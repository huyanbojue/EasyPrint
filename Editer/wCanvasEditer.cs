﻿/*
 * Copyright (C) 2016-2018
 * 由SharpDevelop创建。
 * 作者: Byron Gong
 * 日期: 03/09/2018 时间: 16:29
 * 邮箱: 1032066879@QQ.com
 * 描述: 画布属性编辑
 *
 */ 
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Spoon.Tools.TemplatePrint.Editer
{
	/// <summary>
	/// Description of wCanvasEditer.
	/// </summary>
	public partial class wCanvasEditer : UserControl
	{
		protected wCanvas m_control=null;
		protected EventHandler m_propertyEvent=null;
		
		public wCanvas Control{
			get{return m_control;}
			set{
				m_control=value;
				if(value!=null){
					InitData();
				}
			}
		}
		
		public int bLeft{
			get{return m_control.BackgroundRect.X;}
			set{
				var rect=m_control.BackgroundRect;
//				if(rect.X!=value){
					rect.X=value;
					Control.BackgroundRect=rect;
					txtLeftPixel.Text=value.ToString();
					txtLeftMm.Text=Helper.PrintHelper.DisplayToMm(value).ToString();
					m_control.Refresh();
//				}
			}
		}
		
		public int bTop{
			get{return m_control.BackgroundRect.Top;}
			set{
				var rect=m_control.BackgroundRect;
//				if(rect.Y!=value){
					rect.Y=value;
					Control.BackgroundRect=rect;
					txtTopPixel.Text=value.ToString();
					txtTopMm.Text=Helper.PrintHelper.DisplayToMm(value).ToString();
					m_control.Refresh();
//				}
			}
		}
		
		public int bWidth{
			get{return m_control.BackgroundRect.Width;}
			set{
				var rect=m_control.BackgroundRect;
//				if(rect.Width!=value){
					rect.Width=value;
					Control.BackgroundRect=rect;
					txtWidthPixel.Text=value.ToString();
					txtWidthMm.Text=Helper.PrintHelper.DisplayToMm(value).ToString();
					m_control.Refresh();
//				}
			}
		}
		
		public int bHeight{
			get{return m_control.BackgroundRect.Height;}
			set{
				var rect=m_control.BackgroundRect;
//				if(rect.Height!=value){
					rect.Height=value;
					Control.BackgroundRect=rect;
					txtHeightPixel.Text=value.ToString();
					txtHeightMm.Text=Helper.PrintHelper.DisplayToMm(value).ToString();
					m_control.Refresh();
//				}
			}
		}
		
		public float cWidth{
			get{return m_control.Width;}
			set{
                m_control.SizeF = new SizeF(value,m_control.SizeF.Height);
				txtSizeWidthPixel.Text=value.ToString();
				txtSizeWidthMm.Text=(Helper.PrintHelper.DisplayToMm((int)(value*100))/100.0F).ToString();
			}
		}
		
		public float cHeight{
			get{return m_control.Height;}
			set{
                m_control.SizeF = new SizeF(m_control.SizeF.Width, value);
                txtSizeHeightPixel.Text=value.ToString();
				txtSizeHeightMm.Text= (Helper.PrintHelper.DisplayToMm((int)(value * 100)) / 100.0F).ToString();
			}
		}
		
		protected void InitData(){
			if(Control==null) return;
			
			txtPath.Text=Control.BackgroundPath;
			bLeft=Control.BackgroundRect.Left;
			bTop=Control.BackgroundRect.Top;
			bWidth=Control.BackgroundRect.Width;
			bHeight=Control.BackgroundRect.Height;
			
			if(Control.BackgroundScale==0){
				ckScale.Checked=false;
				txtScale.Text="0";
			}else{
				ckScale.Checked=true;
				txtScale.Text=Control.BackgroundScale.ToString();
			}
			
			ckShowBackground.Checked=Control.ShowBackground;
			
			cWidth=Control.Width;
			cHeight=Control.Height;
			
			txtAuthor.Text=Control.Author;
			txtCreate.Text=Control.CreateTime.ToString();
		}
		
		public wCanvasEditer()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
		}
		
		public wCanvasEditer(wCanvas control){
			InitializeComponent();
			Control=control;
		}
		
		void OnTextBoxLeave(object sender,EventArgs e){
			var txt = sender as TextBox;
			if(Control==null || txt==null) return;
			
			switch (txt.Name) {
				case "txtPath":
					Control.BackgroundPath=txt.Text;
					break;
				case "txtScale":
					float scale;
					if(!float.TryParse(txt.Text,out scale)){
						MessageBox.Show("请输入有效的数值");
						txt.Focus();
						return;
					}
					bWidth=(int)(scale*Control.BackgroundImage.Width);
					bHeight=(int)(scale*Control.BackgroundImage.Height);
					Control.BackgroundScale=scale;
					Control.Refresh();
					break;
				case "txtLeftPixel":
					bLeft=int.Parse(txt.Text);
					break;
				case "txtTopPixel":
					bTop=int.Parse(txt.Text);
					break;
				case "txtWidthPixel":
					bWidth=int.Parse(txt.Text);
					break;
				case "txtHeightPixel":
					bHeight=int.Parse(txt.Text);
					break;
				case "txtLeftMm":
					bLeft=Helper.PrintHelper.MmToDisplay(float.Parse(txt.Text));
					break;
				case "txtTopMm":
					bTop=Helper.PrintHelper.MmToDisplay(float.Parse(txt.Text));
					break;
				case "txtWidthMm":
					bWidth=Helper.PrintHelper.MmToDisplay(float.Parse(txt.Text));
					break;
				case "txtHeightMm":
					bHeight=Helper.PrintHelper.MmToDisplay(float.Parse(txt.Text));
					break;
				case "txtSizeWidthPixel":
					cWidth=float.Parse(txt.Text);
					break;
				case "txtSizeHeightPixel":
					cHeight=float.Parse(txt.Text);
					break;
				case "txtSizeWidthMm":
					cWidth=Helper.PrintHelper.MmToDisplay(float.Parse(txt.Text)*100)/100.0F;
					break;
				case "txtSizeHeightMm":
					cHeight=Helper.PrintHelper.MmToDisplay(float.Parse(txt.Text)*100)/100.0F;
					break;
				case "txtAuthor":
					Control.Author=txt.Text;
					break;
			}
		}
		
		void OnTextBoxKeyDown(object sender,KeyEventArgs arg){
			if(arg.KeyCode==Keys.Enter){
				OnTextBoxLeave(sender,EventArgs.Empty);
			}
		}
		
		void BtnPathClick(object sender, EventArgs e)
		{
			if(Control==null){
				return;
			}
			using (var ofd=new OpenFileDialog()) {
				ofd.Filter="图片文件(*.jpeg;*.jpg;*.png;*.bmp;*.gif)|*.jpeg;*.jpg;*.png;*.bmp;*.gif|所有文件(*.*)|*.*";
				if(Control.BackgroundPath!=string.Empty){
					ofd.InitialDirectory=System.IO.Path.GetDirectoryName(Control.BackgroundPath);
					ofd.FileName=Control.BackgroundPath;
				}
				if(ofd.ShowDialog()==DialogResult.OK){
					txtPath.Text=ofd.FileName;
					Control.BackgroundPath=ofd.FileName;
				}
			}
		}
		
		void CkShowBackgroundCheckStateChanged(object sender, EventArgs e)
		{
			Control.ShowBackground=ckShowBackground.Checked;
		}
		
		void CkScaleCheckStateChanged(object sender, EventArgs e)
		{
			if(ckScale.Checked){
				txtWidthPixel.Enabled=false;
				txtWidthMm.Enabled=false;
				txtHeightPixel.Enabled=false;
				txtHeightMm.Enabled=false;
				txtScale.Enabled=true;
				txtScale.Focus();
				
			}else{
				txtWidthPixel.Enabled=true;
				txtWidthMm.Enabled=true;
				txtHeightPixel.Enabled=true;
				txtHeightMm.Enabled=true;
				txtScale.Enabled=false;
				
			}
		}
		void OnMoveButtonClick(object sender, EventArgs e)
		{
			if(Control==null) return;
			var btn = sender as Button;
			int step=Helper.PrintHelper.MmToDisplay(float.Parse(txtMoveStep.Text));
			switch (btn.Name) {
				case "btnMoveUp":
					Control.Offset(0,-step);
					break;
				case "btnMoveRight":
					Control.Offset(step,0);
					break;
				case "btnMoveDown":
					Control.Offset(0,step);
					break;
				case "btnMoveLeft":
					Control.Offset(-step,0);
					break;
			}
		}
    }
}
