using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class SaveSystem
{
    private static readonly string EncryptionKey = "ChaveSecretaDe32CaracteresAqui!"; // 32 caracteres (256 bits)

    public static void SaveSlot(int slotIndex, GameData data)
    {
        string json = JsonUtility.ToJson(data, true);
        byte[] encryptedData = Encrypt(json, EncryptionKey);

        string path = GetPathForSlot(slotIndex);
        File.WriteAllBytes(path, encryptedData);

        // Se for salvo em um slot normal (1, 2, 3), replica automaticamente no Slot 0 (Autosave)
        if (slotIndex != 0)
        {
            File.WriteAllBytes(GetPathForSlot(0), encryptedData);
        }
    }

    public static GameData LoadSlot(int slotIndex)
    {
        string path = GetPathForSlot(slotIndex);
        if (!File.Exists(path)) return null;

        try
        {
            byte[] encryptedData = File.ReadAllBytes(path);
            string json = Decrypt(encryptedData, EncryptionKey);
            return JsonUtility.FromJson<GameData>(json);
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao decriptar o save no slot {slotIndex}: {e.Message}");
            return null;
        }
    }

    public static bool SlotExists(int slotIndex)
    {
        return File.Exists(GetPathForSlot(slotIndex));
    }

    private static string GetPathForSlot(int slotIndex)
    {
        return Path.Combine(Application.persistentDataPath, $"save_slot_{slotIndex}.dat");
    }

    #region Crypto AES
    private static byte[] Encrypt(string plainText, string key)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        using (Aes aes = Aes.Create())
        {
            aes.Key = keyBytes;
            aes.GenerateIV();
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(aes.IV, 0, aes.IV.Length);
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                using (StreamWriter sw = new StreamWriter(cs))
                {
                    sw.Write(plainText);
                }
                return ms.ToArray();
            }
        }
    }

    private static string Decrypt(byte[] cipherText, string key)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        using (MemoryStream ms = new MemoryStream(cipherText))
        {
            byte[] iv = new byte[16];
            ms.Read(iv, 0, iv.Length);
            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = iv;
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                using (StreamReader sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
    #endregion
}