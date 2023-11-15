using System.Collections.Generic;
using System.Text.RegularExpressions;

/// <summary>
/// String型の拡張メソッド
/// https://soft-rime.com/post-2289/
/// </summary>
public static class StringExtension
{
    public static string GetExtractedData(this string dataList, string frontPat, string endPat)
    {
        // データの抽出のためのマッチパターン(正規表現）
        // (ExtractedData:抽出したデータの意味）
        var anchor = $"({frontPat})(?<ExtractedData>.*?)({endPat})";

        // 正規表現でマッチする条件のオプションセット
        var options =
                    // 大文字小文字の違いを無視
                    RegexOptions.IgnoreCase |

                    // 単一行モード（改行を含む文字列でも、それを改行とみなさず他の文字と同等に処理）
                    RegexOptions.Singleline;

        // 抽出したデータをセットする変数（List)
        var extractedData = new List<string>();

        // 正規表現でマッチしたデータのみ抽出
        foreach (Match match in Regex.Matches(dataList, anchor, options))
        {
            // マッチしたデータのみ抽出データにAddする。
            // .ValueでMatchクラスのデータ部分のみ取得できる
            extractedData.Add(match.Groups["ExtractedData"].Value);
        }

        // 条件にマッチしたデータセット
        return extractedData[0].Trim();
    }
}