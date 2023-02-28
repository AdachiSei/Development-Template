# DevelopmentTemplate

長期開発用テンプレート
制作中

# 命名規則

## Unity

- 変数名は[キャメルケース](https://e-words.jp/w/%E3%82%AD%E3%83%A3%E3%83%A1%E3%83%AB%E3%82%B1%E3%83%BC%E3%82%B9.html) (先頭小文字)

- メンバー変数の接頭辞には「＿」(アンダースコア)を付けること

- bool型変数の接頭辞には「is」を付けること

- 定数は[スネークケース](https://e-words.jp/w/%E3%82%B9%E3%83%8D%E3%83%BC%E3%82%AF%E3%82%B1%E3%83%BC%E3%82%B9.html#:~:text=%E3%82%B9%E3%83%8D%E3%83%BC%E3%82%AF%E3%82%B1%E3%83%BC%E3%82%B9%E3%81%A8%E3%81%AF%E3%80%81%E3%83%97%E3%83%AD%E3%82%B0%E3%83%A9%E3%83%9F%E3%83%B3%E3%82%B0,%E3%81%AA%E8%A1%A8%E8%A8%98%E3%81%8C%E3%81%93%E3%82%8C%E3%81%AB%E5%BD%93%E3%81%9F%E3%82%8B%E3%80%82)
(全て大文字、単語と単語の間には「＿」(アンダースコア))

- 関数　クラス　プロパティの名前は[パスカルケース](https://wa3.i-3-i.info/word13955.html) (先頭大文字)

- イベントの接頭辞には「On」を付けること

| 種類 | 例 |
| ---------- | ----------- |
| 変数      | memberName |
| メンバー変数 | _●● |
| bool型変数 | is●● |
| 定数 | MEMBER_NAME |
| 関数 クラス プロパティ | MethodName |
| イベント| On●● |

## Sourcetree

- ブランチの名前は[スネークケース](https://e-words.jp/w/%E3%82%B9%E3%83%8D%E3%83%BC%E3%82%AF%E3%82%B1%E3%83%BC%E3%82%B9.html#:~:text=%E3%82%B9%E3%83%8D%E3%83%BC%E3%82%AF%E3%82%B1%E3%83%BC%E3%82%B9%E3%81%A8%E3%81%AF%E3%80%81%E3%83%97%E3%83%AD%E3%82%B0%E3%83%A9%E3%83%9F%E3%83%B3%E3%82%B0,%E3%81%AA%E8%A1%A8%E8%A8%98%E3%81%8C%E3%81%93%E3%82%8C%E3%81%AB%E5%BD%93%E3%81%9F%E3%82%8B%E3%80%82)
(単語と単語の間には「＿」(アンダースコア)) `branch_name`

- 機能を作成するブランチであれば接頭辞に「feature/」を付けてください `feature/branch_name`

- 機能の修正等は接頭辞に「hotfix/」を付けてください `hotfix/branch_name`

- 削除を行う際は接頭辞に「remove/」を付けてください `remove/branch_name`

| ブランチ | 例 |
| ---------- | ----------- |
| 機能作成 | feature/branch_name |
| 機能修正 | hotfix/branch_name |
| 機能削除 | remove/branch_name |
