using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace db_test
{
    class DialogClass
    {
        public string OpenFileByDialog(string initFile, string fileFilter)
        {
            string ret=null;

            try
            {
                // テキストボックスからファイル名 (ファイルパス) を取得
                string fileName = initFile;

                // OpenFileDialog クラスのインスタンスを生成
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    // ファイルの種類リストを設定
//                    openFileDialog.Filter = "テキストファイル (*.txt)|*.txt|すべてのファイル (*.*)|*.*";
                    openFileDialog.Filter = fileFilter;

                    //存在しないファイルの名前が指定されたとき警告を表示する
                    //デフォルトでTrueなので指定する必要はない
                    openFileDialog.CheckFileExists = false;
                    //存在しないパスが指定されたとき警告を表示する
                    //デフォルトでTrueなので指定する必要はない
                    openFileDialog.CheckPathExists = true;
                    
                    // テキストボックスにファイル名 (ファイルパス) が設定されている場合は
                    // ファイルのディレクトリー (フォルダー) を初期表示する
                    if (fileName != string.Empty & fileName != null)
                    {
                        // FileInfo クラスのインスタンスを生成
                        FileInfo fileInfo = new FileInfo(fileName);
                        // ディレクトリー名 (ディレクトリーパス) を取得
                        string directoryName = fileInfo.DirectoryName;
                        // 存在する場合は InitialDirectory プロパティに設定
                        if (Directory.Exists(directoryName))
                        {
                            openFileDialog.InitialDirectory = directoryName;
                        }
                    }

                    // ダイアログを表示
                    DialogResult dialogResult = openFileDialog.ShowDialog();
                    if (dialogResult == DialogResult.Cancel)
                    {
                        // キャンセルされたので終了
                        ret = null;
                    }

                    // 選択されたファイル名 (ファイルパス) をテキストボックスに設定
                    ret = openFileDialog.FileName;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return ret;

        }

        public string OpenFolderByDialog(string initFolder, string folderFilter)
        {
            string ret = null;

            try
            {
                // テキストボックスからフォルダーパスを取得
                string folderPath = initFolder;

                // FolderBrowserDialog クラスのインスタンスを生成
                using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
                {
                    // 説明文を設定
                    folderBrowserDialog.Description = "フォルダーを選択してください。";

                    // テキストボックスにフォルダーパスが設定されている場合は選択する
                    if (folderPath != string.Empty)
                    {
                        folderBrowserDialog.SelectedPath = folderPath;
                    }

                    // ダイアログを表示
                    DialogResult dialogResult = folderBrowserDialog.ShowDialog();
                    if (dialogResult == DialogResult.Cancel)
                    {
                        // キャンセルされたので終了
                        ret = null;
                    }

                    // 選択されたフォルダーパスをテキストボックスに設定
                    ret = folderBrowserDialog.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return ret;

        }

    }
}
