using System.Text;
using System.Web;

namespace DouYin.DownLoader.Common
{
    /// <summary>
    /// 代码参考：https://github.com/Evil0ctal/Douyin_TikTok_Download_API
    /// </summary>
    public class ABogus
    {
        private const string EndString = "cus";
        private const string StrS0 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
        private const string StrS1 = "Dkdpgh4ZKsQB80/Mfvw36XI1R25+WUAlEi7NLboqYTOPuzmFjJnryx9HVGcaStCe=";
        private const string StrS2 = "Dkdpgh4ZKsQB80/Mfvw36XI1R25-WUAlEi7NLboqYTOPuzmFjJnryx9HVGcaStCe=";
        private const string StrS3 = "ckdp1h4ZKsUB80/Mfvw36XIgR25+WQAlEi7NLwoqYTOPuzKFjJnryx9HVGDaStCe";
        private const string StrS4 = "Dkdpgh2ZmsQB80/MfvV36XI1R45-WUAlEixNLwoqYTOPuzKFjJnry79HbGcaStCe";
        private readonly int[] Env = { 49, 53, 51, 54, 124, 55, 52, 50, 124, 49, 53, 51, 54, 124, 56, 54, 52, 124, 48, 124, 48, 124, 48, 124, 48, 124, 49, 53, 51, 54, 124, 56, 54, 52, 124, 49, 53, 51, 54, 124, 56, 54, 52, 124, 49, 53, 51, 54, 124, 55, 52, 50, 124, 50, 52, 124, 50, 52, 124, 87, 105, 110, 51, 50 };
        private readonly long[] Reg = { 1937774191, 1226093241, 388252375, 3666478592, 2842636476, 372324522, 3817729613, 2969243214 };
        private readonly int[] Arguments = { 0, 1, 14 };
        private List<long> Chunk = new List<long>();
        private int Size = 0;
        private List<long> RegList = new List<long>();
        public string GetValue(string url, string userAgent)
        {
            var queryParams = System.Web.HttpUtility.ParseQueryString(new Uri(url).Query);
            var urlParams = new Dictionary<string, string>();
            foreach (string key in queryParams.AllKeys)
            {
                urlParams[key] = queryParams[key];
            }
            return GetValue(null, null, null, urlParams, userAgent);
        }
        private string GetValue(int? randomNum = 1, int? randomNum2 = 2, int? randomNum3 = 3, Dictionary<string, string> urlParams = null, string userAgent = "", long startTime = 0, long endTime = 0, string s = "s4")
        {
            var string1 = GenerateString1(randomNum, randomNum2, randomNum3);

            var string2 = GenerateString2(string.Join("&", urlParams.Select(kvp => $"{kvp.Key}={kvp.Value}")), userAgent, startTime, endTime);

            var string3 = string1 + string2;

            var s1 = GenerateResult(string3, 40, s);

            var s2 = GenerateResultEnd(string3, s);

            return s1 + s2;
        }
        private static List<int> List1(int? randomNum, int a = 170, int b = 85, int c = 45)
        {
            List<int> result = RandomList(randomNum, a, b, 1, 2, 5, c & a);
            return result;
        }



        private List<int> List2(int? randomNum, int a = 170, int b = 85)
        {
            var result = RandomList(randomNum, a, b, 1, 0, 0, 0);
            return result;
        }

        private List<int> List3(int? randomNum, int a = 170, int b = 85)
        {
            var result = RandomList(randomNum, a, b, 1, 0, 5, 0);
            return result;
        }

        private static List<int> RandomList(float? a = null, int b = 170, int c = 85, int d = 0, int e = 0, int f = 0, int g = 0)
        {
            var random = new Random();
            double r = a ?? (random.NextDouble() * 10000);
            int[] v = [(int)r, (int)r & 255, (int)r >> 8];
            int s = (v[1] & b) | d;
            v = AddToArray(v, s);
            s = (v[1] & c) | e;
            v = AddToArray(v, s);
            s = (v[2] & b) | f;
            v = AddToArray(v, s);
            s = (v[2] & c) | g;
            v = AddToArray(v, s);

            var result = new ArraySegment<int>(v, v.Length - 4, 4).ToList();

            return result;
        }
        private static T[] AddToArray<T>(T[] array, T element)
        {
            Array.Resize(ref array, array.Length + 1);
            array[array.Length - 1] = element;
            return array;
        }
        private string GenerateString1(int? randomNum = 1, int? randomNum2 = 2, int? randomNum3 = 3)
        {
            var result = FromCharCode(List1(randomNum).Select(x => (long)x).ToList())
             + FromCharCode(List2(randomNum2).Select(x => (long)x).ToList())
             + FromCharCode(List3(randomNum3).Select(x => (long)x).ToList());

            return result;
        }

        private string GenerateString2(string urlParams, string userAgent, long startTime = 0, long endTime = 0)
        {

            var a = GenerateString2List(urlParams, userAgent, startTime, endTime);

            int e = EndCheckNum(a);
            a.AddRange(Env.Select(x => (long)x));
            a.Add(e);

            var result = Rc4Encrypt(FromCharCode(a), "y");

            return result;

        }

        private List<long> GenerateString2List(string urlParams, string userAgent, long startTime = 0, long endTime = 0)
        {
            startTime = startTime == 0 ? (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds : startTime;
            endTime = endTime == 0 ? startTime + new Random().Next(4, 8) : endTime;
            var queryParams = HttpUtility.ParseQueryString(urlParams);
            var urlStr = string.Join("&", queryParams.AllKeys.Select(key => $"{key}={queryParams[key]}"));
            var paramsArray = Sum(urlStr);

            paramsArray = Sum("", paramsArray.Select(x => x).ToList());

            List<int> uaCode = new List<int> { 76, 98, 15, 131, 97, 245, 224, 133, 122, 199, 241, 166, 79, 34, 90, 191, 128, 126, 122, 98, 66, 11, 14, 40, 49, 110, 110, 173, 67, 96, 138, 252 };
            var result = List4(
             (endTime >> 24) & 255,
             paramsArray[21],
             uaCode[23],
             (endTime >> 16) & 255,
             paramsArray[22],
             uaCode[24],
             (endTime >> 8) & 255,
             endTime & 255,
             (startTime >> 24) & 255,
             (startTime >> 16) & 255,
             (startTime >> 8) & 255,
             startTime & 255
            );

            return result;
        }

        private List<long> RegToArray(List<long> a)
        {

            long[] o = new long[32];
            for (int i = 0; i < 8; i++)
            {
                long c = a[i];
                o[4 * i + 3] = 255 & c;
                c >>= 8;
                o[4 * i + 2] = 255 & c;
                c >>= 8;
                o[4 * i + 1] = 255 & c;
                c >>= 8;
                o[4 * i] = 255 & c;
            }

            return o.ToList();
        }

        private void Compress(List<long> a)
        {

            var f = GenerateF(a);
            var i = new List<long>(RegList);

            for (int o = 0; o < 64; o++)
            {
                long c = De(i[0], 12) + i[4] + De(Pe(o), o);
                c = c & 0xFFFFFFFF;
                c = De(c, 7);
                long s = (c ^ De(i[0], 12)) & 0xFFFFFFFF;
                long u = He(o, (int)i[0], (int)i[1], (int)i[2]);
                u = (u + i[3] + s + f[o + 68]) & 0xFFFFFFFF;
                long b = Ve(o, (int)i[4], (int)i[5], (int)i[6]);
                b = (b + i[7] + c + f[o]) & 0xFFFFFFFF;
                i[3] = i[2]; i[2] = De(i[1], 9);
                i[1] = i[0]; i[0] = u;
                i[7] = i[6]; i[6] = De(i[5], 19);
                i[5] = i[4];
                i[4] = (b ^ De(b, 9) ^ De(b, 17)) & 0xFFFFFFFF;

            }

            for (int l = 0; l < 8; l++)
            {
                RegList[l] = (RegList[l] ^ i[l]) & 0xFFFFFFFF;
            }

        }

        private List<long> GenerateF(List<long> e)
        {
            var r = new List<long>(new long[132]);

            for (int t = 0; t < 16; t++)
            {
                r[t] = (e[4 * t] << 24) | (e[4 * t + 1] << 16) | (e[4 * t + 2] << 8) | e[4 * t + 3]; r[t] &= 0xFFFFFFFFL;
            }
            for (int n = 16; n < 68; n++)
            {

                long a = r[n - 16] ^ r[n - 9] ^ De(r[n - 3], 15);
                a = a ^ De(a, 15) ^ De(a, 23);
                r[n] = (a ^ De(r[n - 13], 7) ^ r[n - 6]) & 0xFFFFFFFFL;

            }
            for (int n = 68; n < 132; n++)
            {
                r[n] = (r[n - 68] ^ r[n - 64]) & 0xFFFFFFFFL;
            }

            return r;
        }

        private void Fill(int length = 60)
        {
            long size = 8 * Size;

            Chunk.Add(128);

            while (Chunk.Count < length)
            {
                Chunk.Add(0);
            }

            for (int i = 0; i < 4; i++)
            {
                Chunk.Add((size >> 8 * (3 - i)) & 255);
            }

        }

        private List<long> List4(long a, long b, long c, long d, long e, long f, long g, long h, long i, long j, long k, long m)
        {
            var result = new List<long> { 44, a, 0, 0, 0, 0, 24, b, 58, 0, c, d, 0, 24, 97, 1, 0, 239, e, 51, f, g, 0, 0, 0, 0, h, 0, 0, 14, i, j, 0, k, m, 3, 399, 1, 399, 1, 64, 0, 0, 0 };
            return result;
        }

        private int EndCheckNum(List<long> a)
        {
            int r = 0;
            foreach (int i in a)
            {
                r ^= i;
            }
            return r;
        }

        private string FromCharCode(List<long> codes)
        {
            var result = string.Join("", codes.Select(code => (char)code));

            return result;
        }

        private long De(long e, int r)
        {
            r %= 32;
            var result = ((e << r) & 0xFFFFFFFF) | (e >> (32 - r));
            return result;
        }

        private int Pe(int e)
        {
            return e < 16 ? 2043430169 : 2055708042;
        }

        private int He(int e, int r, int t, int n)
        {
            if (e < 16)
            {
                return (int)((r ^ t ^ n) & 0xFFFFFFFF);
            }
            else if (e < 64)
            {
                return (int)((r & t | r & n | t & n) & 0xFFFFFFFF);
            }
            throw new ArgumentOutOfRangeException(nameof(e));
        }

        private int Ve(int e, int r, int t, int n)
        {
            if (e < 16)
            {
                return (int)((r ^ t ^ n) & 0xFFFFFFFF);
            }
            else if (e < 64)
            {
                return (int)((r & t | ~r & n) & 0xFFFFFFFF);
            }
            throw new ArgumentOutOfRangeException(nameof(e));
        }

        private List<long> ConvertToCharCode(string s)
        {
            return s.Select(c => (long)c).ToList();
        }

        private List<List<long>> SplitArray(List<long> arr, int chunkSize = 64)
        {
            var result = new List<List<long>>();
            for (int i = 0; i < arr.Count; i += chunkSize)
            {
                result.Add(arr.GetRange(i, Math.Min(chunkSize, arr.Count - i)));
            }
            return result;
        }

        private void Write(string? e, List<long> eList = null)
        {
            Size = eList?.Count ?? 0;
            if (!string.IsNullOrWhiteSpace(e))
            {
                e = HttpUtility.UrlDecode(e + EndString);
                eList = ConvertToCharCode(e);
                Size = eList.Count;
            }

            if (eList?.Count <= 64)
            {
                Chunk = eList;
            }
            else
            {
                var chunks = SplitArray(eList, 64);
                for (int i = 0; i < chunks.Count - 1; i++)
                {
                    Compress(chunks[i]);
                }
                Chunk = chunks.Last();
            }
         
        }

        private void Reset()
        {
            Chunk = new List<long>();
            Size = 0;
            RegList = new List<long>(Reg);
         
        }

        private List<long> Sum(string e, List<long> eList = null, int length = 60)
        {
          
            Reset();
            Write(e, eList);
            Fill(length);
            Compress(Chunk);
            var a = RegToArray(RegList);
            Reset();
            return a;
        }

        private string GenerateResultUnit(int n, string s)
        {
            var r = new StringBuilder();
            var str = s switch
            {
                "s0" => StrS0,
                "s1" => StrS1,
                "s2" => StrS2,
                "s3" => StrS3,
                _ => StrS4
            };
            for (int i = 18, j = 16515072; i >= 0; i -= 6, j >>= 6)
            {
                r.Append(str[(n & j) >> i]);
            }
            return r.ToString();
        }


        private string GenerateResultEnd(string s, string e = "s4")
        {
            var r = new StringBuilder();
            var str = e switch
            {
                "s0" => StrS0,
                "s1" => StrS1,
                "s2" => StrS2,
                "s3" => StrS3,
                _ => StrS4
            };
            int b = s[120] << 16;
            r.Append(str[(b & 16515072) >> 18]);
            r.Append(str[(b & 258048) >> 12]);
            r.Append("==");
            return r.ToString();
        }

        private string GenerateResult(string s, int n, string e = "s4")
        {
            var r = new StringBuilder();
            for (int i = 0; i < n; i++)
            {
                int b = (s[i * 3] << 16) | (s[i * 3 + 1] << 8) | s[i * 3 + 2]; r.Append(GenerateResultUnit(b, e));
            }
            return r.ToString();
        }

        private List<int> GenerateArgsCode()
        {
            var a = new List<int>();
            for (int j = 24; j >= 0; j -= 8)
            {
                a.Add((Arguments[0] >> j) & 255);
            }
            a.Add((Arguments[1] / 256) & 255);
            a.Add(Arguments[1] % 256);
            a.Add((Arguments[1] >> 24) & 255);
            a.Add((Arguments[1] >> 16) & 255);
            for (int j = 24; j >= 0; j -= 8)
            {
                a.Add((Arguments[2] >> j) & 255);
            }
            return a;
        }

        private string Rc4Encrypt(string plaintext, string key)
        {
            var s = Enumerable.Range(0, 256).ToList();
            int j = 0;
            for (int i = 0; i < 256; i++)
            {
                j = (j + s[i] + key[i % key.Length]) % 256; (s[i], s[j]) = (s[j], s[i]);
            }
            int i1 = 0;
            int j1 = 0;
            var cipher = new List<char>();
            for (int k = 0; k < plaintext.Length; k++)
            {
                i1 = (i1 + 1) % 256; j1 = (j1 + s[i1]) % 256; (s[i1], s[j1]) = (s[j1], s[i1]); int t = (s[i1] + s[j1]) % 256; cipher.Add((char)(s[t] ^ plaintext[k]));
            }
            return new string(cipher.ToArray());
        }

    }
}