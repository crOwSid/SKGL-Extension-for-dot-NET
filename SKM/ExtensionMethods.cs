﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKGL
{
    /// <summary>
    /// Some extension methods that allow you to use SKGL Extension as a Fluent API.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Checks that the Key Information object is valid (in the correct format). You can always add constraints
        /// such as @<see cref="HasNotExpired"/>.
        /// </summary>
        /// <param name="keyInformation"></param>
        /// <returns>Returns true if the object is valid and false otherwise.</returns>
        public static bool IsValid(this KeyInformation keyInformation)
        {
            if(keyInformation != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks that the Key Information object is valid (in the correct format). You can always add constraints
        /// such as @<see cref="HasNotExpired"/>.
        /// </summary>
        /// <param name="keyInformation"></param>
        /// <param name="rsaPublicKey">The public key (RSA). It can be found here: https://serialkeymanager.com/User/Security </param>
        /// <returns>Returns true if the object is valid and false otherwise.</returns>
        public static bool IsValid(this KeyInformation keyInformation, string rsaPublicKey)
        {
            if (keyInformation != null)
            {
                if(!SKGL.SKM.IsKeyInformationGenuine(keyInformation, rsaPublicKey))
                {
                    return false;
                }

                return true;
            }
            return false;
        }

        
        /// <summary>
        /// Checks that they key has not expired (i.e. the expire date has not been reached).
        /// </summary>
        /// <param name="keyInformation"></param>
        /// <param name="checkWithInternetTime">If set to true, we will also check that the local
        /// time (on the client computer) has not been changed (using SKM.TimeCheck). 
        /// </param>
        /// <returns>A key information object if the condition is satisfied. Null otherwise.</returns>
        public static KeyInformation HasNotExpired(this KeyInformation keyInformation, bool checkWithInternetTime = false)
        {
            if (keyInformation != null)
            {
                TimeSpan ts = keyInformation.ExpirationDate - DateTime.Today;

                if (ts.Days >= 0)
                {
                    if (checkWithInternetTime && SKGL.SKM.TimeCheck())
                        return null;
                    return keyInformation;
                }
            }
            return null;
        }

        /// <summary>
        /// Checks that this object has a valid signature, which means that the content has not been altered
        /// after that it was generated by Serial Key Manager.
        /// </summary>
        /// <param name="keyInformation"></param>
        /// <param name="rsaPublicKey">The public key (RSA). It can be found here: https://serialkeymanager.com/User/Security </param>
        /// <returns>A key information object if the condition is satisfied. Null otherwise.</returns>
        public static KeyInformation HasValidSignature(this KeyInformation keyInformation, string rsaPublicKey)
        {
            return HasValidSignature(keyInformation, rsaPublicKey, null);
        }


        /// <summary>
        /// Checks that this object has a valid signature, which means that the content has not been altered
        /// after that it was generated by Serial Key Manager.
        /// </summary>
        /// <param name="keyInformation"></param>
        /// <param name="rsaPublicKey">The public key (RSA). It can be found here: https://serialkeymanager.com/User/Security </param>
        /// <param name="signatureExpireInterval">If the activation object contains an activation date (when signDate=true),
        /// this method will check so that no more than "signatureExpirationInterval" days have passed since the last activation.
        /// </param>
        /// <returns>A key information object if the condition is satisfied. Null otherwise.</returns>
        public static KeyInformation HasValidSignature(this KeyInformation keyInformation, string rsaPublicKey, int? signatureExpirationInterval)
        {
            if (keyInformation != null)
            {
                if (SKGL.SKM.IsKeyInformationGenuine(keyInformation, rsaPublicKey))
                {
                    if(signatureExpirationInterval.HasValue && keyInformation.Date.HasValue)
                    {
                        TimeSpan ts = DateTime.Today -  keyInformation.Date.Value;
                        if(ts.Days >= signatureExpirationInterval.Value)
                        {
                            return null;
                        }
                    }

                    return keyInformation;
                }
            }
            return null;
        }


        /// <summary>
        /// Save the current object into a file. It can be read using @<see cref="LoadFromFile"/>.
        /// </summary>
        /// <param name="keyInformation"></param>
        /// <param name="file">The entire path including file name, i.e. c:\folder\file.txt</param>
        /// <returns>A key information object if the condition is satisfied. Null otherwise.</returns>
        public static KeyInformation SaveToFile(this KeyInformation keyInformation, string file = "")
        {
            return SaveToFile(keyInformation, file);
        }

        /// <summary>
        /// Save the current object into a file. It can be read using @<see cref="LoadFromFile"/>.
        /// </summary>
        /// <param name="keyInformation"></param>
        /// <param name="file">The entire path including file name, i.e. c:\folder\file.txt</param>
        /// <param name="json">If the file is stored in JSON (eg. an activation file with .skm extension), set this parameter to TRUE.</param>
        /// <returns>A key information object if the condition is satisfied. Null otherwise.</returns>
        /// <remarks>This method does not use the same JSON format structure as activation files. Instead,
        /// if you want to read these files using <see cref="LoadFromFile"/>, then activationFile has
        /// to be set to FALSE.</remarks>
        public static KeyInformation SaveToFile(this KeyInformation keyInformation, string file = "", bool json = false)
        {
            if (keyInformation != null)
            {
                if(SKGL.SKM.SaveKeyInformationToFile(keyInformation, file, json))
                {
                    return keyInformation;
                }
            }
            return null;
        }


        /// <summary>
        /// Load a saved object from file (using @<see cref="SaveToFile"/>).
        /// </summary>
        /// <param name="keyInformation"></param>
        /// <param name="file">The entire path including file name, i.e. c:\folder\file.txt</param>
        /// <returns>A key information object if the condition is satisfied. Null otherwise.</returns>
        public static KeyInformation LoadFromFile(this KeyInformation keyInformation, string file = "")
        {
            return LoadFromFile(keyInformation, file);
        }

        /// <summary>
        /// Load a saved object from file (using @<see cref="SaveToFile"/>).
        /// </summary>
        /// <param name="keyInformation"></param>
        /// <param name="file">The entire path including file name, i.e. c:\folder\file.txt</param>
        /// <param name="json">If the file is stored in JSON (eg. an activation file with .skm extension), set this parameter to TRUE.</param>
        /// <param name="activationFile">If you obtained this file from an Activation Form (.skm extension), this should be set to true.</param>
        /// <returns>A key information object if the condition is satisfied. Null otherwise.</returns>
        public static KeyInformation LoadFromFile(this KeyInformation keyInformation, string file = "", bool json = false, bool activationFile = false)
        {
            return SKGL.SKM.LoadKeyInformationFromFile(file, json);
        }


        /// <summary>
        /// Checks so that a certain Feature is enabled (i.e. it's set to TRUE).
        /// </summary>
        /// <param name="keyInformation"></param>
        /// <param name="featureNumber">The feature number, eg. feature1, feature 2, etc. FeatureNumber can be 1,2,...,8.</param>
        /// <returns>A key information object if the condition is satisfied. Null otherwise.</returns>
        public static KeyInformation HasFeature(this KeyInformation keyInformation, int featureNumber)
        {
            if (keyInformation != null && featureNumber <= 8
                                       && featureNumber >= 1
                                       && keyInformation.Features[featureNumber - 1])
            {
                return keyInformation;
            }
            return null;
        }

        /// <summary>
        /// Checks so that a certain Feature is disabled (i.e. it's set to FALSE).
        /// </summary>
        /// <param name="keyInformation"></param>
        /// <param name="featureNumber">The feature number, eg. feature1, feature 2, etc. FeatureNumber can be 1,2,...,8.</param>
        /// <returns>A key information object if the condition is satisfied. Null otherwise.</returns>
        public static KeyInformation HasNotFeature(this KeyInformation keyInformation, int featureNumber)
        {
            if (keyInformation != null && featureNumber <= 8
                                       && featureNumber >= 1
                                       && !keyInformation.Features[featureNumber - 1])
            {
                return keyInformation;
            }
            return null;
        }

        /// <summary>
        /// Checks so that the machine code corresponds to the machine code of this computer.
        /// The default hash function is SHA1.
        /// </summary>
        /// <param name="keyInformation"></param>
        /// <returns></returns>
        public static KeyInformation IsOnRightMachine(this KeyInformation keyInformation)
        {
            return IsOnRightMachine(keyInformation, SKM.getSHA1);
        }

        /// <summary>
        /// Checks so that the machine code corresponds to the machine code of this computer.
        /// </summary>
        /// <param name="keyInformation"></param>
        /// <param name="hashFunction">A hash function used to hash the current computer's parameters.</param>
        /// <returns></returns>
        public static KeyInformation IsOnRightMachine(this KeyInformation keyInformation, Func<string,string> hashFunction)
        {
            if (keyInformation != null && SKM.getMachineCode(hashFunction)
                                             .Equals(keyInformation.Mid))
            {

                return keyInformation;
            }
            return null;
        }
    }
}
