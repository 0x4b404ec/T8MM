    
/****************************************************************
 *          Copyright (c) 2024 0x4b404ec MIT License            *
 *                                                              *
 *          0x4b404ec (https://github.com/0x4b404ec)            *
 ****************************************************************/

using System;
using System.Security.Cryptography;
using System.Text;

namespace T8MM.Utils;

public class LocalCryptography
{
    /// <summary>
    /// RSA
    /// </summary>
    /// <param name="express"></param>
    /// <param name="keyContainerName"></param>
    /// <returns></returns>
    public static string Encryp(string express, string keyContainerName = null)
    {

        CspParameters param = new CspParameters();
        param.KeyContainerName = keyContainerName ?? "T8MM";
        using RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(param);
        byte[] plaindata = Encoding.Default.GetBytes(express);
        byte[] encryptdata = rsa.Encrypt(plaindata, false);
        return Convert.ToBase64String(encryptdata);
    }

    /// <summary>
    /// RSA
    /// </summary>
    /// <param name="ciphertext"></param>
    /// <param name="keyContainerName"></param>
    /// <returns></returns>
    public static string Decrypt(string ciphertext, string keyContainerName = null)
    {
        CspParameters param = new CspParameters();
        param.KeyContainerName = keyContainerName ?? "T8MM"; //密匙容器的名称，保持加密解密一致才能解密成功
        using RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(param);
        byte[] encryptdata = Convert.FromBase64String(ciphertext);
        byte[] decryptdata = rsa.Decrypt(encryptdata, false);
        return Encoding.Default.GetString(decryptdata);
    }
}