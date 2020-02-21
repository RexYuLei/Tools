using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class WebAPIHelper : MonoBehaviour
{
    public static string MD5(string s)
    {
        var provider = System.Security.Cryptography.MD5.Create();
        StringBuilder builder = new StringBuilder();
        foreach (var b in provider.ComputeHash(Encoding.UTF8.GetBytes(s)))
        {
            builder.Append(b.ToString("x2").ToString());
        }
        return builder.ToString();
    }

    public static string GenerateRailSignature(string url, string railAccessKey)
    {
        HMACSHA1 hmacsha1 = new HMACSHA1();
        hmacsha1.Key = System.Text.Encoding.UTF8.GetBytes(railAccessKey);
        byte[] dataBuffer = System.Text.Encoding.UTF8.GetBytes(url);
        byte[] hashBytes = hmacsha1.ComputeHash(dataBuffer);
        string signature = Convert.ToBase64String(hashBytes);
        //url safe
        signature = signature.Replace("/", "_");
        signature = signature.Replace("+", "-");
        return signature;
    }
}
