# ipTestApp
- 任意のパケットをUDP/IPまたはTCP/IPで送受信するWindowsアプリケーションです。
- いろいろ問題あり。

---

## 概要

- csv形式のパケット定義ファイルを読み込んで送信します。
- 今はUDPしかできません。

### 開発環境

- Visual Studio2017
  - MaterialDesignInXamlToolkit 
  - MahApps.Metro

---

## 使用方法

### 必要なもの
- Visual Studio 2017 の Microsoft Visual C++ 再頒布可能パッケージ
  - （https://visualstudio.microsoft.com/ja/downloads/?q=#other-ja）
- .Net Framework 4.6.1以降
  
### 準備と操作

ファイル選択ボタンを押して，読み込むcsvファイルを選択します。
- csvの形式は次のようにしなければなりません。
  - 番号，オフセット，データ名，データ型，値，単位，配列数
    - 番号：1～ （読み込み時にはとくに使用しません）
    - オフセット：これまでのバイト数 （読み込み時にはとくに使用しません）
    - データ名：データの名前，意味
    - データ型：次の種類のみ対応しています。
      - INT，UINT，SHORT，USHORT，FLOAT，DOUBLE，CHAR
    - 値：データ型の範囲内の値
    - 単位：必要なら（オプション）
    - 配列数：配列でないなら1を入れる。
  
  - 例
    - 1行目：1, 0, テスト1(速度), INT, 15, m/s, 1
    - 2行目：2, 4, テスト2(重量), DOUBLE, 100, kg, 1

  - 先頭に#を付けるとコメント行として無視されます。


送信停止中ボタンを押すと送信開始します。
