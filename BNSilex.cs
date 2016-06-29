using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace bnsilex {
	public class BNSilex {
		// mysql server
		private string m_host = "localhost";

		// mysql user
		private string m_user = "root";

		// mysql password
		private string m_password = "Sanko0061";

		// mysql database
		private string m_database = "bndb";

		private MySqlConnection m_conn = null;
		// connect database
		private void Open() {
			string connstr = string.Format("userid={0};password={1};database={2};host={3}", m_user, m_password, m_database, m_host);
			m_conn = new MySqlConnection(connstr);
		}

		// disconnect database
		private void Close() {
			m_conn.Close();
		}

		// make data PS common
		private void MakeDataPS(DataRow row, Dictionary<string, object> cols) {
			cols["KishuType"] = row["KishuType"];
			cols["KishuName"] = row["KishuName"];
			cols["Kadouchu"] = row["Kadouchu"];
			cols["T1chu"] = row["T1chu"];
			cols["T2chu"] = row["T2chu"];
			cols["RBchu"] = row["RBchu"];
			cols["最高獲得数"] = ((Func<int>)(() => {
				int a = int.Parse(row["MaxMochidama"].ToString());
				return Math.Max(0, a);
			}))();
			cols["最高継続回数"] = row["SaikouKeizokuSu"];
			cols["持玉数"] = ((Func<int>)(() => {
				int a = int.Parse(row["KokoMochidama"].ToString());
				int b = int.Parse(row["RuikeiMochidama"].ToString());
				int c = a;	// or b 設定を見る必要あり
				return Math.Max(0, c);
			}))();
			cols["出玉数"] = ((Func<int>)(() => {
				int a = int.Parse(row["RuikeiMochidama"].ToString());
				return Math.Max(0, a);
			}))();
			cols["過去最高獲得数"] = ((Func<int>)(() => {
				int kakoSaikouMochidama = int.Parse(row["kakoSaikouMochidama"].ToString());
				int maxMochidama = int.Parse(row["MaxMochidama"].ToString());
				return Math.Max(kakoSaikouMochidama, maxMochidama);
			}))();
			cols["過去最高継続回数"] = ((Func<int>)(() => {
				int kakoSaikouKeizokuSu = int.Parse(row["KakoSaikouKeizokuSu"].ToString());
				int saikouKeizokuSu = int.Parse(row["saikouKeizokuSu"].ToString());
				return Math.Max(kakoSaikouKeizokuSu, saikouKeizokuSu);
			}))();
		}

		// make data P
		private Dictionary<string, object> MakeDataP(DataRow row) {
			var cols = new Dictionary<string, object>();

			MakeDataPS(row, cols);

			cols["大当"] = row["T1Kaisu"];
			cols["確変"] = row["T2Kaisu"];
			cols["初当回数"] = row["TKaisu"];
			cols["スタート"] = row["SaishuTokushou1KanStart"];
			cols["総スタート"] = row["StartCount"];
			// 最高獲得数 = PS共通
			// 最高継続回数 = PS共通
			// 持玉数 = PS共通
			// 出玉数 = PS共通
			cols["大当確率"] = ((Func<int>)(() => {
				int t1Kaisu = int.Parse(row["T1Kaisu"].ToString());
				if (t1Kaisu == 0) return 0;
				int startCount = int.Parse(row["StartCount"].ToString());
				return startCount / t1Kaisu;
			}))();
			cols["初当確率"] = ((Func<int>)(() => {
				int tKaisu = int.Parse(row["TKaisu"].ToString());
				if (tKaisu == 0) return 0;
				int startCount = int.Parse(row["StartCount"].ToString());
				int tokushou2Start = int.Parse(row["Tokushou2Start"].ToString());
				int jitanStart = int.Parse(row["JitanStart"].ToString());
				return (startCount - tokushou2Start + jitanStart) / tKaisu;
			}))();
			cols["過去最高大当"] = ((Func<int>)(() => {
				int kakoSaikouT1Kaisu = int.Parse(row["KakoSaikouT1Kaisu"].ToString());
				int t1Kaisu = int.Parse(row["T1Kaisu"].ToString());
				return Math.Max(kakoSaikouT1Kaisu, t1Kaisu);
			}))();
			// 過去最高獲得数 = PS共通
			// 過去最高継続回数 = PS共通
			cols["過去最高確変回数"] = ((Func<int>)(() => {
				int kakoSaikouT2Kaisu = int.Parse(row["KakoSaikouT2Kaisu"].ToString());
				int t2Kaisu = int.Parse(row["T2Kaisu"].ToString());
				return Math.Max(kakoSaikouT2Kaisu, t2Kaisu);
			}))();
			return cols;
		}

		// make data S
		private Dictionary<string, object> MakeDataS(DataRow row) {
			var cols = new Dictionary<string, object>();

			MakeDataPS(row, cols);

			cols["BB"] = row["T1Kaisu"];
			cols["RB"] = row["RegularKaisu"];
			cols["ART"] = row["T2Kaisu"];
			cols["ボーナス回数"] = ((Func<int>)(() => {
				int t1Kaisu = int.Parse(row["T1Kaisu"].ToString());
				int regularKaisu = int.Parse(row["RegularKaisu"].ToString());
				int t2Kaisu = int.Parse(row["T2Kaisu"].ToString());
				return t1Kaisu + regularKaisu + t2Kaisu;
			}))();
			cols["ゲーム"] = ((Func<int>)(() => {
				int a = int.Parse(row["SaishuTokushou1kanOut"].ToString());
				return a / 3;
			}))();
			cols["総ゲーム"] = ((Func<int>)(() => {
				int a = int.Parse(row["GameSu"].ToString());
				return a / 3;
			}))();
			cols["ARTゲーム"] = ((Func<int>)(() => {
				int a = int.Parse(row["SaishuTokushou1kanOut"].ToString());
				return a / 3;
			}))();
			// 最高獲得数 = PS共通
			// 最高継続回数 = PS共通
			// 持玉数 = PS共通
			// 出玉数 = PS共通
			cols["BB確率"] = ((Func<int>)(() => {
				int a = int.Parse(row["T1Kaisu"].ToString());
				if (a == 0) return 0;
				int b = int.Parse(row["OutCount"].ToString());
				int c = int.Parse(row["TokushouOut"].ToString());
				int d = int.Parse(row["Tokushou2Out"].ToString());
				return ((b - c + d) / 3) / a;
			}))();
			cols["RB確率"] = ((Func<int>)(() => {
				int a = int.Parse(row["RegularKaisu"].ToString());
				if (a == 0) return 0;
				int b = int.Parse(row["OutCount"].ToString());
				int c = int.Parse(row["TokushouOut"].ToString());
				int d = int.Parse(row["Tokushou2Out"].ToString());
				return ((b - c + d) / 3) / a;
			}))();
			cols["ART確率"] = ((Func<int>)(() => {
				int a = int.Parse(row["T2Kaisu"].ToString());
				if (a == 0) return 0;
				int b = int.Parse(row["OutCount"].ToString());
				int c = int.Parse(row["TokushouOut"].ToString());
				int d = int.Parse(row["Tokushou2Out"].ToString());
				return ((b - c + d) / 3) / a;
			}))();
			cols["ボーナス確率"] = ((Func<int>)(() => {
				int a1 = int.Parse(row["T1Kaisu"].ToString());
				int a2 = int.Parse(row["RegularKaisu"].ToString());
				int a3 = int.Parse(row["T2Kaisu"].ToString());
				int a = a1 + a2 + a3;
				if (a == 0) return 0;
				int b = int.Parse(row["OutCount"].ToString());
				int c = int.Parse(row["TokushouOut"].ToString());
				int d = int.Parse(row["Tokushou2Out"].ToString());
				return ((b - c + d) / 3) / a;
			}))();
			cols["ART割合"] = ((Func<int>)(() => {
				int b = int.Parse(row["OutCount"].ToString());
				int c = int.Parse(row["TokushouOut"].ToString());
				int d = int.Parse(row["Tokushou2Out"].ToString());
				int a = b - c + d;
				if (a == 0) return 0;
				return ((d + 2) / 3) / (a / 3) * 100;
			}))();
			cols["過去最高BB回数"] = ((Func<int>)(() => {
				int a = int.Parse(row["KakoSaikouT1Kaisu"].ToString());
				int b = int.Parse(row["T1Kaisu"].ToString());
				return Math.Max(a, b);
			}))();
			cols["過去最高ART回数"] = ((Func<int>)(() => {
				int a = int.Parse(row["KakoSaikouT2Kaisu"].ToString());
				int b = int.Parse(row["T2Kaisu"].ToString());
				return Math.Max(a, b);
			}))();
			cols["過去最高ART回数"] = ((Func<int>)(() => {
				int a = int.Parse(row["KakoSaikouT2Kaisu"].ToString());
				int b = int.Parse(row["T2Kaisu"].ToString());
				return Math.Max(a, b);
			}))();
			cols["過去最高ARTゲーム数"] = ((Func<int>)(() => {
				int a = int.Parse(row["KakoSaikouT2Out"].ToString());
				int b = int.Parse(row["MaxMochidama"].ToString());
				return (Math.Max(a, b) + 2) / 3;
			}))();
			// 過去最高獲得数 = PS共通
			// 過去最高継続回数 = PS共通

			return cols;
		}

		// get data
		public void GetData(Dictionary<int,Dictionary<string, object>> data) {
			Open();

			// sql を実行
			MySqlDataAdapter da = new MySqlDataAdapter("select * from total_dai_today where YMDTarget='2016/04/08'", m_conn);
			DataTable dt = new DataTable();
			da.Fill(dt);

			Close();

			foreach (DataRow row in dt.Rows) {
				int daiNo = int.Parse(row["DaiNo"].ToString());
				int kishuType = int.Parse(row["KishuType"].ToString());

				data[daiNo] = (kishuType == 0 ? MakeDataP(row) : MakeDataS(row));
			}
		}
	}
}
