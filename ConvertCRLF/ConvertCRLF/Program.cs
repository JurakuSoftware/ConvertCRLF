using System;
using System.IO;
using System.Text;

namespace ConvertCRLF
{
    /// <summary>
    /// 指定されたファイルの改行コードをCRLFに統一する
    /// ・指定されたファイルを上書き更新する
    /// ・指定されたファイルの文字コードはHnx8.ReadJEnc.dllを利用して自動判別している
    /// 
    /// プログラム戻り値
    /// 0 = 正常終了（変換完了）
    /// 1 = 異常終了（何らかのエラー発生。エラー内容はコンソール出力を参照）
    /// </summary>
    public class Program
    {
        /// <summary>
        /// プログラム戻り値（正常終了）
        /// </summary>
        private const int OK = 0;
        /// <summary>
        /// プログラム戻り値（異常終了）
        /// </summary>
        private const int NG = 1;

        /// <summary>
        /// 引数１で指定されたファイルパス
        /// </summary>
        private static string filePath = "";
        /// <summary>
        /// 引数１で指定されたファイルパスの文字コード
        /// </summary>
        private static Encoding encode;


        /// <summary>
        /// 改行コード変換処理
        /// 引数１：ファイルパス
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static int Main(string[] args)
        {
            try
            {
                //引数チェック
                if (!CheckArgs(args)) return NG;

                //ファイル文字コード取得
                encode = GetEncoding();

                //ファイル読み込み
                var fileData = ReadFile();

                //改行コード変更
                fileData = ConvertLineFeedCode(fileData);

                //ファイル書き込み
                WriteFile(fileData);
            }
            catch (Exception ex)
            {
                //何らかのエラー発生
                WriteLog("例外エラー発生", ex);
                return NG;
            }

            return OK;
        }

        /// <summary>
        /// 起動引数チェック
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static bool CheckArgs(string[] args)
        {
            //引数未指定チェック
            if (args == null || args.Length == 0) { WriteLog("ファイル名を指定してください"); return false; }

            //ファイル存在チェック
            filePath = args[0];
            if (!File.Exists(filePath)) { WriteLog("指定されたファイルは存在しません。パスに誤りが無いかをご確認ください"); return false; }

            //ファイル使用中チェック
            if (CheckFileInUse()) { WriteLog("指定されたファイルは使用中のため、変換出来ません"); return false; }

            return true;
        }

        /// <summary>
        /// ファイル使用中チェック
        /// </summary>
        /// <returns></returns>
        public static bool CheckFileInUse()
        {
            try
            {
                //移動元と移動先を同じにすることで、何も変化させない
                //ただし、Excelやサクラエディタなどで開いている場合は、例外エラーが発生するためこれを利用してファイル使用中と判断する
                File.Move(filePath, filePath);
            }
            catch (IOException)
            {
                //ファイル使用中
                return true;
            }

            //ファイル使用中でない
            return false;
        }

        /// <summary>
        /// 文字コード取得
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private static Encoding GetEncoding()
        {
            FileInfo fileInfo = new FileInfo(filePath);
            using (Hnx8.ReadJEnc.FileReader reader = new Hnx8.ReadJEnc.FileReader(fileInfo))
            {
                var enc = reader.Read(fileInfo).GetEncoding();
                WriteLog($"文字コード自動判定：{enc.EncodingName}");

                return enc;
            }
        }

        /// <summary>
        /// ファイル読み込み
        /// </summary>
        /// <returns></returns>
        private static string ReadFile()
        {
            using (var nowfile = new StreamReader(filePath, encode))
                return nowfile.ReadToEnd();
        }

        /// <summary>
        /// 改行コードをCRLFに変更
        /// </summary>
        /// <param name="fileData"></param>
        /// <returns></returns>
        private static string ConvertLineFeedCode(string fileData)
        {
            //改行コード変換（複数の改行コード混在が考えられるため、一旦全て\nに置換した後、\nを\r\nに置換する）
            return fileData.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\r\n");
        }

        /// <summary>
        /// ファイル書き込み
        /// </summary>
        /// <param name="fileData"></param>
        private static void WriteFile(string fileData)
        {
            using (var newfile = new StreamWriter(filePath, false, encode))
                newfile.Write(fileData);
        }

        /// <summary>
        /// ログ出力（コンソールへ）
        /// </summary>
        /// <param name="s"></param>
        /// <param name="ex"></param>
        private static void WriteLog(string s, Exception ex = null)
        {
            //例外エラー発生時は、その内容も記載
            if (ex != null) s = $"[ERROR]{s}\r\n{ex.ToString()}";

            //コンソール出力
            Console.WriteLine(s);
        }
    }
}
