using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TheArtOfDev.HtmlRenderer.WinForms;

namespace bnsilex {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
		}

		//MimeTypeで指定されたImageCodecInfoを探して返す
		private static System.Drawing.Imaging.ImageCodecInfo
			GetEncoderInfo(string mineType) {
			//GDI+ に組み込まれたイメージ エンコーダに関する情報をすべて取得
			System.Drawing.Imaging.ImageCodecInfo[] encs =
				System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
			//指定されたMimeTypeを探して見つかれば返す
			foreach (System.Drawing.Imaging.ImageCodecInfo enc in encs) {
				if (enc.MimeType == mineType) {
					return enc;
				}
			}
			return null;
		}

		//ImageFormatで指定されたImageCodecInfoを探して返す
		private static System.Drawing.Imaging.ImageCodecInfo
			GetEncoderInfo(System.Drawing.Imaging.ImageFormat f) {
			System.Drawing.Imaging.ImageCodecInfo[] encs =
				System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
			foreach (System.Drawing.Imaging.ImageCodecInfo enc in encs) {
				if (enc.FormatID == f.Guid) {
					return enc;
				}
			}
			return null;
		}

		public LinearGradientBrush GetRainbowLinearGradientBrush(Rectangle rc) {
			LinearGradientBrush lgb = new LinearGradientBrush(rc, Color.Black, Color.Black, LinearGradientMode.Vertical);

			ColorBlend cb = new ColorBlend();
			cb.Positions = new[] { 0, 1 / 6f, 2 / 6f, 3 / 6f, 4 / 6f, 5 / 6f, 1 };
			cb.Colors = new[] { Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Indigo, Color.Violet };
			
			lgb.InterpolationColors = cb;

			// rotate
			//lgb.RotateTransform(45);
			//lgb.RotateTransform(90);

			return lgb;
		}

		public GraphicsPath GetString(string s, Rectangle rc) {
			GraphicsPath path = new GraphicsPath();
			StringFormat sf = new StringFormat();
			sf.Alignment = StringAlignment.Center;
			sf.LineAlignment = StringAlignment.Center;
			path.AddString(s, FontFamily.GenericSansSerif, (int)FontStyle.Bold, 12, rc, sf);
			//path.AddString(s, FontFamily.GenericSansSerif, 0, 9, rc, sf);
			return path;
		}

		public GraphicsPath GetRoundRect(Rectangle rect, int radius) {
			GraphicsPath path = new GraphicsPath();

			path.StartFigure();

			// 左上の角丸
			path.AddArc(rect.Left, rect.Top,
				radius * 2, radius * 2,
				180, 90);
			// 上の線
			path.AddLine(rect.Left + radius, rect.Top,
				rect.Right - radius, rect.Top);
			// 右上の角丸
			path.AddArc(rect.Right - radius * 2, rect.Top,
				radius * 2, radius * 2,
				270, 90);
			// 右の線
			path.AddLine(rect.Right, rect.Top + radius,
				rect.Right, rect.Bottom - radius);
			// 右下の角丸
			path.AddArc(rect.Right - radius * 2, rect.Bottom - radius * 2,
				radius * 2, radius * 2,
				0, 90);
			// 下の線
			path.AddLine(rect.Right - radius, rect.Bottom,
				rect.Left + radius, rect.Bottom);
			// 左下の角丸
			path.AddArc(rect.Left, rect.Bottom - radius * 2,
				radius * 2, radius * 2,
				90, 90);
			// 左の線
			path.AddLine(rect.Left, rect.Bottom - radius,
				rect.Left, rect.Top + radius);

			path.CloseFigure();

			return path;
		}
	
		// HhtmlRenderer は、実験的に使ってみた
		private void DrawMyHtml(Dictionary<int, Dictionary<string, object>> data) {
			Bitmap canvas = new Bitmap(800, 600);
			Graphics g = Graphics.FromImage(canvas);
			g.Clear(Color.White);

			PointF point = new PointF(0, 0);
			SizeF maxSize = new System.Drawing.SizeF(500, 500);
			HtmlRender.Render(g,
				"<html><body><p>This is a shitty html code</p><p>This is another html line</p></body>",
				point, maxSize);
			g.Dispose();
			canvas.Save("no2.jpg", ImageFormat.Jpeg);
		}

		private void DrawDaiNo(Graphics g, int dno, int ofs) {
			StringFormat sf = new StringFormat();
			sf.Alignment = StringAlignment.Center;
			sf.LineAlignment = StringAlignment.Center;

			Font fnt = new Font("MS UI Gothic", 20);

			Rectangle rc = new Rectangle(0, 0, 115, 64);
			rc.Offset(0, 96 * ofs);
			LinearGradientBrush lgb = new LinearGradientBrush(rc, Color.White, Color.FromArgb(220, 155, 183), LinearGradientMode.Vertical);
			g.FillRectangle(lgb, rc);

			g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
			g.DrawString(dno.ToString(), fnt, Brushes.Black, rc, sf);

			rc = new Rectangle(118, 0, 283 - 118, 94);
			rc.Offset(0, 96 * ofs);
			lgb = new LinearGradientBrush(rc, Color.FromArgb(244, 238, 185), Color.FromArgb(219, 201, 46), LinearGradientMode.Vertical);
			g.FillRectangle(lgb, rc);

			rc = new Rectangle(288, 0, 443 - 288, 94);
			rc.Offset(0, 96 * ofs);
			lgb = new LinearGradientBrush(rc, Color.White, Color.FromArgb(219, 212, 212), LinearGradientMode.Vertical);
			g.FillRectangle(lgb, rc);

			rc = new Rectangle(448, 0, 881-448, 94);
			rc.Offset(0, 96 * ofs);
			//lgb = new LinearGradientBrush(rc, Color.White, Color.FromArgb(219, 212, 212), LinearGradientMode.Vertical);
			g.FillRectangle(lgb, rc);

			rc = new Rectangle(886, 0, 1075 - 886, 94);
			rc.Offset(0, 96 * ofs);
			//lgb = new LinearGradientBrush(rc, Color.White, Color.FromArgb(219, 212, 212), LinearGradientMode.Vertical);
			g.FillRectangle(lgb, rc);

			g.SmoothingMode = SmoothingMode.AntiAlias;
			rc = new Rectangle(906, 4, 971 - 906, 20-4);
			rc.Offset(0, 96 * ofs);
			lgb = new LinearGradientBrush(rc, Color.FromArgb(205, 16, 74), Color.FromArgb(162, 0, 6), LinearGradientMode.Vertical);
			g.FillPath(lgb, GetRoundRect(rc, 8));
			g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
			//g.DrawString("スタート", fnt, Brushes.Black, rc, sf);
			GraphicsPath gp = GetString("スタート", rc);
			g.DrawPath(Pens.Black, gp);
			g.FillPath(Brushes.White, gp);

			g.SmoothingMode = SmoothingMode.AntiAlias;
			rc = new Rectangle(906, 4, 971 - 906, 20 - 4);
			rc.Offset(992-906, 96 * ofs);
			lgb = new LinearGradientBrush(rc, Color.FromArgb(76, 176, 54), Color.FromArgb(4, 117, 86), LinearGradientMode.Vertical);
			g.FillPath(lgb, GetRoundRect(rc, 8));
			g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
			//g.DrawString("スタート", fnt, Brushes.Black, rc, sf);
			gp = GetString("総スタート", rc);
			g.DrawPath(Pens.Black, gp);
			g.FillPath(Brushes.White, gp);

			g.SmoothingMode = SmoothingMode.AntiAlias;
			rc = new Rectangle(906, 4, 971 - 906, 20 - 4);
			rc.Offset(0, 96 * ofs + (48 - 4));
			lgb = GetRainbowLinearGradientBrush(rc);
			g.FillPath(lgb, GetRoundRect(rc, 8));
			g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
			//g.DrawString("スタート", fnt, Brushes.Black, rc, sf);
			gp = GetString("最高出玉", rc);
			g.DrawPath(Pens.Black, gp);
			g.FillPath(Brushes.White, gp);

			g.SmoothingMode = SmoothingMode.AntiAlias;
			rc = new Rectangle(906, 4, 971 - 906, 20 - 4);
			rc.Offset(992 - 906, 96 * ofs + (48 - 4));
			lgb = new LinearGradientBrush(rc, Color.FromArgb(253, 241, 7), Color.FromArgb(242, 158, 0), LinearGradientMode.Vertical);
			g.FillPath(lgb, GetRoundRect(rc, 8));
			g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
			//g.DrawString("スタート", fnt, Brushes.Black, rc, sf);
			gp = GetString("大当", rc);
			g.DrawPath(Pens.Black, gp);
			g.FillPath(Brushes.White, gp);

			//g.DrawRectangle(Pens.Blue, rc);
		}

		private void DrawMyCanvas(Dictionary<int, Dictionary<string, object>> data) {
			Bitmap canvas = new Bitmap(1080, 1920);	// Silexの表示ぴったりサイズ
			Graphics g = Graphics.FromImage(canvas);
			g.Clear(Color.White);
			Font fnt = new Font("MS UI Gothic", 20);
			
			float x = 0;
			float y = 0;
			int ofs = 0;
			foreach (int daiNo in data.Keys) {
				SizeF size = g.MeasureString("9999", fnt);
				//g.DrawString(daiNo.ToString(), fnt, Brushes.Black, x, y);
				DrawDaiNo(g, daiNo, ofs++);

				Dictionary<string, object> daiData = data[daiNo];
				if (daiData["KishuType"].ToString() == "0") {
					// P表示
					//g.DrawString(daiData["出玉数"].ToString(), fnt, Brushes.Black, x+size.Width, y);
				} else {
					// S表示
					//g.DrawString(daiData["出玉数"].ToString(), fnt, Brushes.Black, x + size.Width, y);
				}

				y += size.Height;
			}

			Rectangle rc = new Rectangle(0, 0, 10, 10);
			g.DrawRectangle(Pens.Red, rc);
			g.DrawRectangle(Pens.Green, 15, 0, 10, 10);

			g.Dispose();

			EncoderParameters eps = new EncoderParameters(1);
			//品質を指定
			System.Drawing.Imaging.EncoderParameter ep =
				new System.Drawing.Imaging.EncoderParameter(
				System.Drawing.Imaging.Encoder.Quality, (long)100);

			//EncoderParametersにセットする
			eps.Param[0] = ep;

			//イメージエンコーダに関する情報を取得する
			System.Drawing.Imaging.ImageCodecInfo ici = GetEncoderInfo("image/jpeg");


			canvas.Save("no1.jpg", ici, eps);
			canvas.Save("no1.png", ImageFormat.Png);
			//canvas.Save("no1.jpg", ImageFormat.Jpeg);
			//pictureBox1.Image = canvas;
		}

		private void button1_Click(object sender, EventArgs e) {
			Cursor save = Cursor.Current;
			Cursor.Current = Cursors.WaitCursor;

			DateTime dt = DateTime.Now;

			BNSilex bns = new BNSilex();

			Dictionary<int,Dictionary<string,object>> data = new Dictionary<int,Dictionary<string,object>>();
			bns.GetData(data);

			DrawMyCanvas(data);
			//DrawMyHtml(data);

			label1.Text = string.Format("所要時間: {0} 秒", (DateTime.Now - dt).TotalSeconds);
			Cursor.Current = save;
		}
	}
}
