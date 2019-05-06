# 概要
機械学習による画像の認識・検出には、一般的に大量の教師用画像データを用いた学習が必要です。学習の『技法』は話題になりますが、教師用画像データの生成はあまり触れられた事は無いと思います。今回、その部分に注目し、使用する環境を考慮した教師用画像データ生成を行うシステムを作成してみました。

## システム構成図
![system block diagram](https://raw.githubusercontent.com/tomitomi3/CreateOwnImagedatasetsSystem/master/_pic/system_block.png)

## システム全景
* システム全景
![image alt](https://raw.githubusercontent.com/tomitomi3/CreateOwnImagedatasetsSystem/master/_pic/coids_1.jpg) 

* ソフトウェア
![image alt](https://raw.githubusercontent.com/tomitomi3/CreateOwnImagedatasetsSystem/master/_pic/collect_and_recognize_soft.png)

## Related Project

* [サイコロの目認識のための教師用画像データセット](https://github.com/tomitomi3/DiceRecognitionDatasetForML)
* [サイコロを振ってサイコロの出た目を読み取るシステム](https://github.com/tomitomi3/DiceRecognizeSystem)
