# ConvertCRLF
テキストファイルの改行コードをCRLFに統一するコンソールアプリ  
プログラム起動引数で指定されたファイルの改行コードを書き換えます（上書き保存）

## 使用例
ConvertCRLF.exe "C:\temp\hoge.txt"  
もしくは、ConvertCRLF.exeにファイルをドラッグ＆ドロップすれば変換されます。  
  
※変換後、元には戻せませんので、予めバックアップを取得しておいてから実行してください！

## 戻り値
0:正常終了（変換成功）  
1:異常終了（変換失敗。エラー詳細はコンソール出力を参照）

## 使用ライブラリ
Hnx8.ReadJEnc.dll（文字コード判別） 1.3.1.2

## ライセンス
MITライセンス

## 作成
https://juraku-software.net/  
  
コンパイルしてすぐに使える状態のEXEは以下のページで公開しています。  
https://juraku-software.net/windows-app-convertcrlf/
