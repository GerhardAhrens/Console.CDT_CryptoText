//-----------------------------------------------------------------------
// <copyright file="CryptoText.cs" company="Lifeprojects.de">
//     Class: CryptoText
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <Framework>8.0</Framework>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>18.09.2025</date>
//
// <summary>
// Struct Class for Custom DataType CryptoText
// </summary>
//-----------------------------------------------------------------------

namespace System
{
    using System.Security.Cryptography;
    using System.Text;

    public struct CryptoText : IEquatable<CryptoText>, IComparable<CryptoText>
    {
        private readonly string _value;

        public CryptoText(string value)
        {
            this._value = value;
        }

        public string Value
        {
            get
            {
                return this._value;
            }
        }

        public static string Default
        {
            get { return string.Empty; }
        }

        public bool IsNullOrEmpty
        {
            get { return string.IsNullOrEmpty(this.Value); }
        }

        public int Length
        {
            get
            {
                if (string.IsNullOrEmpty(this.Value) == true)
                {
                    return 0;
                }
                else
                {
                    return this.Value.Length;
                }
            }
        }

        public bool IsNull
        {
            get
            {
                if (this.Value == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static implicit operator CryptoText(string value)
        {
            if (value.EndsWith("==") == true)
            {
                string encryptString = CryptoHelperInternal.Decrypt(value);
                return new CryptoText(encryptString);
            }
            else
            {

                string encryptString = CryptoHelperInternal.Encrypt(value);
                return new CryptoText(encryptString);
            }
        }

        public static bool operator ==(CryptoText left, CryptoText right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CryptoText left, CryptoText right)
        {
            return !left.Equals(right);
        }

        public static CryptoText Encrypt(string plainText)
        {
            string encryptString = CryptoHelperInternal.Encrypt(plainText);
            return new CryptoText(encryptString);
        }

        public static string Decrypt(CryptoText cryptText)
        {
            string decryptString = CryptoHelperInternal.Decrypt(cryptText.Value);
            return decryptString;
        }

        public static string Decrypt(string cryptText)
        {
            string decryptString = CryptoHelperInternal.Decrypt(cryptText);
            return decryptString;
        }

        #region Check Funktionen
        public bool IsBase64String()
        {
            bool result = false;

            if (string.IsNullOrEmpty(this.Value) || this.Value.Length % 4 != 0 || this.Value.Contains(" ") 
                || this.Value.Contains("\t") || this.Value.Contains("\r") || this.Value.Contains("\n"))
            {
                return result;
            }

            Span<byte> buffer = new Span<byte>(new byte[this.Value.Length]);
            result = Convert.TryFromBase64String(this.Value, buffer, out int bytesParsed);
            return result;
        }
        #endregion Check Funktionen

        public override int GetHashCode()
        {
            HashCode hashCode = new HashCode();
            hashCode.Add(this.Value);
            return hashCode.ToHashCode();
        }

        #region Implementation of IEquatable<Base64>
        public override bool Equals(object obj)
        {
            if ((obj is CryptoText) == false)
            {
                return false;
            }

            CryptoText other = (CryptoText)obj;
            return Equals(other);
        }

        public bool Equals(CryptoText other)
        {
            return this.Value == other.Value;
        }
        #endregion Implementation of IEquatable<Base64>

        #region Konvertierung nach To...
        public override string ToString()
        {
            return this.Value.ToString();
        }

        #endregion Konvertierung nach To...

        #region Implementation of IComparable<Base64>

        public int CompareTo(CryptoText other)
        {
            int valueCompare = this.Value.CompareTo(other.Value);

            return valueCompare;
        }
        #endregion Implementation of IComparable<Base64>


        private class CryptoHelperInternal
        {
            private static readonly byte[] internalKey = { 0x16, 0x15, 0x14, 0x13, 0x11, 0x10, 0x09, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };
            private static readonly byte[] internalVector = { 0x16, 0x15, 0x14, 0x13, 0x11, 0x10, 0x09, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };


            public static string Decrypt(string pInputString)
            {
                string outputString = string.Empty;

                if (string.IsNullOrEmpty(pInputString) == true)
                {
                    return string.Empty;
                }

                try
                {
                    using (MemoryStream inStream = new MemoryStream())
                    {
                        byte[] inBytes = Convert.FromBase64String(pInputString);
                        inStream.Write(inBytes, 0, inBytes.Length);
                        inStream.Position = 0;

                        MemoryStream outStream = new MemoryStream();
                        byte[] buffer = new byte[128];

#pragma warning disable SYSLIB0045 // Typ oder Element ist veraltet
                        SymmetricAlgorithm algorithm = SymmetricAlgorithm.Create("Rijndael");
#pragma warning restore SYSLIB0045 // Typ oder Element ist veraltet
                        algorithm.IV = internalVector;
                        algorithm.Key = internalKey;
                        ICryptoTransform transform = algorithm.CreateDecryptor();
                        CryptoStream cryptedStream = new CryptoStream(inStream, transform, CryptoStreamMode.Read);

                        int restLength = cryptedStream.Read(buffer, 0, buffer.Length);
                        while (restLength > 0)
                        {
                            outStream.Write(buffer, 0, restLength);
                            restLength = cryptedStream.Read(buffer, 0, buffer.Length);
                        }

                        outputString = System.Text.Encoding.Default.GetString(outStream.ToArray());

                        cryptedStream.Close();
                        cryptedStream = null;
                        inStream.Close();
                        outStream.Close();
                        outStream = null;
                    }

                    return outputString;
                }
                catch (Exception)
                {
                    throw;
                }
            }

            public static string Encrypt(string pInputString)
            {
                string outputString = string.Empty;

                if (string.IsNullOrEmpty(pInputString) == true)
                {
                    return string.Empty;
                }

                try
                {
                    using (MemoryStream inStream = new MemoryStream())
                    {
                        byte[] inBytes = new byte[pInputString.Length];
                        inBytes = System.Text.Encoding.Default.GetBytes(pInputString);
                        inStream.Write(inBytes, 0, inBytes.Length);
                        inStream.Position = 0;

                        MemoryStream outStream = new MemoryStream();
                        byte[] buffer = new byte[128];

#pragma warning disable SYSLIB0045 // Typ oder Element ist veraltet
                        SymmetricAlgorithm algorithm = SymmetricAlgorithm.Create("Rijndael");
#pragma warning restore SYSLIB0045 // Typ oder Element ist veraltet
                        algorithm.IV = internalVector;
                        algorithm.Key = internalKey;
                        ICryptoTransform transform = algorithm.CreateEncryptor();
                        CryptoStream cryptedStream = new CryptoStream(outStream, transform, CryptoStreamMode.Write);

                        int restLength = inStream.Read(buffer, 0, buffer.Length);
                        while (restLength > 0)
                        {
                            cryptedStream.Write(buffer, 0, restLength);
                            restLength = inStream.Read(buffer, 0, buffer.Length);
                        }

                        cryptedStream.FlushFinalBlock();

                        outputString = System.Convert.ToBase64String(outStream.ToArray());

                        cryptedStream.Close();
                        cryptedStream = null;
                        inStream.Close();
                        outStream.Close();
                        outStream = null;
                    }

                    return outputString;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// var symmetricEncryptDecrypt = new CryptoHelperCOREInternal();
        /// var(Key, IVBase64) = symmetricEncryptDecrypt.InitSymmetricEncryptionKeyIV();
        /// var encryptedText = symmetricEncryptDecrypt.Encrypt(text, IVBase64, Key);
        /// var decryptedText = symmetricEncryptDecrypt.Decrypt(encryptedText, IVBase64, Key);
        /// </summary>
        /// <remarks>
        /// https://damienbod.com/2020/08/19/symmetric-and-asymmetric-encryption-in-net-core/
        /// </remarks>
        private class CryptoHelperCOREInternal
        {
            public (string Key, string IVBase64) InitSymmetricEncryptionKeyIV()
            {
                string key = GetEncodedRandomString(32); // 256 Byte
                Aes cipher = CreateCipher(key);
                string IVBase64 = Convert.ToBase64String(cipher.IV);
                return (key, IVBase64);
            }

            private string GetEncodedRandomString(int length)
            {
                string base64 = Convert.ToBase64String(GenerateRandomBytes(length));
                return base64;
            }

            private Aes CreateCipher(string keyBase64)
            {
                // Default values: Keysize 256, Padding PKC27
                Aes cipher = Aes.Create();
                cipher.Mode = CipherMode.CBC;  // Ensure the integrity of the ciphertext if using CBC

                cipher.Padding = PaddingMode.ISO10126;
                cipher.Key = Convert.FromBase64String(keyBase64);

                return cipher;
            }

            private byte[] GenerateRandomBytes(int length)
            {
                byte[] byteArray = new byte[length];
                RandomNumberGenerator.Fill(byteArray);
                return byteArray;
            }

            public string Encrypt(string text, string IV, string key)
            {
                Aes cipher = CreateCipher(key);
                cipher.IV = Convert.FromBase64String(IV);

                ICryptoTransform cryptTransform = cipher.CreateEncryptor();
                byte[] plaintext = Encoding.UTF8.GetBytes(text);
                byte[] cipherText = cryptTransform.TransformFinalBlock(plaintext, 0, plaintext.Length);

                return Convert.ToBase64String(cipherText);
            }

            public string Decrypt(string encryptedText, string IV, string key)
            {
                Aes cipher = CreateCipher(key);
                cipher.IV = Convert.FromBase64String(IV);

                ICryptoTransform cryptTransform = cipher.CreateDecryptor();
                byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                byte[] plainBytes = cryptTransform.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

                return Encoding.UTF8.GetString(plainBytes);
            }
        }
    }
}