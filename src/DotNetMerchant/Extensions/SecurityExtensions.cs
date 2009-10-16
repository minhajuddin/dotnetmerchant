#region License

// The MIT License
// 
// Copyright (c) 2009 Conatus Creative, Inc.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

#endregion

using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace DotNetMerchant.Extensions
{
    internal static class SecurityExtensions
    {
        private static readonly byte[] _entropy = Encoding.Unicode.GetBytes("rosebud");

        public static byte[] Encrypt(this byte[] data)
        {
            if (data.LongLength == 0)
            {
                return data;
            }

            var encrypted = ProtectedData.Protect(data, _entropy, DataProtectionScope.CurrentUser);
            return encrypted;
        }

        public static string Encrypt(this SecureString input)
        {
            if (input == null)
            {
                return null;
            }

            var bytes = Encoding.Unicode.GetBytes(input.Insecure());
            var encrypted = bytes.Encrypt();

            var output = Convert.ToBase64String(encrypted);
            Array.Clear(encrypted, 0, encrypted.Length);

            return output;
        }

        public static byte[] Decrypt(this byte[] data)
        {
            if (data.LongLength == 0)
            {
                return data;
            }

            var decrypted = ProtectedData.Unprotect(data, _entropy, DataProtectionScope.CurrentUser);
            return decrypted;
        }

        public static SecureString Decrypt(this string encryptedData)
        {
            if (encryptedData == null)
            {
                return null;
            }

            try
            {
                var bytes = Convert.FromBase64String(encryptedData);
                var decrypted = bytes.Decrypt();

                var output = Encoding.Unicode.GetString(decrypted).Secure();
                Array.Clear(decrypted, 0, decrypted.Length);

                return output;
            }
            catch
            {
                return new SecureString();
            }
        }

        public static SecureString Secure(this string input)
        {
            if (input == null)
            {
                return null;
            }

            var secure = new SecureString();
            foreach (var c in input)
            {
                secure.AppendChar(c);
            }
            secure.MakeReadOnly();
            return secure;
        }

        public static string Insecure(this SecureString input)
        {
            if (input == null)
            {
                return null;
            }

            string returnValue;
            var ptr = Marshal.SecureStringToBSTR(input);
            try
            {
                returnValue = Marshal.PtrToStringBSTR(ptr);
            }
            finally
            {
                Marshal.ZeroFreeBSTR(ptr);
            }
            return returnValue;
        }
    }
}