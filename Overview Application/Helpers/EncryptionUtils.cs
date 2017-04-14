﻿using System;
using System.Security.Cryptography;
using System.Text;

namespace OverviewApp.Helpers
{
    public static class EncryptionUtils
    {
        /// <summary>
        ///     Used for decrypting db passwords.
        /// </summary>
        public static string Unprotect(string encryptedString)
        {
            byte[] buffer;
            try
            {
                buffer = ProtectedData.Unprotect(Convert.FromBase64String(encryptedString), null,
                    DataProtectionScope.CurrentUser);
            }
            catch (Exception)
            {
                //if it's empty or incorrectly formatted, we get an exception. Just return an empty string.
                return "";
            }
            return Encoding.Unicode.GetString(buffer);
        }

        /// <summary>
        ///     Used for encrypting db passwords.
        /// </summary>
        public static string Protect(string unprotectedString)
        {
            var buffer = ProtectedData.Protect(Encoding.Unicode.GetBytes(unprotectedString), null,
                DataProtectionScope.CurrentUser);

            return Convert.ToBase64String(buffer);
        }
    }
}