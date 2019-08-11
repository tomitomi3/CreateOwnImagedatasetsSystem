# 概要
機械学習による画像の認識・検出には、一般的に大量の教師用画像データを用いた学習が必要です。学習の『技法』は話題になりますが、教師用画像データの生成はあまり触れられた事は無いと思います。今回、その部分に注目し、使用する環境を考慮した教師用画像データ生成を行うシステムを作成してみました。

## システム構成及び全景

### Marker Faire Tokyo 2019 展示Ver

DNNの演算を可能とするMPU、カメラ、LCDが一体化しているM5StickVというモノがある。機械学習（特に深層学習、DNN）の成果をPC内に留めず、ユーザーが欲しい機械学習アプリケーションを身近にできるはず。より機械学習の要件（したいこと）とデータが求めらる事になると考える。先手というわけではないが、データを身近に作るシステムというモノを作り展示した。

![image alt](https://raw.githubusercontent.com/tomitomi3/CreateOwnImagedatasetsSystem/master/_pic/coids_mft2019.jpg) 

* [機械学習のための教師用画像データ生成＆認識システム(Maker Faire Tokyo 2019 紹介ページ)](https://makezine.jp/event/makers-mft2019/m0124/)

Makezineのブログで紹介してもらいました↓

* [Maker Faire Tokyo 2019の見どころ ＃４](https://makezine.jp/blog/2019/07/mft2019_4_ai.html)

### Marker Faire Tokyo 2019 申請時

* システム構成図

![system block diagram](https://raw.githubusercontent.com/tomitomi3/CreateOwnImagedatasetsSystem/master/_pic/system_block.png)

* システム全景

![image alt](https://raw.githubusercontent.com/tomitomi3/CreateOwnImagedatasetsSystem/master/_pic/coids_1.jpg) 

* ソフトウェア

![image alt](https://raw.githubusercontent.com/tomitomi3/CreateOwnImagedatasetsSystem/master/_pic/collect_and_recognize_soft.png)

## Related Project

* [サイコロの目認識のための教師用画像データセット](https://github.com/tomitomi3/DiceRecognitionDatasetForML)
* [サイコロを振ってサイコロの出た目を読み取るシステム](https://github.com/tomitomi3/DiceRecognizeSystem)
